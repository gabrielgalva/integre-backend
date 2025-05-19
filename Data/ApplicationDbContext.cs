using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IntegreBackend.Models;
using Microsoft.AspNetCore.Identity;

namespace IntegreBackend.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Company> Companies { get; set; }

        // Novas tabelas para registros de cursos
            public DbSet<RegisterFrontend> RegisterFrontends { get; set; }
            public DbSet<RegisterBackend> RegisterBackends { get; set; }
            public DbSet<RegisterDatabase> RegisterDatabases { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuração da Tabela ApplicationUser
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.UserName).HasColumnType("TEXT");
                entity.Property(u => u.NormalizedUserName).HasColumnType("TEXT");
                entity.Property(u => u.Email).HasColumnType("TEXT");
                entity.Property(u => u.NormalizedEmail).HasColumnType("TEXT");
                entity.Property(u => u.PasswordHash).HasColumnType("TEXT");
                entity.Property(u => u.SecurityStamp).HasColumnType("TEXT");
                entity.Property(u => u.ConcurrencyStamp).HasColumnType("TEXT");
                entity.Property(u => u.PhoneNumber).HasColumnType("TEXT");
                entity.Property(u => u.Cargo).HasColumnType("TEXT");
                entity.Property(u => u.Nome).HasColumnType("TEXT");
            });

            // Configuração da Tabela de Roles
            builder.Entity<IdentityRole>(entity =>
            {
                entity.Property(r => r.Name).HasColumnType("TEXT");
                entity.Property(r => r.NormalizedName).HasColumnType("TEXT");
                entity.Property(r => r.ConcurrencyStamp).HasColumnType("TEXT");
            });

            // Configuração global para strings como TEXT no SQLite
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    if (property.ClrType == typeof(string) && property.GetColumnType() == null)
                    {
                        property.SetColumnType("TEXT");
                    }
                }
            }
        }
    }
}
