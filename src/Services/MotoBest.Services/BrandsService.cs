namespace MotoBest.Services
{
    using System.Linq;
    using System.Collections.Generic;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Services.DTOs;
    using MotoBest.Services.Contracts;

    public class BrandsService : IBrandsService
    {
        private readonly ApplicationDbContext dbContext;

        public BrandsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<BrandDto> GetAll()
        {
            return dbContext.Brands
                            .OrderBy(brand => brand.Name)
                            .Select(brand => new BrandDto { Id = brand.Id, Name = brand.Name })
                            .ToList();
        }

        public Brand GetOrCreate(string brandName)
        {
            if (brandName == null)
            {
                return null;
            }

            return dbContext.Brands.FirstOrDefault(brand => brand.Name == brandName) ?? new Brand { Name = brandName };
        }
    }
}
