using CafeManagement.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace CafeManagement.API.Infrastructure.Persistence
{
    public class CafeManagementDbContext : DbContext
    {
        public DbSet<MCustomer> Customers { get; set; }
        public DbSet<MEmployee> Employees { get; set; }
        public DbSet<MMenuItem> MenuItems { get; set; }
        public DbSet<MOrder> Orders { get; set; }
        public DbSet<MTable> Tables { get; set; }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public CafeManagementDbContext(DbContextOptions<CafeManagementDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("admin");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MMenuItem>()
                .Property(m => m.PicturesUrl)
                .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>()
                );

            modelBuilder.Entity<MMenuItem>()
                .Property(m => m.PicturesUrl)
                .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                ));


            modelBuilder.Entity<MOrder>()
                .HasIndex(o => o.AssignedWaiterId);

            modelBuilder.Entity<MOrder>()
                .HasIndex(o => o.AssignedBaristaId);

            modelBuilder.Entity<MOrder>()
                .HasIndex(o => o.AssignedChefId);

            modelBuilder.Entity<MOrder>()
                .HasIndex(o => o.AssignedCashierId);


        }



    }
}
