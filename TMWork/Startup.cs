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
using TMWork.Data;
using TMWork.Models;
using TMWork.Services;
using TMWork.Data.Models.User;
using Newtonsoft.Json.Serialization;
using TMWork.Data.Repos;
using AutoMapper;
using TMWork.ViewModels.Home;
using TMWork.Data.Models.Customer;
using TMWork.ViewModels.Invoice;
using TMWork.Data.Models.Invoice;

namespace TMWork
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        private IHostingEnvironment _env;
        private IConfigurationRoot _config;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;

            var builder = new ConfigurationBuilder()
                                .SetBasePath(env.ContentRootPath)
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
                                //.AddJsonFile("config.json");

            _config = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TMDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<AuthUser, AuthRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = true;
            })
              .AddEntityFrameworkStores<TMDbContext>()
              .AddDefaultTokenProviders();

            services.AddMvc()
                .AddJsonOptions(config =>
                {
                    config.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();  /*new DefaultContractResolver();*/  //
                    config.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

                });
            services.AddKendo();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            // Add application services.
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<ISmsService, MailService>();
            services.AddTransient<GlobalService, GlobalService>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerApplianceTypeRepository, CustomerApplianceTypeRepository>();
            services.AddScoped<ICustomerApplianceBrandRepository, CustomerApplianceBrandRepository>();
            services.AddScoped<ICustomerCouponRepository, CustomerCouponRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<SelectedTabFilterAttribute>();
            services.AddSingleton(_config);

            services.AddTransient<TMCoreSeedData>();

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, TMCoreSeedData seedData)
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

            Mapper.Initialize(config =>
            {
                config.CreateMap<ContactViewModel, Contact>().ReverseMap();
                config.CreateMap<InvoiceViewModel, Invoice>().ReverseMap();
            });

            app.UseStaticFiles();

            app.UseAuthentication();

            //app.UseStatusCodePagesWithRedirects("~/errors/{0}");
            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller=Admin}/{action=Index}/{id?}");
                //routes.MapRoute(
                //    name: "defaultApi",
                //    template: "api/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseFileServer();

            //seeding Data on startup!
            seedData.EnsureSeedData().Wait();
        }
    }
}
