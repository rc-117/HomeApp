namespace Homeapp.Backend.Db
{
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Identity;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        //Identity dbSets
        DbSet<User> Users { get; set; }

        // Checkbook dbSets
        DbSet<Account> Accounts { get; set; }
        DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        DbSet<IncomeCategory> IncomeCategories { get; set; }
        DbSet<Transaction> Transactions { get; set; }
    }
}
