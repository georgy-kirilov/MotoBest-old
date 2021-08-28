namespace MotoBest.Data.Seeding.Seeders
{
    using System.Linq;
    using System.Threading.Tasks;

    using MotoBest.Data;
    using MotoBest.Data.Models;
    using MotoBest.Data.Seeding.Entities;

    public class ColorSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            foreach (string color in Colors.All())
            {
                if (dbContext.Colors.FirstOrDefault(c => c.Name == color) == null)
                {
                    await dbContext.Colors.AddAsync(new Color { Name = color });
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
