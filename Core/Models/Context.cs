using System;
using Microsoft.EntityFrameworkCore;

namespace Core.Models
{
    public class Context : DbContext
    {
        public Context()
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemMovement> ItemMovements { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Range> Ranges { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vat> Vats { get; set; }
        public DbSet<VatDetail> VatDetails { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }
}
