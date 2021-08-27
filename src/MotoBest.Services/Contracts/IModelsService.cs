namespace MotoBest.Services.Contracts
{
    using System.Collections.Generic;

    using MotoBest.Models;
    using MotoBest.Services.DTOs;

    public interface IModelsService
    {
        IList<ModelDto> GetAllModelsByBrandId(int brandId);

        Model GetOrCreate(Brand brand, string modelName);
    }
}
