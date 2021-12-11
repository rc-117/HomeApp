namespace DummyRepo
{
    using DummyRepo.Entities;
    using System;
    using System.Collections.Generic;

    public static class Repo
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
    }
}
