namespace MotoBest.Seeder
{
    using MotoBest.Data;
    using MotoBest.Models;
    using System.Threading.Tasks;

    public class BodyStyleSeeder : ISeeder
    {
        public async Task Seed(ApplicationDbContext dbContext)
        {
            var bodyStyles = new BodyStyle[]
            {
                new BodyStyle { Name = "Ван" },
                new BodyStyle { Name = "Седан" },
                new BodyStyle { Name = "Хечбек" },
                new BodyStyle { Name = "Комби" },
                new BodyStyle { Name = "Kупе" },
                new BodyStyle { Name = "Кабрио" },
                new BodyStyle { Name = "Джип" },
                new BodyStyle { Name = "Пикап" },
                new BodyStyle { Name = "Миниван" },
                new BodyStyle { Name = "Стреч лимузина" },
            };

            await dbContext.AddRangeAsync(bodyStyles);
            await dbContext.SaveChangesAsync();
        }
    }
}
