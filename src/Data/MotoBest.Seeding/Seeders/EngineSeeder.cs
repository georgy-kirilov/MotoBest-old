namespace MotoBest.Seeding.Seeders
{
    using System.Linq;
    using System.Threading.Tasks;

    using MotoBest.Data;
    using MotoBest.Data.Models;
    using MotoBest.Seeding.Entities;

    public class EngineSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            foreach (string engine in Engines.All())
            {
                if (dbContext.Engines.FirstOrDefault(e => e.Type == engine) == null)
                {
                    await dbContext.Engines.AddAsync(new Engine { Type = engine });
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
