using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IntegreBackend.Models;
using Microsoft.AspNetCore.Identity;

namespace IntegreBackend.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<RegisterFrontend> RegisterFrontends { get; set; }
        public DbSet<RegisterBackend> RegisterBackends { get; set; }
        public DbSet<RegisterDatabase> RegisterDatabases { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Mapeamento explícito das tabelas
            builder.Entity<RegisterFrontend>().ToTable("RegisterFrontend");
            builder.Entity<RegisterBackend>().ToTable("RegisterBackend");
            builder.Entity<RegisterDatabase>().ToTable("RegisterDatabase");
        }
    }
}
