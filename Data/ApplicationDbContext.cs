using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SAMHO.Models;

namespace SAMHO.Data
{    
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /*public ApplicationDbContext()
        {
        }*/

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.HasDefaultSchema("Identity");
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User").HasIndex(u=>u.Identificacion).IsUnique();
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");

            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }

        //Entidades

        public DbSet<SAMHO.Models.Estado>? Estado { get; set; }
        public DbSet<SAMHO.Models.Pais>? Pais { get; set; }
        public DbSet<SAMHO.Models.Especialidad>? Especialidad { get; set; }
        public DbSet<SAMHO.Models.HorarioTrabajo>? HorarioTrabajo { get; set; }
    }
}