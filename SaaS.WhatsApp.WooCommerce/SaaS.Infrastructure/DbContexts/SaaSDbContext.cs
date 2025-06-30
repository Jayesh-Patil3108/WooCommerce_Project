using Microsoft.EntityFrameworkCore;
using SaaS.Domain.Entities;


namespace SaaS.Infrastructure.DbContexts
{
    public class SaaSDbContext : DbContext
    {
        public SaaSDbContext(DbContextOptions<SaaSDbContext> options) : base(options) {}
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<ClientWooSetting> ClientWooSettings => Set<ClientWooSetting>();
        public DbSet<ClientWhatsAppSetting> ClientWhatsAppSettings => Set<ClientWhatsAppSetting>();
        public DbSet<WooOrder> WooOrders => Set<WooOrder>();
        public DbSet<WooCustomer> WooCustomers => Set<WooCustomer>();
        public DbSet<WooProduct> WooProducts => Set<WooProduct>();
        public DbSet<WhatsAppMessageLog> WhatsAppMessageLogs => Set<WhatsAppMessageLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            object value = modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<ClientWooSetting>().ToTable("ClientWooSetting");
            modelBuilder.Entity<ClientWhatsAppSetting>().ToTable("ClientWhatsAppSetting");
            modelBuilder.Entity<WooOrder>().ToTable("WooOrder");
            modelBuilder.Entity<WooCustomer>().ToTable("WooCustomer");
            modelBuilder.Entity<WooProduct>().ToTable("WooProduct");
            modelBuilder.Entity<WhatsAppMessageLog>().ToTable("WhatsAppMessageLog");
        }

    }
}
