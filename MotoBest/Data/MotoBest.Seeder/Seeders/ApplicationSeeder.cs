namespace MotoBest.Seeding.Seeders
{
    using System.Threading.Tasks;

    using MotoBest.Data;

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
