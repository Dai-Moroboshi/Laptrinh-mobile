using Microsoft.EntityFrameworkCore;
using web_api_p5.Models;

namespace web_api_p5.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationItem> ReservationItems { get; set; }
        public DbSet<TableEntity> Tables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure unique constraints
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Reservation>()
                .HasIndex(r => r.ReservationNumber)
                .IsUnique();

            modelBuilder.Entity<TableEntity>()
                .HasIndex(t => t.TableNumber)
                .IsUnique();
                
            // Configure relationships
             modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Customer)
                .WithMany()
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

             modelBuilder.Entity<ReservationItem>()
                .HasOne(ri => ri.Reservation)
                .WithMany(r => r.ReservationItems)
                .HasForeignKey(ri => ri.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);
                
             modelBuilder.Entity<ReservationItem>()
                .HasOne(ri => ri.MenuItem)
                .WithMany()
                .HasForeignKey(ri => ri.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
