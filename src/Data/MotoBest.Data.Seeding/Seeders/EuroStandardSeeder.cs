namespace MotoBest.Data.Seeding.Seeders
{
    using System.Linq;
    using System.Threading.Tasks;

    using MotoBest.Data;
    using MotoBest.Data.Models;
    using MotoBest.Data.Seeding.Entities;

    public class EuroStandardSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            foreach (string euroStandard in EuroStandards.All())
            {
                if (dbContext.EuroStandards.FirstOrDefault(es => es.Type == euroStandard) == null)
                {
                    await dbContext.EuroStandards.AddAsync(new EuroStandard { Type = euroStandard });
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
