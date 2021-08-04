namespace MotoBest.Seeder
{
    using MotoBest.Data;
    using MotoBest.Models;
    using System.Threading.Tasks;

    public class TransmissionSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            var transmissions = new Transmission[]
            {
                new Transmission { Type = "Ръчна" },
                new Transmission { Type = "Автоматична" },
                new Transmission { Type = "Полуавтоматична" },
            };

            await dbContext.Transmissions.AddRangeAsync(transmissions);
            await dbContext.SaveChangesAsync();
        }
    }
}
