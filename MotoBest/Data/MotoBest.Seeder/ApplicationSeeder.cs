namespace MotoBest.Seeder
{
    using MotoBest.Data;
    using System.Threading.Tasks;

    public class ApplicationSeeder<T> : ISeeder where T : ISeeder
    {
        private readonly T[] seeders;

        public ApplicationSeeder(params T[] seeders)
        {
            this.seeders = seeders;
        }

        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            foreach (ISeeder seeder in seeders)
            {
                await seeder.SeedAsync(dbContext);
            }
        }
    }
}
