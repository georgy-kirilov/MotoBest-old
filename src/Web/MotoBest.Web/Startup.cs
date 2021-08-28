namespace MotoBest.Web
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using MotoBest.Data;
    using MotoBest.Data.Models;

    using MotoBest.Services;
    using MotoBest.Services.Contracts;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IAdvertsService, AdvertsService>();
            services.AddTransient<IAdvertsFormatter, AdvertsFormatter>();
            services.AddTransient<IBrandsService, BrandsService>();
            services.AddTransient<IModelsService, ModelsService>();
            services.AddTransient<IColorsService, ColorsService>();
            services.AddTransient<IEnginesService, EnginesService>();
            services.AddTransient<ITransmissionsService, TransmissionsService>();
            services.AddTransient<IBodyStylesService, BodyStylesService>();
            services.AddTransient<IEuroStandardsService, EuroStandardsService>();
            services.AddTransient<IConditionsService, ConditionsService>();
            services.AddTransient<IRegionsService, RegionsService>();
            services.AddTransient<ITownsService, TownsService>();
            services.AddTransient<IImagesService, ImagesService>();
            services.AddTransient<IAdvertProvidersService, AdvertProvidersService>();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
