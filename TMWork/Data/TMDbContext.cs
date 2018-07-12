using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TMWork.Data.Models.User;
using TMWork.Data.Models.Customer;
using TMWork.Data.Models.Invoice;

namespace TMWork.Data
{
    public class TMDbContext : IdentityDbContext<AuthUser,AuthRole,string>
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerApplianceProblem> CustomerApplianceProblems { get; set; }
        public DbSet<CustomerApplianceType> CustomerApplianceTypes { get; set; }
        public DbSet<CustomerApplianceBrand> CustomerApplianceBrands { get; set; }
        public DbSet<CustomerCoupon> CustomerCoupons { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<FileUpload> FileUploads { get; set; }
        public TMDbContext(DbContextOptions<TMDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AuthUser>(b => {
                b.HasMany(x => x.Roles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            });

            builder.Entity<FileUpload>()
                .HasOne(i => i.Invoice)
                .WithMany(i => i.FileUpload)
                .HasForeignKey(i => i.InvoiceId)
                .HasConstraintName("FK_FileUpload_Invoice")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CustomerApplianceProblem>()
                .HasOne(p => p.Customer)
                .WithMany(p => p.CustomerApplianceProblems)
                .HasForeignKey(p => p.CustomerId)
                .HasConstraintName("FK_CustomerApplianceProblem_Customer")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CustomerApplianceBrand>()
                .HasOne(p => p.customerApplianceType)
                .WithMany(p => p.CustomerApplianceBrands)
                .HasForeignKey(p => p.CustomerApplianceTypeId)
                .HasConstraintName("FK_CustomerApplianceBrand_Customer")
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<CustomerApplianceProblem>()
            //    .HasOne(p => p.CustomerApplianceType);


            //builder.Entity<CustomerApplianceProblem>()
            //    .HasOne(p => p.CustomerApplianceBrand);

            //builder.Entity<CustomerApplianceProblem>()
            //    .HasOne(c => c.CustomerApplianceType)                
            //    .WithOne()
            //    .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Restrict);

            //builder.Entity<CustomerApplianceProblem>()
            //    .HasOne(c => c.CustomerApplianceBrand)
            //    .WithOne()
            //    .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Restrict);

            //builder.Entity<CustomerApplianceType>()
            //  .HasMany(c => c.CustomerApplianceBrands)
            //  .WithOne()
            //  .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Restrict);

            //builder.Entity<CustomerApplianceProblem>()
            //   .HasOne(c => c.customerApplianceType)
            //   .WithOne()
            //   .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Restrict);

            //builder.Entity<CustomerApplianceProblem>()
            //   .HasOne(c => c.customerApplianceBrand)
            //   .WithOne()
            //   .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Restrict);


        }
    }
}
