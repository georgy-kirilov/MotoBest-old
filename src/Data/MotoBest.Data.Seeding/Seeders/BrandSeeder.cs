namespace MotoBest.Data.Seeding.Seeders
{
    using System.Linq;
    using System.Threading.Tasks;

    using MotoBest.Data;
    using MotoBest.Data.Models;
    using MotoBest.Data.Seeding.Entities;

    public class BrandSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            foreach (string brand in Brands.All())
            {
                if (dbContext.Brands.FirstOrDefault(b => b.Name == brand) == null)
                {
                    await dbContext.Brands.AddAsync(new Brand { Name = brand });
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
