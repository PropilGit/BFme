using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BFme.Infrastructure;
using BFme.Models;
using BFme.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BFme
{
    public class Startup
    {
        Access access;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Access
            access = JSONConverter.OpenJSONFile<Access>(Environment.CurrentDirectory + "\\_access\\access.json");

            // ад
            string dbConnection = "Server=" + access.Server + "; Database=" + access.Login + ";user=" + access.Login + ";password=" + access.Password + ";";
            services.AddDbContext<InvestContext>(options => options.UseMySql(dbConnection));

            // FTP
            services.AddSingleton<IFileController, FtpController>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IFileController ftp)
        {
            ftp.Configure(access);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
