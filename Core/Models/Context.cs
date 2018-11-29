using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Core.Models
{
    public class Context : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountMovement> AccountMovements { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<ItemMovement> ItemMovements { get; set; }
        public DbSet<ItemPromotion> ItemPromotions { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<PaymentContract> PaymentContracts { get; set; }
        public DbSet<PaymentContractDetail> PaymentContractDetails { get; set; }
        public DbSet<PaymentSchedual> PaymentSchedual { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<PointOfSale> PointOfSales { get; set; }
        public DbSet<Range> Ranges { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vat> Vats { get; set; }
        public DbSet<VatDetail> VatDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess = true)
        {
            generateTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            generateTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void generateTimestamps()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["createdAt"] = DateTime.Now;
                        entry.CurrentValues["deletedAt"] = null;
                        break;

                    case EntityState.Modified:
                        entry.CurrentValues["updatedAt"] = DateTime.Now;
                        entry.CurrentValues["deletedAt"] = null;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["deletedAt"] = DateTime.Now;
                        break;
                }
            }
        }
    }
}
