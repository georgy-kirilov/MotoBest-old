namespace MotoBest.Seeder
{
    using MotoBest.Data;
    using MotoBest.Models;
    using System.Threading.Tasks;

    public class EngineSeeder : ISeeder
    {
        public const string Gasoline = "бензинов";
        public const string Diesel = "дизелов";
        public const string Hybrid = "хибриден";
        public const string Electric = "електрически";

        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            var engines = new Engine[]
            {
                new Engine { Type = Gasoline },
                new Engine { Type = Diesel },
                new Engine { Type = Hybrid },
                new Engine { Type = Electric },
            };

            await dbContext.Engines.AddRangeAsync(engines);
            await dbContext.SaveChangesAsync();
        }
    }
}
