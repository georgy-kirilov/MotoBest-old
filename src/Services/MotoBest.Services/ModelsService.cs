namespace MotoBest.Services
{
    using System.Linq;
    using System.Collections.Generic;

    using MotoBest.Data;
    using MotoBest.Data.Models;
    using MotoBest.Services.DTOs;
    using MotoBest.Services.Contracts;

    public class ModelsService : IModelsService
    {
        private readonly ApplicationDbContext dbContext;

        public ModelsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IList<ModelDto> GetAllModelsByBrandId(int brandId)
        {
            return dbContext.Models
                            .Where(model => model.Brand.Id == brandId)
                            .OrderBy(model => model.Name)
                            .Select(model => new ModelDto { Id = model.Id, Name = model.Name })
                            .ToList();
        }

        public Model GetOrCreate(Brand brand, string modelName)
        {
            if (modelName == null)
            {
                return null;
            }

            return brand.Models.FirstOrDefault(model => model.Name == modelName) ?? new Model { Name = modelName };
        }
    }
}
