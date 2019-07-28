﻿namespace JudgeSystem.Web
{
    using System;
    using System.Reflection;

    using JudgeSystem.Common;
    using JudgeSystem.Data;
    using JudgeSystem.Data.Common;
    using JudgeSystem.Data.Common.Repositories;
    using JudgeSystem.Data.Models;
    using JudgeSystem.Data.Repositories;
    using JudgeSystem.Data.Seeding;
    using JudgeSystem.Services;
    using JudgeSystem.Services.Data;
    using JudgeSystem.Services.Mapping;
    using JudgeSystem.Services.Messaging;
    using JudgeSystem.Web.Dtos.Course;
    using JudgeSystem.Web.Filters;
    using JudgeSystem.Web.InputModels.Course;
    using JudgeSystem.Web.Utilites;
    using JudgeSystem.Web.ViewModels;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services
                .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;

					options.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddDefaultTokenProviders()
                .AddDefaultUI(UIFramework.Bootstrap4);

			services.AddDistributedMemoryCache();
			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromHours(GlobalConstants.SessionIdleTimeout);
				options.Cookie.HttpOnly = true;
			});

            services
                .AddMvc(options => 
				{
					options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    options.Filters.Add<EntityNotFoundExceptionFilter>();
				})
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                });

            services
                .ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/Identity/Account/Login";
                    options.LogoutPath = "/Identity/Account/Logout";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                });

            services
                .Configure<CookiePolicyOptions>(options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.Lax;
                    options.ConsentCookie.Name = ".AspNetCore.ConsentCookie";
                });

            //Send grid configuration
            var sendGridSection = this.configuration.GetSection("SendGrid");
            services.Configure<SendGridOptions>(sendGridSection);
            var emailSection = this.configuration.GetSection("Email");
            services.Configure<BaseEmailOptions>(emailSection);
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddSingleton(this.configuration);

            // Identity stores
            services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICourseService, CourseService>();
			services.AddTransient<ILessonService, LessonService>();
            services.AddTransient<IResourceService, ResourceService>();
            services.AddTransient<IProblemService, ProblemService>();
            services.AddTransient<ISubmissionService, SubmissionService>();
			services.AddTransient<ITestService, TestService>();
			services.AddTransient<IContestService, ContestService>();
			services.AddTransient<IExecutedTestService, ExecutedTestService>();
			services.AddTransient<IStudentService, StudentService>();
			services.AddTransient<ISchoolClassService, SchoolClassService>();
			services.AddTransient<IPracticeService, PracticeService>();
            services.AddTransient<IFileManager, FileManager>();
			services.AddTransient<IEstimator, Estimator>();
			services.AddTransient<IPasswordHashService, PasswordHashService>();
			services.AddTransient<IPaginationService, PaginationService>();
			services.AddTransient<ContestReslutsHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly, 
                typeof(CourseInputModel).GetTypeInfo().Assembly, typeof(ContestCourseDto).GetTypeInfo().Assembly);

			// Seed data on application startup
			using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
			app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
