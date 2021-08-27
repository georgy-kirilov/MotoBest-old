namespace MotoBest.Services
{
    using System.Linq;
    using System.Collections.Generic;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Services.DTOs;
    using MotoBest.Services.Contracts;

    public class ConditionsService : IConditionsService
    {
        private readonly ApplicationDbContext dbContext;

        public ConditionsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<ConditionDto> GetAll()
        {
            return dbContext.Conditions
                            .OrderBy(condition => condition.Type)
                            .Select(condition => new ConditionDto { Id = condition.Id, Type = condition.Type })
                            .ToList();
        }

        public Condition GetOrCreate(string conditionType)
        {
            if (conditionType == null)
            {
                return null;
            }

            return dbContext.Conditions.FirstOrDefault(condition => condition.Type == conditionType) ?? new Condition { Type = conditionType };
        }
    }
}
