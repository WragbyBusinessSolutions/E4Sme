﻿// <auto-generated />
using E4S.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace E4S.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190620112124_jobs")]
    partial class jobs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("E4S.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("EmployeeName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<Guid>("OrganisationId");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Status");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<string>("UserRole");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("E4S.Models.Branch", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BranchName");

                    b.Property<string>("Comment");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid>("OrganisationId");

                    b.HasKey("Id");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("E4S.Models.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("DepartmentName");

                    b.Property<string>("Description");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid>("OrganisationId");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("E4S.Models.HumanResource.ContactDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressOne");

                    b.Property<string>("AddressTwo");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<Guid>("EmployeeDetailId");

                    b.Property<string>("HomeTelephone");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Mobile");

                    b.Property<Guid>("OrganisationId");

                    b.Property<string>("OtherEmail");

                    b.Property<string>("State");

                    b.Property<string>("WorkEmail");

                    b.Property<string>("WorkTelephone");

                    b.Property<string>("Zip");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeDetailId");

                    b.ToTable("ContactDetails");
                });

            modelBuilder.Entity("E4S.Models.HumanResource.Dependant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<Guid>("EmployeeDetailId");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<Guid>("OrganisationId");

                    b.Property<string>("Relationship");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeDetailId");

                    b.ToTable("Dependants");
                });

            modelBuilder.Entity("E4S.Models.HumanResource.EmergencyContact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<Guid>("EmployeeDetailId");

                    b.Property<string>("HomeTelephone");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Mobile");

                    b.Property<string>("Name");

                    b.Property<Guid>("OrganisationId");

                    b.Property<string>("Relationship");

                    b.Property<string>("WorkTelephone");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeDetailId");

                    b.ToTable("EmergencyContacts");
                });

            modelBuilder.Entity("E4S.Models.HumanResource.EmployeeDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Email");

                    b.Property<string>("EmployeeId");

                    b.Property<string>("EmployeeStatus");

                    b.Property<string>("FirstName");

                    b.Property<string>("Gender");

                    b.Property<string>("ImageUrl");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName");

                    b.Property<string>("MaritalStatus");

                    b.Property<string>("MiddleName");

                    b.Property<string>("Nationality");

                    b.Property<Guid>("OrganisationId");

                    b.Property<string>("OtherId");

                    b.Property<string>("PhoneNumber");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.ToTable("EmployeeDetails");
                });

            modelBuilder.Entity("E4S.Models.HumanResource.EmploymentStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Description");

                    b.Property<string>("EmploymentStatusName");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid>("OrganisationId");

                    b.HasKey("Id");

                    b.ToTable("EmploymentStatuses");
                });

            modelBuilder.Entity("E4S.Models.HumanResource.Job", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("BranchId");

                    b.Property<string>("ContractDetail");

                    b.Property<Guid>("DepartmentId");

                    b.Property<Guid>("EmployeeDetailId");

                    b.Property<Guid>("EmploymentStatusId");

                    b.Property<DateTime>("EndDate");

                    b.Property<bool>("IsActive");

                    b.Property<Guid>("JobCategoryId");

                    b.Property<string>("JobSpecification");

                    b.Property<Guid>("JobTitleId");

                    b.Property<DateTime>("JoinedDate");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("EmployeeDetailId");

                    b.HasIndex("EmploymentStatusId");

                    b.HasIndex("JobCategoryId");

                    b.HasIndex("JobTitleId");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("E4S.Models.HumanResource.JobCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("JobCategoryName");

                    b.Property<Guid>("OrganisationId");

                    b.HasKey("Id");

                    b.ToTable("JobCategories");
                });

            modelBuilder.Entity("E4S.Models.HumanResource.JobTitle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("JobTitleName");

                    b.Property<string>("Note");

                    b.Property<Guid>("OrganisationId");

                    b.HasKey("Id");

                    b.ToTable("JobTitles");
                });

            modelBuilder.Entity("E4S.Models.HumanResource.PayGrade", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Currency");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<bool>("IsDeleted");

                    b.Property<float>("MaximumSalary");

                    b.Property<float>("MinimumSalary");

                    b.Property<Guid>("OrganisationId");

                    b.Property<string>("PayGradeName");

                    b.HasKey("Id");

                    b.ToTable("PayGrades");
                });

            modelBuilder.Entity("E4S.Models.HumanResource.Salary", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("Amount");

                    b.Property<string>("Comment");

                    b.Property<string>("Currency");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<Guid>("EmployeeDetailId");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid>("OrganisationId");

                    b.Property<string>("PayFrequency");

                    b.Property<Guid>("PayGradeId");

                    b.Property<string>("SalaryComponent");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeDetailId");

                    b.HasIndex("PayGradeId");

                    b.ToTable("Salaries");
                });

            modelBuilder.Entity("E4S.Models.Organisation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Email");

                    b.Property<string>("ImageUrl");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("NoOfEmployees");

                    b.Property<Guid>("OrganisationId");

                    b.Property<string>("OrganisationName");

                    b.Property<string>("OrganisationPrefix");

                    b.Property<string>("PhoneNumber");

                    b.Property<Guid>("RegistrarId");

                    b.Property<string>("RegistrationNo");

                    b.Property<string>("State");

                    b.Property<string>("TaxId");

                    b.Property<int>("ZipCode");

                    b.HasKey("Id");

                    b.ToTable("Organisations");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

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

            modelBuilder.Entity("E4S.Models.HumanResource.ContactDetail", b =>
                {
                    b.HasOne("E4S.Models.HumanResource.EmployeeDetail", "EmployeeDetail")
                        .WithMany()
                        .HasForeignKey("EmployeeDetailId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("E4S.Models.HumanResource.Dependant", b =>
                {
                    b.HasOne("E4S.Models.HumanResource.EmployeeDetail", "EmployeeDetail")
                        .WithMany()
                        .HasForeignKey("EmployeeDetailId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("E4S.Models.HumanResource.EmergencyContact", b =>
                {
                    b.HasOne("E4S.Models.HumanResource.EmployeeDetail", "EmployeeDetail")
                        .WithMany()
                        .HasForeignKey("EmployeeDetailId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("E4S.Models.HumanResource.Job", b =>
                {
                    b.HasOne("E4S.Models.Branch", "Branch")
                        .WithMany()
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("E4S.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("E4S.Models.HumanResource.EmployeeDetail", "EmployeeDetail")
                        .WithMany()
                        .HasForeignKey("EmployeeDetailId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("E4S.Models.HumanResource.EmploymentStatus", "EmploymentStatus")
                        .WithMany()
                        .HasForeignKey("EmploymentStatusId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("E4S.Models.HumanResource.JobCategory", "JobCategory")
                        .WithMany()
                        .HasForeignKey("JobCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("E4S.Models.HumanResource.JobTitle", "JobTitle")
                        .WithMany()
                        .HasForeignKey("JobTitleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("E4S.Models.HumanResource.Salary", b =>
                {
                    b.HasOne("E4S.Models.HumanResource.EmployeeDetail", "EmployeeDetail")
                        .WithMany()
                        .HasForeignKey("EmployeeDetailId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("E4S.Models.HumanResource.PayGrade", "PayGrade")
                        .WithMany()
                        .HasForeignKey("PayGradeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("E4S.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("E4S.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("E4S.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("E4S.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
