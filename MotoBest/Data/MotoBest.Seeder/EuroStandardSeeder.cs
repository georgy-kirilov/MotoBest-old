namespace MotoBest.Seeder
{
    using MotoBest.Data;
    using MotoBest.Models;
    using System.Threading.Tasks;

    public class EuroStandardSeeder : ISeeder
    {
        public const string EuroOne = "евро 1";
        public const string EuroTwo = "евро 2";
        public const string EuroThree = "евро 3";
        public const string EuroFour = "евро 4";
        public const string EuroFive = "евро 5";
        public const string EuroSix = "евро 6";

        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            var euroStandards = new EuroStandard[]
            {
                new EuroStandard { Type =  EuroOne },
                new EuroStandard { Type =  EuroTwo },
                new EuroStandard { Type =  EuroThree },
                new EuroStandard { Type =  EuroFour },
                new EuroStandard { Type =  EuroFive },
                new EuroStandard { Type =  EuroSix },
            };

            await dbContext.AddRangeAsync(euroStandards);
            await dbContext.SaveChangesAsync();
        }
    }
}
