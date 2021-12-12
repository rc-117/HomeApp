namespace Homeapp.Test
{
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Identity;
    using System;
    using System.Collections.Generic;

    public static class TestRepo
    {
        public static List<Account> Accounts = new List<Account>
            {
                new Account()
                {
                    Id = new Guid("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4"),
                    Name = "Checkings",
                    StartingBalance = 120.34
                },
                new Account()
                {
                    Id = new Guid("936DA01F-9ABD-4d9d-80C7-02AF85C822A8"),
                    Name = "Savings",
                    StartingBalance = 500
                }
            };

        public static List<User> Users = new List<User>()
        {
            new User()
            {
                Id = new Guid("e79d653c-c5db-4740-8edd-87d3851f039d"),
                EmailAddress = "test1@mail.com",
                PasswordHash = "5E884898DA28047151D0E56F8DC6292773603D0D6AABBDD62A11EF721D1542D8",
                FirstName = "John",
                LastName = "Doe"
            },

            new User()
            {
                Id = new Guid("c79aa2d4-1c0e-4fd1-9eb3-55b7d9f0051c"),
                EmailAddress = "test2@mail.com",
                PasswordHash = "6CF615D5BCAAC778352A8F1F3360D23F02F34EC182E259897FD6CE485D7870D4",
                FirstName = "John",
                LastName = "Doe"
            }
        };
    }
}
