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
                new Color { Name = "Бежов" },
                new Color { Name = "Бордо" },
                new Color { Name = "Бронзов" },
                new Color { Name = "Бял" },
                new Color { Name = "Виолетов" },
                new Color { Name = "Жълт" },
                new Color { Name = "Зелен" },
                new Color { Name = "Златен" },
                new Color { Name = "Кафяв" },
                new Color { Name = "Оранжев" },
                new Color { Name = "Сив" },
                new Color { Name = "Син" },
                new Color { Name = "Сребърен" },
                new Color { Name = "Червен" },
                new Color { Name = "Черен" },
                new Color { Name = "Лилав" },
                new Color { Name = "Охра" },
                new Color { Name = "Перла" },
                new Color { Name = "Розов" },
                new Color { Name = "" },
                new Color { Name = "" },
                new Color { Name = "" },
                new Color { Name = "" },
            };
        }
    }
}
