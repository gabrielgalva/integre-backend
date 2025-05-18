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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Ajustar Identity para SQLite
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

            builder.Entity<IdentityRole>(entity =>
            {
                entity.Property(r => r.Name).HasColumnType("TEXT");
                entity.Property(r => r.NormalizedName).HasColumnType("TEXT");
                entity.Property(r => r.ConcurrencyStamp).HasColumnType("TEXT");
            });

            // Ajuste global para todas as strings
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
