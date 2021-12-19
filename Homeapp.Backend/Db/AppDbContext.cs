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
        public DbSet<User> Users { get; set; }
        public DbSet<UserHousehold> UserHouseholds { get; set; }
        public DbSet<UserHouseholdGroup> UserHouseholdGroups { get; set; }
        public DbSet<Household> Households { get; set; }
        public DbSet<HouseholdGroup> HouseholdGroups { get; set; }
        
        // Checkbook dbSets
        public DbSet<Account> Accounts { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<IncomeCategory> IncomeCategories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
