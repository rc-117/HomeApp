namespace Homeapp.Test
{
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Identity;
    using System;
    using System.Collections.Generic;

    public static class TestRepo
    {
        #region Accounts
        public static List<Account> Accounts = new List<Account>
            {
                new Account()
                {
                    Id = new Guid("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4"),
                    User = new User()
                        {
                            Id = new Guid("e79d653c-c5db-4740-8edd-87d3851f039d"),
                            EmailAddress = "john@mail.com",
                            PasswordHash = "5E884898DA28047151D0E56F8DC6292773603D0D6AABBDD62A11EF721D1542D8",
                            FirstName = "John",
                            LastName = "Doe"
                        },
                    UserId = new Guid("e79d653c-c5db-4740-8edd-87d3851f039d"),
                    Name = "Checkings",
                    StartingBalance = 120.34
                },
                new Account()
                {
                    Id = new Guid("936DA01F-9ABD-4d9d-80C7-02AF85C822A8"),
                    User = new User()
                        {
                            Id = new Guid("c79aa2d4-1c0e-4fd1-9eb3-55b7d9f0051c"),
                            EmailAddress = "axel@mail.com",
                            PasswordHash = "6CF615D5BCAAC778352A8F1F3360D23F02F34EC182E259897FD6CE485D7870D4",
                            FirstName = "Axel",
                            LastName = "Carino"
                        },
                    UserId = new Guid("c79aa2d4-1c0e-4fd1-9eb3-55b7d9f0051c"),
                    Name = "Savings",
                    StartingBalance = 500
                },
                                new Account()
                {
                    Id = new Guid("2cd10fe9-7a87-447f-8449-74db14a6dabc"),
                    User = new User()
                        {
                            Id = new Guid("c79aa2d4-1c0e-4fd1-9eb3-55b7d9f0051c"),
                            EmailAddress = "axel@mail.com",
                            PasswordHash = "6CF615D5BCAAC778352A8F1F3360D23F02F34EC182E259897FD6CE485D7870D4",
                            FirstName = "Axel",
                            LastName = "Carino"
                        },
                    UserId = new Guid("c79aa2d4-1c0e-4fd1-9eb3-55b7d9f0051c"),
                    Name = "Checkings",
                    StartingBalance = 1589.58
                }
            };
        #endregion

        #region Users
        public static List<User> Users = new List<User>()
        {
            // am9obkBtYWlsLmNvbTo1RTg4NDg5OERBMjgwNDcxNTFEMEU1NkY4REM2MjkyNzczNjAzRDBENkFBQkJERDYyQTExRUY3MjFEMTU0MkQ4
            new User()
            {
                Id = new Guid("e79d653c-c5db-4740-8edd-87d3851f039d"),
                EmailAddress = "john@mail.com",
                PasswordHash = "5E884898DA28047151D0E56F8DC6292773603D0D6AABBDD62A11EF721D1542D8",
                FirstName = "John",
                LastName = "Doe"
            },

            // YXhlbEBtYWlsLmNvbTo2Q0Y2MTVENUJDQUFDNzc4MzUyQThGMUYzMzYwRDIzRjAyRjM0RUMxODJFMjU5ODk3RkQ2Q0U0ODVENzg3MEQ0
            new User()
            {
                Id = new Guid("c79aa2d4-1c0e-4fd1-9eb3-55b7d9f0051c"),
                EmailAddress = "axel@mail.com",
                PasswordHash = "6CF615D5BCAAC778352A8F1F3360D23F02F34EC182E259897FD6CE485D7870D4",
                FirstName = "Axel",
                LastName = "Carino"
            },

            // cmljaGFyZEBtYWlsLmNvbTo1OTA2QUMzNjFBMTM3RTJEMjg2NDY1Q0Q2NTg4RUJCNUFDM0Y1QUU5NTUwMDExMDBCQzQxNTc3QzNENzUxNzY0
            new User()
            {
                Id = new Guid("bf1eff92-ce26-4e45-a97e-96cd7f4a3a4c"),
                EmailAddress = "richard@mail.com",
                PasswordHash = "5906AC361A137E2D286465CD6588EBB5AC3F5AE955001100BC41577C3D751764",
                FirstName = "Richard",
                LastName = "Carino"
            },

            // Z2VvcmdlQG1haWwuY29tOjJDMzA5Mjc1OUU2NUFEMDlBNUU3MThEOUNFOUI1MzU3QkI1QkQ0M0QxQzE0Q0Y4QjEzMDBGODAyQjczMUI4ODg=
            new User()
            {
                Id = new Guid("76d50506-7e6b-40f9-a083-5c3734a4c79e"),
                EmailAddress = "george@mail.com",
                PasswordHash = "2C3092759E65AD09A5E718D9CE9B5357BB5BD43D1C14CF8B1300F802B731B888",
                FirstName = "George",
                LastName = "Benson"
            },


        };
        #endregion
    }
}