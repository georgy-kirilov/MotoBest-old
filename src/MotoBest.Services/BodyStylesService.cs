namespace MotoBest.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Services.DTOs;
    using MotoBest.Services.Contracts;

    public class BodyStylesService : IBodyStylesService
    {
        private readonly ApplicationDbContext dbContext;

        public BodyStylesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<BodyStyleDto> GetAll()
        {
            return dbContext.BodyStyles
                            .OrderBy(bodyStyle => bodyStyle.Name)
                            .Select(bodyStyle => new BodyStyleDto { Id = bodyStyle.Id, Name = bodyStyle.Name })
                            .ToList();
        }

        public BodyStyle GetOrCreate(string bodyStyleName)
        {
            if (bodyStyleName == null)
            {
                return null;
            }

            return dbContext.BodyStyles.FirstOrDefault(bodyStyle => bodyStyle.Name == bodyStyleName) ?? new BodyStyle { Name = bodyStyleName };
        }
    }
}
