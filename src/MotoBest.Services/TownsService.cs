namespace MotoBest.Services
{
    using System.Linq;
    using System.Collections.Generic;

    using MotoBest.Data;

    public class TownsService : ITownsService
    {
        private readonly ApplicationDbContext dbContext;

        public TownsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<string> GetAllTownNamesByRegionName(string regionName)
        {
            return dbContext.Towns.Where(town => town.Region.Name == regionName).Select(town => town.Name).OrderBy(town => town).ToList();
        }
    }
}
