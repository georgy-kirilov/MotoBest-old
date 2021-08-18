namespace MotoBest.Seeding.Seeders
{
    using System.Linq;
    using System.Threading.Tasks;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Seeding.Entities;

    public class ConditionSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            foreach (string condition in Conditions.All())
            {
                if (dbContext.Conditions.FirstOrDefault(c => c.Type == condition) == null)
                {
                    await dbContext.Conditions.AddAsync(new Condition { Type = condition });
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
