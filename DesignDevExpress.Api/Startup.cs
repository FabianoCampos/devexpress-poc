using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesignDevExpress.Api.Filters;
using DesignDevExpress.Api.StorageWeb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using DevExpress.AspNetCore;
using DevExpress.XtraReports.Web.Extensions;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;

namespace DesignDevExpress.Api
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
            services.AddControllers();
            services.AddDevExpressControls();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DesignDevExpress.Api", Version = "v1" });
            });
            
            services.AddCors(options => {
                options.AddDefaultPolicy(o=> o.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            });

            services.AddScoped<ReportStorageWebExtension, CustomReportStorageWebExtension>();
            services.AddSingleton<IDashboardStorage, DashboardStorage>();
            //services.AddMvcCore();
            DashboardConfigurator.PassCredentials = true;
            services
                .AddMvc(options => { options.Filters.Add(typeof(CustomExceptionFilter));})
                .AddDefaultDashboardController((configurator, serviceProvider) =>
                {
                    configurator.SetDashboardStorage(serviceProvider.GetRequiredService<IDashboardStorage>());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDevExpressControls();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DesignDevExpress.Api v1"));
            }
            
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                EndpointRouteBuilderExtension.MapDashboardRoute(endpoints, "api/dashboard");
                endpoints.MapControllers();
            });
        }
    }
}