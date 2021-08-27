namespace MotoBest.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Services.DTOs;
    using MotoBest.Services.Contracts;

    public class RegionsService : IRegionsService
    {
        private readonly ApplicationDbContext dbContext;

        public RegionsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<RegionDto> GetAll()
        {
            return dbContext.Regions
                            .OrderBy(region => region.Name)
                            .Select(region => new RegionDto { Id = region.Id, Name = region.Name })
                            .ToList();
        }

        public Region GetOrCreate(string regionName)
        {
            if (regionName == null)
            {
                return null;
            }

            return dbContext.Regions.FirstOrDefault(region => region.Name == regionName) ?? new Region { Name = regionName };
        }
    }
}
