namespace MotoBest.Services.Contracts
{
    using System.Collections.Generic;

    using MotoBest.Data.Models;
    using MotoBest.Services.DTOs;

    public interface IBrandsService
    {
        Brand GetOrCreate(string brandName);

        IEnumerable<BrandDto> GetAll();
    }
}
