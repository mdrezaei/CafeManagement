using MenuAndOrdering.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace MenuAndOrdering.API.Infrastructure.Persistence
{
    public class MenuAndOrderingDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Table> Tables { get; set; }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public MenuAndOrderingDbContext(DbContextOptions<MenuAndOrderingDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("menu");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MenuItem>()
                .Property(m => m.PicturesUrl)
                .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>()
                 );

            modelBuilder.Entity<MenuItem>()
            .Property(m => m.PicturesUrl)
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()
            ));


        }


    }
}
