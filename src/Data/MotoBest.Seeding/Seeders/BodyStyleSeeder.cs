namespace MotoBest.Seeding.Seeders
{
    using System.Linq;
    using System.Threading.Tasks;

    using MotoBest.Data;
    using MotoBest.Data.Models;
    using MotoBest.Seeding.Entities;

    public class BodyStyleSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            foreach (string bodyStyle in BodyStyles.All())
            {
                if (dbContext.BodyStyles.FirstOrDefault(bs => bs.Name == bodyStyle) == null)
                {
                    await dbContext.BodyStyles.AddAsync(new BodyStyle { Name = bodyStyle });
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
