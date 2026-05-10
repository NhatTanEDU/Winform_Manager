using Microsoft.EntityFrameworkCore;
using RMA.Server.Entities;

namespace RMA.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ComponentChecklist> ComponentChecklists { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<RmaTicket> RmaTickets { get; set; }
        public DbSet<StatusHistory> StatusHistories { get; set; }
        public DbSet<StatusMaster> StatusMasters { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Fix multiple cascade paths issue
            modelBuilder.Entity<RmaTicket>()
                .HasOne(r => r.Device)
                .WithMany(d => d.RmaTickets)
                .HasForeignKey(r => r.DeviceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RmaTicket>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.RmaTickets)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
