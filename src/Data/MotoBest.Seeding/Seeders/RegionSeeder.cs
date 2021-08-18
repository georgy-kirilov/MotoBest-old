namespace MotoBest.Seeding.Seeders
{
    using System.Linq;
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
                if (dbContext.Regions.FirstOrDefault(r => r.Name == region) == null)
                {
                    await dbContext.Regions.AddAsync(new Region { Name = region });
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
