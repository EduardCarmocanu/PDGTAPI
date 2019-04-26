using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PDGTAPI.Data;
using PDGTAPI.Data.Entities;
using PDGTAPI.Services;

namespace PDGTAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IRedCapService redCapService)
		{
			Configuration = configuration;
			RedCapService = redCapService;
		}

		public IConfiguration Configuration { get; }
		public IRedCapService RedCapService { get; set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(Configuration);
			services.AddSingleton(RedCapService);

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
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString"));
			});
				
			services.AddIdentity<UserEntity, IdentityRole>()
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
					policy.RequireRole("Doctor, Administrator");
				});
				options.AddPolicy("Patients", policy =>
				{
					policy.RequireRole("Patient");
				});
				options.AddPolicy("Administrators", policy =>
				{
					policy.RequireRole("Administrator");
				});
			});

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDataContext DataContext)
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
			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
