using hiload.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using hiload.Services;

namespace hiload
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<HiloadContext>();
            services.AddMvc(options => options.MaxModelValidationErrors = 70);
            services.AddTransient<InitService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, InitService initService)
        {
            app.UseResponseBuffering();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            initService.LoadData(env.IsDevelopment() ? @"d:\tmp\data" : "/tmp/data");
        }
    }
}
