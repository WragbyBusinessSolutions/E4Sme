using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using E4S.Models;
using E4S.Models.HumanResource;

namespace E4S.Data
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      // Customize the ASP.NET Identity model and override the defaults if needed.
      // For example, you can rename the ASP.NET Identity table names and more.
      // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<Organisation> Organisations { get; set; }
    public DbSet<JobTitle> JobTitles { get; set; }
    public DbSet<EmploymentStatus> EmploymentStatuses { get; set; }
    public DbSet<JobCategory> JobCategories { get; set; }
    public DbSet<PayGrade> PayGrades { get; set; }
    public DbSet<EmployeeDetail> EmployeeDetails { get; set; }

    public DbSet<ContactDetail> ContactDetails { get; set; }
    public DbSet<Dependant> Dependants { get; set; }
    public DbSet<EmergencyContact> EmergencyContacts { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Salary> Salaries { get; set; }


    public DbSet<Branch> Branches { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Leave> Leaves { get; set; }

    public DbSet<Vacancy> Vacancies { get; set; }
    public DbSet<LeaveConfiguration> LeaveConfigurations { get; set; }
    public DbSet<OrganisationAsset> OrganisationAssets { get; set; }
    public DbSet<InstitutionQualification> InstitutionQualifications { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<WorkExperience> WorkExperiences { get; set; }
  }
}
