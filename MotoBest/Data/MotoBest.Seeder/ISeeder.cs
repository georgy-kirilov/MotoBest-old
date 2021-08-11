namespace MotoBest.Seeder
{
    using MotoBest.Data;
    using System.Threading.Tasks;

    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext dbContext);
    }
}
