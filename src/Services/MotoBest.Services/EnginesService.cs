namespace MotoBest.Services
{
    using System.Linq;
    using System.Collections.Generic;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Services.DTOs;
    using MotoBest.Services.Contracts;

    public class EnginesService : IEnginesService
    {
        private readonly ApplicationDbContext dbContext;

        public EnginesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<EngineDto> GetAll()
        {
            return dbContext.Engines
                            .OrderBy(engine => engine.Type)
                            .Select(engine => new EngineDto { Id = engine.Id, Type = engine.Type })
                            .ToList();
        }

        public Engine GetOrCreate(string engineType)
        {
            if (engineType == null)
            {
                return null;
            }

            return dbContext.Engines.FirstOrDefault(engine => engine.Type == engineType) ?? new Engine { Type = engineType };
        }
    }
}
