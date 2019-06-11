using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using E4S.Data;
using E4S.Models;
using E4S.Services;

namespace E4S
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

      services.AddIdentity<ApplicationUser, IdentityRole>()
          .AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders();

      // Add application services.
      services.AddTransient<IEmailSender, EmailSender>();

      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
    {
      if (env.IsDevelopment())
      {
        app.UseBrowserLink();
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

      app.UseStaticFiles();

      app.UseAuthentication();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
      });

      try
      {
        CreateRoles(serviceProvider).Wait();
      }
      catch
      {

      }

    }

    private async Task CreateRoles(IServiceProvider serviceProvider)
    {
      //initializing custom roles   
      var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
      // var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
      string[] roleNames = { "Super Admin", "Admin", "Human Resources", "Accountant", "Customer Service", "Employee" };
      IdentityResult roleResult;

      foreach (var roleName in roleNames)
      {
        var roleExist = await RoleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
          //create the roles and seed them to the database: Question 1  
          roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
        }
      }


    }

  }
}
