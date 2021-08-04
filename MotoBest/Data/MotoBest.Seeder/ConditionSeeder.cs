namespace MotoBest.Seeder
{
    using MotoBest.Data;
    using MotoBest.Models;
    using System.Threading.Tasks;

    public class ConditionSeeder : ISeeder
    {
        public async Task Seed(ApplicationDbContext dbContext)
        {
            var conditions = new Condition[]
            {
                new Condition { Type = "Нов" },
                new Condition { Type = "Употребяван" },
                new Condition { Type = "Повреден/Ударен" },
                new Condition { Type = "За части" },
            };

            await dbContext.Conditions.AddRangeAsync(conditions);
            await dbContext.SaveChangesAsync();
        }
    }
}
