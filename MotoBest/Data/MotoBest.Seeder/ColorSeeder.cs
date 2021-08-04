namespace MotoBest.Seeder
{
    using MotoBest.Data;
    using MotoBest.Models;

    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class ColorSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            string path = "./Resources/colors.json";
            string text = await File.ReadAllTextAsync(path);

            var colors = JsonSerializer
                            .Deserialize<List<string>>(text)
                            .Select(name => new Color { Name = name });

            await dbContext.Colors.AddRangeAsync(colors);
            await dbContext.SaveChangesAsync();
        }
    }
}
