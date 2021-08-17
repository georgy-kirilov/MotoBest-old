namespace MotoBest.Seeding.Seeders
{
    using System.Threading.Tasks;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Seeding.Entities;

    public class ColorSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            foreach (string color in Colors.All())
            {
                await dbContext.Colors.AddAsync(new Color { Name = color });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
