namespace MotoBest.Services
{
    using System.Linq;
    using System.Collections.Generic;

    using MotoBest.Data;

    public class ModelsService : IModelsService
    {
        private readonly ApplicationDbContext dbContext;

        public ModelsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IList<string> GetAllModelNamesByBrandName(string brandName)
        {
            return dbContext.Models.Where(model => model.Brand.Name == brandName).Select(model => model.Name).OrderBy(model => model).ToList();
        }
    }
}
