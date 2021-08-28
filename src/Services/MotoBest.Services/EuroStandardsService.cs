namespace MotoBest.Services
{
    using System.Linq;
    using System.Collections.Generic;

    using MotoBest.Data;
    using MotoBest.Data.Models;
    using MotoBest.Services.DTOs;
    using MotoBest.Services.Contracts;

    public class EuroStandardsService : IEuroStandardsService
    {
        private readonly ApplicationDbContext dbContext;

        public EuroStandardsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<EuroStandardDto> GetAll()
        {
            return dbContext.EuroStandards
                            .OrderBy(euroStandard => euroStandard.Type)
                            .Select(euroStandard => new EuroStandardDto { Id = euroStandard.Id, Type = euroStandard.Type })
                            .ToList();
        }

        public EuroStandard GetOrCreate(string euroStandardType)
        {
            if (euroStandardType == null)
            {
                return null;
            }

            return dbContext.EuroStandards.FirstOrDefault(euroStandard => euroStandard.Type == euroStandardType) ?? new EuroStandard { Type = euroStandardType };
        }
    }
}
