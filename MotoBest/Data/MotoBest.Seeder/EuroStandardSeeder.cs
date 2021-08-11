namespace MotoBest.Seeder
{
    using MotoBest.Data;
    using MotoBest.Models;
    using System.Threading.Tasks;

    public class EuroStandardSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            var euroStandards = new EuroStandard[]
            {
                new EuroStandard { Type = "Евро 1" },
                new EuroStandard { Type = "Евро 2" },
                new EuroStandard { Type = "Евро 3" },
                new EuroStandard { Type = "Евро 4" },
                new EuroStandard { Type = "Евро 5" },
                new EuroStandard { Type = "Евро 6" },
            };

            await dbContext.AddRangeAsync(euroStandards);
            await dbContext.SaveChangesAsync();
        }
    }
}
