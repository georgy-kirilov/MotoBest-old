namespace MotoBest.Seeder
{
    using MotoBest.Data;
    using MotoBest.Models;
    using System.Threading.Tasks;

    public class BodyStyleSeeder : ISeeder
    {
        public const string Van = "ван";
        public const string Sedan = "седан";
        public const string Hatchback = "хечбек";
        public const string StationWagon = "комби";
        public const string Coupe = "купе";
        public const string Convertible = "кабрио";
        public const string Jeep = "джип";
        public const string Pickup = "пикап";
        public const string Minivan = "миниван";
        public const string StretchLimousine = "стреч лимузина";

        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            var bodyStyles = new BodyStyle[]
            {
                new BodyStyle { Name = Van },
                new BodyStyle { Name = Sedan },
                new BodyStyle { Name = Hatchback },
                new BodyStyle { Name = StationWagon },
                new BodyStyle { Name = Coupe },
                new BodyStyle { Name = Convertible },
                new BodyStyle { Name = Jeep },
                new BodyStyle { Name = Pickup },
                new BodyStyle { Name = Minivan },
                new BodyStyle { Name = StretchLimousine },
            };

            await dbContext.AddRangeAsync(bodyStyles);
            await dbContext.SaveChangesAsync();
        }
    }
}
