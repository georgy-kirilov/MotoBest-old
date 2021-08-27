namespace MotoBest.Services
{
    using System.Linq;
    using System.Collections.Generic;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Services.DTOs;
    using MotoBest.Services.Contracts;

    public class TownsService : ITownsService
    {
        private readonly ApplicationDbContext dbContext;

        public TownsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<TownDto> GetAllTownsByRegionId(int regionId)
        {
            return dbContext.Towns
                            .Where(town => town.RegionId == regionId)
                            .OrderBy(town => town.Name)
                            .Select(town => new TownDto { Id = town.Id, Name = town.Name })
                            .ToList();
        }

        public Town GetOrCreate(Region region, string townName)
        {
            if (townName == null)
            {
                return null;
            }

            return dbContext.Towns.FirstOrDefault(town => town.Name == townName) ?? new Town { Name = townName, Region = region };
        }
    }
}
