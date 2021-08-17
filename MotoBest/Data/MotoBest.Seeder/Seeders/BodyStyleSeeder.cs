namespace MotoBest.Seeding.Seeders
{
    using System.Threading.Tasks;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Seeding.Entities;

    public class BodyStyleSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            foreach (string bodyStyle in BodyStyles.All())
            {
                await dbContext.BodyStyles.AddAsync(new BodyStyle { Name = bodyStyle });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
