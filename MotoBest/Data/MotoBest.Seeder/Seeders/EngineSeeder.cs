namespace MotoBest.Seeding.Seeders
{
    using System.Threading.Tasks;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Seeding.Entities;

    public class EngineSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            foreach (string engine in Engines.All())
            {
                await dbContext.Engines.AddAsync(new Engine { Type = engine });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
