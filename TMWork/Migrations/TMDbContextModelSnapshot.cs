﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using TMWork.Data;

namespace TMWork.Migrations
{
    [DbContext(typeof(TMDbContext))]
    partial class TMDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TMWork.Data.Models.Customer.Contact", b =>
                {
                    b.Property<int>("ContactId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateUpdated");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Message")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.Property<string>("UpdatedBy");

                    b.HasKey("ContactId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("TMWork.Data.Models.Customer.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateUpdated");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("PhoneNumber")
                        .IsRequired();

                    b.Property<string>("PostalCode")
                        .IsRequired();

                    b.Property<string>("State")
                        .IsRequired();

                    b.Property<string>("UpdatedBy");

                    b.HasKey("CustomerId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("TMWork.Data.Models.Customer.CustomerApplianceBrand", b =>
                {
                    b.Property<int>("CustomerApplianceBrandId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<int>("CustomerApplianceTypeId");

                    b.Property<DateTime?>("DateCreated");

                    b.Property<DateTime?>("DateUpdated");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Sequence");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("CustomerApplianceBrandId");

                    b.HasIndex("CustomerApplianceTypeId");

                    b.ToTable("CustomerApplianceBrands");
                });

            modelBuilder.Entity("TMWork.Data.Models.Customer.CustomerApplianceProblem", b =>
                {
                    b.Property<int>("CustomerApplianceProblemId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("ActualScheduleTime");

                    b.Property<string>("CreatedBy");

                    b.Property<int>("CustomerApplianceBrandId");

                    b.Property<int>("CustomerApplianceTypeId");

                    b.Property<int>("CustomerId");

                    b.Property<DateTime?>("DateCreated");

                    b.Property<DateTime?>("DateResolved");

                    b.Property<DateTime?>("DateUpdated");

                    b.Property<DateTime>("DesiredScheduleTime");

                    b.Property<string>("IssueResolvedBy");

                    b.Property<string>("ModelNumber")
                        .IsRequired();

                    b.Property<string>("ModelSerial")
                        .IsRequired();

                    b.Property<string>("Problem")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("ProblemStatus");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("CustomerApplianceProblemId");

                    b.HasIndex("CustomerApplianceBrandId");

                    b.HasIndex("CustomerApplianceTypeId");

                    b.HasIndex("CustomerId");

                    b.ToTable("CustomerApplianceProblems");
                });

            modelBuilder.Entity("TMWork.Data.Models.Customer.CustomerApplianceType", b =>
                {
                    b.Property<int>("CustomerApplianceTypeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("DateCreated");

                    b.Property<DateTime?>("DateUpdated");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Sequence");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("CustomerApplianceTypeId");

                    b.ToTable("CustomerApplianceTypes");
                });

            modelBuilder.Entity("TMWork.Data.Models.Customer.CustomerCoupon", b =>
                {
                    b.Property<int>("CustomerCouponId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("DateCreated");

                    b.Property<DateTime?>("DateUpdated");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<DateTime?>("ExpirationDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Sequence");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("CustomerCouponId");

                    b.ToTable("CustomerCoupons");
                });

            modelBuilder.Entity("TMWork.Data.Models.Invoice.FileUpload", b =>
                {
                    b.Property<int>("FileUploadId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContentType");

                    b.Property<int>("InvoiceId");

                    b.Property<string>("Name");

                    b.Property<byte[]>("UploadData");

                    b.HasKey("FileUploadId");

                    b.HasIndex("InvoiceId");

                    b.ToTable("FileUploads");
                });

            modelBuilder.Entity("TMWork.Data.Models.Invoice.Invoice", b =>
                {
                    b.Property<int>("InvoiceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<float?>("ChargedAmount")
                        .IsRequired();

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<string>("CreatedBy");

                    b.Property<string>("CustomerBlackListed");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateUpdated");

                    b.Property<float?>("DiscountApplied");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<DateTime>("InvoiceDate");

                    b.Property<string>("InvoiceName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("PartNotes");

                    b.Property<float?>("PartsPaid");

                    b.Property<string>("PaymentType");

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.Property<string>("PostalCode");

                    b.Property<string>("State")
                        .IsRequired();

                    b.Property<float?>("TaxAmount");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("WarrantyFromDate");

                    b.Property<DateTime>("WarrantyToDate");

                    b.Property<string>("WorkNotes")
                        .IsRequired();

                    b.HasKey("InvoiceId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("TMWork.Data.Models.User.AuthRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("TMWork.Data.Models.User.AuthUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("TMWork.Data.Models.User.AuthRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TMWork.Data.Models.User.AuthUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TMWork.Data.Models.User.AuthUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("TMWork.Data.Models.User.AuthRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TMWork.Data.Models.User.AuthUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TMWork.Data.Models.User.AuthUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TMWork.Data.Models.Customer.CustomerApplianceBrand", b =>
                {
                    b.HasOne("TMWork.Data.Models.Customer.CustomerApplianceType", "customerApplianceType")
                        .WithMany("CustomerApplianceBrands")
                        .HasForeignKey("CustomerApplianceTypeId")
                        .HasConstraintName("FK_CustomerApplianceBrand_Customer")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TMWork.Data.Models.Customer.CustomerApplianceProblem", b =>
                {
                    b.HasOne("TMWork.Data.Models.Customer.CustomerApplianceBrand", "CustomerApplianceBrand")
                        .WithMany()
                        .HasForeignKey("CustomerApplianceBrandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TMWork.Data.Models.Customer.CustomerApplianceType", "CustomerApplianceType")
                        .WithMany()
                        .HasForeignKey("CustomerApplianceTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TMWork.Data.Models.Customer.Customer", "Customer")
                        .WithMany("CustomerApplianceProblems")
                        .HasForeignKey("CustomerId")
                        .HasConstraintName("FK_CustomerApplianceProblem_Customer")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TMWork.Data.Models.Invoice.FileUpload", b =>
                {
                    b.HasOne("TMWork.Data.Models.Invoice.Invoice", "Invoice")
                        .WithMany("FileUpload")
                        .HasForeignKey("InvoiceId")
                        .HasConstraintName("FK_FileUpload_Invoice")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
