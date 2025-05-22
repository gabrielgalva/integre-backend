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

            // Não force o tipo de coluna para SQL Server, use o padrão (nvarchar)
            // Se quiser customizar, use HasMaxLength para strings
            // Exemplo:
            // builder.Entity<Company>().Property(c => c.Email).HasMaxLength(256);
        }
    }
}