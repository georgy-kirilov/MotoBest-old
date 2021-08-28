namespace MotoBest.Seeding.Seeders
{
    using System.Linq;
    using System.Threading.Tasks;

    using MotoBest.Data;
    using MotoBest.Data.Models;
    using MotoBest.Seeding.Entities;

    public class TransmissionSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            foreach (string transmission in Transmissions.All())
            {
                if (dbContext.Transmissions.FirstOrDefault(t => t.Type == transmission) == null)
                {
                    await dbContext.Transmissions.AddAsync(new Transmission { Type = transmission });
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
