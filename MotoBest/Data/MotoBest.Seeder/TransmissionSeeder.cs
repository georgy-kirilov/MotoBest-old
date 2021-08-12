namespace MotoBest.Seeder
{
    using MotoBest.Data;
    using MotoBest.Models;
    using System.Threading.Tasks;

    public class TransmissionSeeder : ISeeder
    {
        public const string Manual = "ръчна";
        public const string Automatic = "автоматична";
        public const string SemiAutomatic = "полуавтоматична";

        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            var transmissions = new Transmission[]
            {
                new Transmission { Type = Manual },
                new Transmission { Type = Automatic },
                new Transmission { Type = SemiAutomatic },
            };

            await dbContext.Transmissions.AddRangeAsync(transmissions);
            await dbContext.SaveChangesAsync();
        }
    }
}
