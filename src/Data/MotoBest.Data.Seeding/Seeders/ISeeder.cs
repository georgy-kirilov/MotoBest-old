namespace MotoBest.Data.Seeding.Seeders
{
    using System.Threading.Tasks;

    using MotoBest.Data;

    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext dbContext);
    }
}
