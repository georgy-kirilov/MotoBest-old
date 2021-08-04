namespace MotoBest.Seeder
{
    using MotoBest.Data;
    using MotoBest.Models;
    using System.Threading.Tasks;

    public class RegionSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            var regions = new Region[]
            {
                new Region { Name = "Благоевград" },
                new Region { Name = "Бургас" },
                new Region { Name = "Варна" },
                new Region { Name = "Велико Търново" },
                new Region { Name = "Видин" },
                new Region { Name = "Враца" },
                new Region { Name = "Габрово" },
                new Region { Name = "Добрич" },
                new Region { Name = "Дупница" },
                new Region { Name = "Кърджали" },
                new Region { Name = "Кюстендил" },
                new Region { Name = "Ловеч" },
                new Region { Name = "Монтана" },
                new Region { Name = "Пазарджик" },
                new Region { Name = "Перник" },
                new Region { Name = "Плевен" },
                new Region { Name = "Пловдив" },
                new Region { Name = "Разград" },
                new Region { Name = "Русе" },
                new Region { Name = "Силистра" },
                new Region { Name = "Сливен" },
                new Region { Name = "Смолян" },
                new Region { Name = "София" },
                new Region { Name = "Стара Загора" },
                new Region { Name = "Търговище" },
                new Region { Name = "Хасково" },
                new Region { Name = "Шумен" },
                new Region { Name = "Ямбол" },
            };

            await dbContext.Regions.AddRangeAsync(regions);
            await dbContext.SaveChangesAsync();
        }
    }
}
