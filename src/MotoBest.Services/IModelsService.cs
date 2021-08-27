namespace MotoBest.Services
{
    using System.Collections.Generic;

    public interface IModelsService
    {
        IList<string> GetAllModelNamesByBrandName(string brandName);
    }
}
