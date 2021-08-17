namespace MotoBest.Seeding.Seeders
{
    using System.Threading.Tasks;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Seeding.Entities;

    public class EuroStandardSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            foreach (string euroStandard in EuroStandards.All())
            {
                await dbContext.EuroStandards.AddAsync(new EuroStandard { Type = euroStandard });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
