namespace MotoBest.Seeder
{
    using MotoBest.Data;
    using MotoBest.Models;
    using System.Threading.Tasks;

    public class ConditionSeeder : ISeeder
    {
        public const string New = "нов";
        public const string Used = "употребяван";
        public const string DamagedOrHit = "повреден/ударен";
        public const string ForParts = "за части";

        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            var conditions = new Condition[]
            {
                new Condition { Type = New },
                new Condition { Type = Used },
                new Condition { Type = DamagedOrHit },
                new Condition { Type = ForParts },
            };

            await dbContext.Conditions.AddRangeAsync(conditions);
            await dbContext.SaveChangesAsync();
        }
    }
}
