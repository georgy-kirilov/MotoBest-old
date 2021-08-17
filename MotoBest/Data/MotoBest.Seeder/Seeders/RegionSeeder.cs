namespace MotoBest.Seeding.Seeders
{
    using System.Threading.Tasks;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Seeding.Entities;

    public class RegionSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            foreach (string region in Regions.All())
            {
                await dbContext.Regions.AddAsync(new Region { Name = region });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
