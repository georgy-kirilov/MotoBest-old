namespace MotoBest.Seeder
{
    using MotoBest.Data;
    using MotoBest.Models;
    using System.Threading.Tasks;

    public class ColorSeeder : ISeeder
    {
        public async Task Seed(ApplicationDbContext dbContext)
        {
            var colors = new Color[]
            {
                new Color { Name = "Банан" },
                new Color { Name = "Беата" },
                new Color { Name = "Бежов" },
                new Color { Name = "Бордо" },
                new Color { Name = "Бронз" },
                new Color { Name = "Бял" },
                new Color { Name = "Винен" },
                new Color { Name = "Виолетов" },
                new Color { Name = "Вишнев" },
                new Color { Name = "Графит" },
                new Color { Name = "Жълт" },
                new Color { Name = "Зелен" },
                new Color { Name = "Златист" },
                new Color { Name = "Кафяв" },
                new Color { Name = "Керемиден" },
                new Color { Name = "Кремав" },
                new Color { Name = "Лилав" },
                new Color { Name = "Металик" },
                new Color { Name = "Оранжев" },
                new Color { Name = "Охра" },
                new Color { Name = "Пепеляв" },
                new Color { Name = "Перла" },
                new Color { Name = "Пясъчен" },
                new Color { Name = "Резидав" },
                new Color { Name = "Розов" },
                new Color { Name = "Сахара" },
                new Color { Name = "Светло зелен" },
                new Color { Name = "Светло сив" },
                new Color { Name = "Светло син" },
                new Color { Name = "Сив" },
                new Color { Name = "Син" },
                new Color { Name = "Слонова кост" },
                new Color { Name = "Сребърен" },
                new Color { Name = "Тъмно зелен" },
                new Color { Name = "Тъмно сив" },
                new Color { Name = "Тъмно син" },
                new Color { Name = "Тъмно червен" },
                new Color { Name = "Тютюн" },
                new Color { Name = "Хамелеон" },
                new Color { Name = "Червен" },
                new Color { Name = "Черен" },
            };

            await dbContext.Colors.AddRangeAsync(colors);
            await dbContext.SaveChangesAsync();
        }
    }
}
