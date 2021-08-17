namespace MotoBest.Seeding.Seeders
{
    using System.Threading.Tasks;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Seeding.Entities;

    public class TransmissionSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            foreach (string transmission in Transmissions.All())
            {
                await dbContext.Transmissions.AddAsync(new Transmission { Type = transmission });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
