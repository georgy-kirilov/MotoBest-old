namespace MotoBest.Seeder
{
    using MotoBest.Data;
    using MotoBest.Models;
    using System.Threading.Tasks;

    public class AdvertProviderSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            var providers = new AdvertProvider[]
            {
                new AdvertProvider { Name = "mobile.bg", AdvertUrlFormat = "https://www.mobile.bg/pcgi/mobile.cgi?act=4&adv={0}" },
                new AdvertProvider { Name = "cars.bg", AdvertUrlFormat = "https://www.cars.bg/offer/{0}"},
                new AdvertProvider { Name = "carmarket.bg", AdvertUrlFormat = "https://www.carmarket.bg/{0}"},
            };

            await dbContext.AdvertProviders.AddRangeAsync(providers);
            await dbContext.SaveChangesAsync();
        }
    }
}
