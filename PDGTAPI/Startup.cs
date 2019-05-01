using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PDGTAPI.Helpers;
using PDGTAPI.Infrastructure;
using PDGTAPI.Services;
using RestSharp;

namespace PDGTAPI
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
			services.AddSingleton(Configuration);

			services.AddTransient<IRedCapService, RedCapService>();
			services.AddTransient<IRestClient, RestClient>();
			services.AddTransient<IUsersService, UsersService>();

			services.AddCors(options =>
			{
				options.AddPolicy("DefaultPolicy", policy =>
				{
					policy.AllowAnyHeader();
					policy.AllowAnyMethod();
					policy.AllowAnyOrigin();
				});
			});
			services.AddDbContext<ApplicationDataContext>(options =>
			{
				// USE SQL SERVER FOR PRODUCTION
				options.UseInMemoryDatabase("PDGTAPI");
				//options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString"));
			});
				
			services.AddIdentity<User, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDataContext>();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = false;
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Security:JWT:Key"])),
					ValidateAudience = true,
					ValidAudience = Configuration["Security:JWT:Audience"],
					ValidateIssuer = true,
					ValidIssuer = Configuration["Security:JWT:Issuer"]
				};
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy("Doctors", policy =>
				{
					policy.RequireRole(Roles.Doctor, Roles.Administrator);
				});
				options.AddPolicy("Patients", policy =>
				{
					policy.RequireRole(Roles.Patient);
				});
				options.AddPolicy("Administrators", policy =>
				{
					policy.RequireRole(Roles.Administrator);
				});
			});

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDataContext DataContext, IServiceProvider serviceProvider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}
			app.UseCors("DefaultPolicy");
			DataContext.Database.EnsureCreated();
			app.UseAuthentication();
			new DataSeeder(serviceProvider.GetRequiredService<RoleManager<IdentityRole>>()).Seed();
			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
