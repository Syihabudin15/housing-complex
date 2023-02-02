using HousingComplex.Entities;
using Microsoft.EntityFrameworkCore;

namespace HousingComplex.Repositories
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserCredential> UserCredentials => Set<UserCredential>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Developer> Developers => Set<Developer>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Housing> Housings => Set<Housing>();
        public DbSet<HouseType> HouseTypes => Set<HouseType>();
        public DbSet<Spesification> Spesifications => Set<Spesification>();
        public DbSet<ImageHouseType> ImageHouseTypes => Set<ImageHouseType>();
        public DbSet<Meet> Meets => Set<Meet>();
        public DbSet<Transaction> Purchases => Set<Transaction>();
        public DbSet<TransactionDetail> PurchaseDetails => Set<TransactionDetail>();

        protected AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasIndex(cs => cs.PhoneNumber).IsUnique();
            modelBuilder.Entity<Developer>().HasIndex(dev => dev.PhoneNumber).IsUnique();
            modelBuilder.Entity<UserCredential>().HasIndex(uc => uc.Email).IsUnique();
        }
    }
}
