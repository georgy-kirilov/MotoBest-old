namespace MotoBest.Seeding.Seeders
{
    using System.Threading.Tasks;

    using MotoBest.Data;

    public class ApplicationSeeder : ISeeder
    {
        private readonly ISeeder[] seeders;

        public ApplicationSeeder() 
            : this(new BodyStyleSeeder(), 
                   new ColorSeeder(), 
                   new ConditionSeeder(), 
                   new EngineSeeder(), 
                   new EuroStandardSeeder(), 
                   new RegionSeeder(), 
                   new TransmissionSeeder())
        {
        }

        public ApplicationSeeder(params ISeeder[] seeders)
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
