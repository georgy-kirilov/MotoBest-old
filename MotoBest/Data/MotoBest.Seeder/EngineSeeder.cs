namespace MotoBest.Seeder
{
    using MotoBest.Data;
    using MotoBest.Models;
    using System.Threading.Tasks;

    public class EngineSeeder : ISeeder
    {
        public async Task Seed(ApplicationDbContext dbContext)
        {
            var engines = new Engine[]
            {
                new Engine { Type = "Бензин" },
                new Engine { Type = "Дизел" },
                new Engine { Type = "Хибриден" },
                new Engine { Type = "Електрически" },
            };

            await dbContext.Engines.AddRangeAsync(engines);
            await dbContext.SaveChangesAsync();
        }
    }
}
