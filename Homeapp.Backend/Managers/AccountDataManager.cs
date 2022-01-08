namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Db;
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Identity;
    using Homeapp.Backend.Tools;
    using Homeapp.Test;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// The Account Manager.
    /// </summary>
    public class AccountDataManager : HomeappDataManagerBase, IAccountDataManager
    {
        /// <summary>
        /// The user data manager.
        /// </summary>
        private IUserDataManager userDataManager;

        /// <summary>
        /// The allowed user data manager.
        /// </summary>
        private IAllowedUsersDataManager allowedUsersManager;

        /// <summary>
        /// The db context for the database.
        /// </summary>
        private AppDbContext appDbContext;

        /// <summary>
        /// Initializes the AccountDataManager.
        /// </summary>
        /// <param name="userDataManager">The user data manager class.</param>
        /// <param name="allowedUsersDataManager">The allowed user data manager.</param>
        /// <param name="appDbContext">The app db context.</param>
        public AccountDataManager(
            IUserDataManager userDataManager,
            IAllowedUsersDataManager allowedUsersDataManager,
            AppDbContext appDbContext)
        {
            this.userDataManager = userDataManager;
            this.allowedUsersManager = allowedUsersDataManager;
            this.appDbContext = appDbContext;
        }

        /// <summary>
        /// Gets an account by its id.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>An account, if any. Returns null if no account is found.</returns>
        public Account GetAccountById(Guid accountId)
        {
            return this.appDbContext.Accounts
                .FirstOrDefault(account => account.Id == accountId);
        }

        /// <summary>
        /// Gets all of a user's accounts.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        public Account[] GetUserAccounts(Guid userId) 
        {
            return this.appDbContext
                .Accounts
                .Where(account => account.OwnerId == userId)
                .ToArray();
        }

        /// <summary>
        /// Gets all transactions from an account.
        /// </summary>
        /// <param name="userId">The account id.</param>
        public Transaction[] GetTransactionsByAccount(Guid accountId)
        {
            //test code
            var testGenerator = new TransactionGenerator();
            return testGenerator.Transactions
                .Where(transaction => transaction.AccountId == accountId)
                .ToArray();
        }

        /// <summary>
        /// Calculates the balance of a specified account.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        public double CalculateAccountBalance(Account account)
        {
            if (this.GetTransactionsByAccount(account.Id).Length == 0)
            {
                return account.StartingBalance;
            }

            double totalincome = 0;
            double totalexpense = 0;

            var incomeList = this.GetTransactionsByAccount(account.Id)
                .Where(t => t.TransactionType == TransactionType.Income);

            var expenseList = this.GetTransactionsByAccount(account.Id)
                .Where(t => t.TransactionType == TransactionType.Expense ||
                            t.TransactionType == TransactionType.Transfer);

            foreach (var income in incomeList)
            {
                totalincome += income.Amount;
            }
            foreach (var expense in expenseList)
            {
                totalexpense += expense.Amount;
            }

            return GetAccountById(account.Id)
                .StartingBalance 
                + totalincome 
                - totalexpense;
        }

        /// <summary>
        /// Creates an account for a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="request">The user's account request.</param>
        /// <param name="allowedUsers">The object containing the allowed entities for this account.</param>
        public async Task<Account> CreateAccount(
            User user, 
            CreateAccountRequest request, 
            AllowedUsers allowedUsers)
        {
            var account = new Account()
            {
                Name = request.Name,
                AccountType = (AccountType)Enum.Parse(typeof(AccountType), request.AccountType),
                StartingBalance = request.StartingBalance,
                OwnerId = user.Id,
            };

            account.AllowedUsers = allowedUsers;

            try
            {
                appDbContext.Accounts.Add(account);
                await appDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new HttpResponseException(
                   new HttpResponseMessage(HttpStatusCode.InternalServerError)
                   {
                       Content = new StringContent("There was an error when saving the account to database."),
                       ReasonPhrase = HttpReasonPhrase
                           .GetPhrase(ReasonPhrase.ErrorSavingToDatabase)
                   });
            }

            return account;
        }

        /// <summary>
        /// Creates an Income Category record in the database for the requesting user.
        /// </summary>
        /// <param name="ownerId">The requesting user's id.</param>
        /// <param name="request">The request body containing properties to create the Income Category.</param>
        public async Task<IncomeCategory> CreateIncomeCategoryForUser(Guid ownerId, IncomeCategoryRequest request)
        {
            var category = new IncomeCategory()
            {
                Name = request.Name,
                Description =
                    String.IsNullOrWhiteSpace(request.Description) ?
                    null : request.Description,
                CreatingUser = this.userDataManager.GetUserFromUserId(ownerId),
                SharedEntities = 
                    this.allowedUsersManager
                    .CreateNewAllowedUsersObject(request.AllowedUsersRequest)
            };

            try
            {
                this.appDbContext.IncomeCategories.Add(category);
                await this.appDbContext.SaveChangesAsync();

                return category;
            }
            catch (Exception)
            {
                throw new HttpResponseException(
                   new HttpResponseMessage(HttpStatusCode.InternalServerError)
                   {
                       Content = new StringContent("There was an error when saving an income category to the database."),
                       ReasonPhrase = HttpReasonPhrase
                           .GetPhrase(ReasonPhrase.ErrorSavingToDatabase)
                   });
            }
        }

        /// <summary>
        /// Gets an Income Category record from the database using its id.
        /// </summary>
        /// <param name="categoryId">The unique id of the category.</param>
        public IncomeCategory GetIncomeCategoryById(Guid categoryId)
        {
            try
            {
                return this.appDbContext
                    .IncomeCategories
                    .FirstOrDefault(i => i.Id == categoryId);
            }
            catch (Exception)
            {
                throw new HttpResponseException(
                   new HttpResponseMessage(HttpStatusCode.InternalServerError)
                   {
                       Content = new StringContent("There was an error when saving retrieving an income category from the database."),
                       ReasonPhrase = HttpReasonPhrase
                           .GetPhrase(ReasonPhrase.ErrorRetrievingFromDatabase)
                   });
            }
        }

        /// <summary>
        /// Creates an expense category record in the database for the requesting user.
        /// </summary>
        /// <param name="ownerId">The requesting user's id.</param>
        /// <param name="request">The request body containing properties to create the expense category.</param>
        public async Task<ExpenseCategory> CreateExpenseCategoryForUser(Guid ownerId, ExpenseCategoryRequest request)
        {
            var category = new ExpenseCategory()
            {
                Name = request.Name,
                Description =
                    String.IsNullOrWhiteSpace(request.Description) ?
                    null : request.Description,
                CreatingUser = this.userDataManager.GetUserFromUserId(ownerId),
                SharedEntities = 
                    this.allowedUsersManager
                    .CreateNewAllowedUsersObject(request.AllowedUsersRequest)
            };

            try
            {
                this.appDbContext.ExpenseCategories.Add(category);
                await this.appDbContext.SaveChangesAsync();

                return category;
            }
            catch (Exception)
            {
                throw new HttpResponseException(
                   new HttpResponseMessage(HttpStatusCode.InternalServerError)
                   {
                       Content = new StringContent("There was an error when saving an expense category to the database."),
                       ReasonPhrase = HttpReasonPhrase
                           .GetPhrase(ReasonPhrase.ErrorSavingToDatabase)
                   });
            }
        }

        /// <summary>
        /// Gets an expense category record from the database using its id.
        /// </summary>
        /// <param name="categoryId">The unique id of the category.</param>
        public ExpenseCategory GetExpenseCategoryById(Guid categoryId)
        {
            try
            {
                return this.appDbContext
                    .ExpenseCategories
                    .FirstOrDefault(i => i.Id == categoryId);
            }
            catch (Exception)
            {
                throw new HttpResponseException(
                   new HttpResponseMessage(HttpStatusCode.InternalServerError)
                   {
                       Content = new StringContent("There was an error when retrieving an expense category from the database."),
                       ReasonPhrase = HttpReasonPhrase
                           .GetPhrase(ReasonPhrase.ErrorRetrievingFromDatabase)
                   });
            }
        }

        /// <summary>
        /// Creates a transaction record in a specified checkbook account.
        /// </summary>
        /// <param name="accountOwnerId">The id of the account owner.</param>
        /// <param name="accountId">The account id.</param>
        /// <param name="transactionOwnerId">The id of the user who created the transaction.</param>
        /// <param name="request">The request object containing properties to create the record.</param>
        public async Task<Transaction> CreateTransactionInAccount(
            Guid accountOwnerId, 
            Guid accountId,
            Guid transactionOwnerId,
            TransactionRequest request)
        {
            var transactionType = (TransactionType)Enum.Parse
                    (typeof(TransactionType), request.TransactionType);

            var transaction = new Transaction()
            {
                Name = request.Name,
                Amount = request.Amount,
                TransactionType = transactionType,
                TransferToExternalAccount = request.TransferToExternalAccount,
                TransferFromExternalAccount = request.TransferFromExternalAccount,
                DateTime = DateTime.Parse(request.DateTime),
                Account = this.GetAccountById(accountId),
                IsCleared = request.IsCleared,
            };

            if (transactionType == TransactionType.Income || transactionType == TransactionType.Transfer)
            {
                if (request.IncomeCategoryId == null)
                {
                    transaction.IncomeCategory = 
                        request.IncomeCategoryRequest == null ? 
                        null : await this.CreateIncomeCategoryForUser
                            (ownerId: transactionOwnerId, request: request.IncomeCategoryRequest);
                }
                else
                {
                    transaction.IncomeCategory = 
                        GetIncomeCategoryById(Guid.Parse(request.IncomeCategoryId));
                }
            }
            else if (transactionType == TransactionType.Expense || transactionType == TransactionType.Transfer)
            {
                if (request.ExpenseCategoryId == null)
                {
                    transaction.ExpenseCategory =
                        request.ExpenseCategoryRequest == null ?
                        null : await this.CreateExpenseCategoryForUser
                            (ownerId: transactionOwnerId, request: request.ExpenseCategoryRequest);
                }
                else
                {
                    transaction.ExpenseCategory =
                        this.GetExpenseCategoryById(Guid.Parse(request.ExpenseCategoryId));
                }
            }

            if (transactionType == TransactionType.Transfer)
            {
                if (!request.TransferFromExternalAccount && !request.TransferToExternalAccount)
                {
                    transaction.AccountIdToTransferTo = Guid.Parse(request.AccountIdToTransferTo);
                }
            }

            transaction.RecurringTransaction = 
                !request.IsRecurringTransaction ? 
                null : this.GetRecurringTransactionById(Guid.Parse(request.RecurringTransactionId));

            transaction.AllowedUsers = this.allowedUsersManager.CreateNewEmptyAllowedUsersObject();

            try
            {
                this.appDbContext.Transactions.Add(transaction);
                await this.appDbContext.SaveChangesAsync();

                return transaction;
            }
            catch (Exception)
            {
                throw new HttpResponseException(
                   new HttpResponseMessage(HttpStatusCode.InternalServerError)
                   {
                       Content = new StringContent("There was an error when saving the transaction to database."),
                       ReasonPhrase = HttpReasonPhrase
                           .GetPhrase(ReasonPhrase.ErrorSavingToDatabase)
                   });
            }
        }

        /// <summary>
        /// Gets a recurring transaction record from the database using its id.
        /// </summary>
        /// <param name="id">The id of the recurring transaction record.</param>
        public RecurringTransaction GetRecurringTransactionById(Guid id)
        {
            return this.appDbContext
                .RecurringTransactions
                .FirstOrDefault(r => r.Id == id);
        }
    }
}
