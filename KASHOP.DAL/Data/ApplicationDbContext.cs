using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbSet<Category> Categories { get; set; } 
        public DbSet<CategoryTranslation> CatergoryTranslations { get; set;  }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTranslation> ProductTraslations { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<BrandTranslation> BrandTranslations { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IHttpContextAccessor HttpContextAccessor) 
            :base(options)
        {
            _httpContextAccessor = HttpContextAccessor;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("User");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");



            builder.Entity<Category>()
                .HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Category>()
              .HasOne(p => p.UpdatedBy)
              .WithMany()
              .HasForeignKey(p => p.UpdatedById)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
               .HasOne(p => p.CreatedBy)
               .WithMany()
               .HasForeignKey(p => p.CreatedById)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
              .HasOne(p => p.UpdatedBy) 
              .WithMany()
              .HasForeignKey(p => p.UpdatedById)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Brand>()
               .HasOne(p => p.CreatedBy)
               .WithMany()
               .HasForeignKey(p => p.CreatedById)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Brand>()
              .HasOne(p => p.UpdatedBy)
              .WithMany()
              .HasForeignKey(p => p.UpdatedById)
              .OnDelete(DeleteBehavior.Restrict);

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var currentUser = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var entries = ChangeTracker.Entries<AuditableEntity>();
                foreach (var entry in entries)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property(x => x.CreatedById).CurrentValue = currentUser;
                        entry.Property(x => x.CreatedOn).CurrentValue = DateTime.UtcNow;
                    }
                    if (entry.State == EntityState.Modified)
                    {
                        entry.Property(x => x.UpdatedById).CurrentValue = currentUser;
                        entry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;

                    }

                }

        }
            return base.SaveChangesAsync(cancellationToken);
        }
       
    }
}
