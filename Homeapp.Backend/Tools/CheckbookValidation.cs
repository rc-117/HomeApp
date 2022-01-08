namespace Homeapp.Backend.Tools
{
    using Homeapp.Backend.Db;
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Managers;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    /// <summary>
    /// Validation class handling all checkbook/transaction related validation tasks.
    /// </summary>
    public static class CheckbookValidation
    {
        /// <summary>
        /// Validates that a specified checkbook account exists in the database.
        /// </summary>
        /// <param name="Id">The Id of the checkbook.</param>
        /// <param name="appDbContext">The application database context.</param>
        public static void CheckbookAccountExists(Guid Id, AppDbContext appDbContext)
        {
            var account = appDbContext.Accounts.FirstOrDefault(a => a.Id == Id);
            if (account == null)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent($"Checkbook account with id '{Id}' was not found."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.CheckbookAccountNotFound)
                    });
            }
        }

        /// <summary>
        /// Validates that values received in a create/modify transaction request are valid.
        /// </summary>
        /// <param name="request">The request object.</param>
        public static void ValidateTransactionRequest(TransactionRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Id))
            {
                CommonValidation.GuidIsValid(guid: request.Id, errorMessage: "Invalid transaction id received.");
            }

            CheckbookValidation.TransactionTypeIsValid(request.TransactionType);

            var transactionType = (TransactionType)Enum.Parse(typeof(TransactionType), request.TransactionType);

            CommonValidation.GuidIsValid(guid: request.OwnerId, "Invalid transaction owner id received.");

            if (transactionType == TransactionType.Income)
            {
                if (!string.IsNullOrWhiteSpace(request.ExpenseCategoryId))
                {
                    throw new HttpResponseException(
                        new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = 
                                new StringContent
                                    ("An invalid transaction request was received. An expense category id cannot be included in a request marked with a transaction type of 'Income'."),
                            ReasonPhrase = HttpReasonPhrase
                                .GetPhrase(ReasonPhrase.InvalidTransactionRequest)
                        });
                }
                else if (request.ExpenseCategoryRequest != null)
                {
                    throw new HttpResponseException(
                        new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content =
                                new StringContent
                                    ("An invalid transaction request was received. An expense category request cannot be included in a request marked with a transaction type of 'Income'."),
                            ReasonPhrase = HttpReasonPhrase
                                .GetPhrase(ReasonPhrase.InvalidTransactionRequest)
                        });
                }

                if (!string.IsNullOrWhiteSpace(request.IncomeCategoryId) && request.IncomeCategoryRequest != null)
                {
                    throw new HttpResponseException(
                        new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content =
                                new StringContent
                                    ("An invalid transaction request was received. The request cannot contain both an income category id and an income category request."),
                            ReasonPhrase = HttpReasonPhrase
                                .GetPhrase(ReasonPhrase.InvalidTransactionRequest)
                        });
                }
                else if (!string.IsNullOrWhiteSpace(request.IncomeCategoryId))
                {
                    CommonValidation.GuidIsValid(guid: request.IncomeCategoryId, errorMessage: "Invalid income category id received.");
                }
                else if (request.IncomeCategoryRequest != null)
                {
                    CheckbookValidation.ValidateIncomeCategoryRequest(request.IncomeCategoryRequest);
                }
            }
            else if (transactionType == TransactionType.Expense)
            {
                if (!string.IsNullOrWhiteSpace(request.IncomeCategoryId))
                {
                    throw new HttpResponseException(
                        new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content =
                                new StringContent
                                    ("An invalid transaction request was received. An income category id cannot be included in a request marked with a transaction type of 'Expense'."),
                            ReasonPhrase = HttpReasonPhrase
                                .GetPhrase(ReasonPhrase.InvalidTransactionRequest)
                        });
                }
                else if (request.IncomeCategoryRequest != null)
                {
                    throw new HttpResponseException(
                        new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content =
                                new StringContent
                                    ("An invalid transaction request was received. An income category request cannot be included in a request marked with a transaction type of 'Expense'."),
                            ReasonPhrase = HttpReasonPhrase
                                .GetPhrase(ReasonPhrase.InvalidTransactionRequest)
                        });
                }

                if (!string.IsNullOrWhiteSpace(request.ExpenseCategoryId) && request.ExpenseCategoryRequest != null)
                {
                    throw new HttpResponseException(
                        new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content =
                                new StringContent
                                    ("An invalid transaction request was received. The request cannot contain both an expense category id and an expense category request."),
                            ReasonPhrase = HttpReasonPhrase
                                .GetPhrase(ReasonPhrase.InvalidTransactionRequest)
                        });
                }
                else if (!string.IsNullOrWhiteSpace(request.ExpenseCategoryId))
                {
                    CommonValidation.GuidIsValid(guid: request.IncomeCategoryId, errorMessage: "Invalid expense category id received.");
                }
                else if (request.IncomeCategoryRequest != null)
                {
                    CheckbookValidation.ValidateExpenseCategoryRequest(request.ExpenseCategoryRequest);
                }
            }
            else if (transactionType == TransactionType.Transfer)
            {
                if (request.TransferToExternalAccount || request.TransferFromExternalAccount)
                {
                    if (!string.IsNullOrWhiteSpace(request.AccountIdToTransferTo))
                    {
                        throw new HttpResponseException(
                            new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content =
                                    new StringContent
                                        ("An invalid transaction request was received. An external transfer request cannot include an account id."),
                                ReasonPhrase = HttpReasonPhrase
                                    .GetPhrase(ReasonPhrase.InvalidTransactionRequest)
                            });
                    }
                }
            }

            CommonValidation.DateStringIsValid(date: request.DateTime, errorMessage: $"An invalid date was provided: '{request.DateTime}'.");


            if (request.IsRecurringTransaction)
            {
                if (string.IsNullOrWhiteSpace(request.RecurringTransactionId))
                {
                    throw new HttpResponseException(
                        new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content =
                                new StringContent
                                    ("An invalid transaction request was received. 'RecurringTransactionId' is required for recurring transaction requests."),
                            ReasonPhrase = HttpReasonPhrase
                                .GetPhrase(ReasonPhrase.InvalidTransactionRequest)
                        });
                }
                else
                {
                    CommonValidation
                        .GuidIsValid(
                            guid: request.RecurringTransactionId, 
                            errorMessage: $"An invalid recurring transaction id was received: '{request.RecurringTransactionId}'");
                }
            }
        }

        /// <summary>
        /// Validates that an income category request is valid.
        /// </summary>
        /// <param name="request">The request to validate.</param>
        public static void ValidateIncomeCategoryRequest(IncomeCategoryRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Id))
            {
                CommonValidation
                    .GuidIsValid(
                        guid: request.Id, 
                        errorMessage: $"An invalid income category id was received: '{request.Id}'.");
            }

            IdentityValidation.ValidateAllowedUsersRequest(request: request.AllowedUsersRequest);
        }

        /// <summary>
        /// Validates that an income category request is valid.
        /// </summary>
        /// <param name="request">The request to validate.</param>
        public static void ValidateExpenseCategoryRequest(ExpenseCategoryRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Id))
            {
                CommonValidation
                    .GuidIsValid(
                        guid: request.Id,
                        errorMessage: $"An invalid income category id was received: '{request.Id}'.");
            }

            IdentityValidation.ValidateAllowedUsersRequest(request: request.AllowedUsersRequest);
        }

        /// <summary>
        /// Validates that the received string can be converted into a TransactionType enum.
        /// </summary>
        /// <param name="type">The string to check.</param>
        public static void TransactionTypeIsValid(string type)
        {
            try
            {
                Enum.Parse(typeof(TransactionType), type);
            }
            catch (Exception)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent($"Invalid transaction type received: '{type}'."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.InvalidTransactionType)
                    });
            }
        }
    }
}