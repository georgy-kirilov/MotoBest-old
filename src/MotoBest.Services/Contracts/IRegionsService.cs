namespace MotoBest.Services.Contracts
{
    using System.Collections.Generic;

    using MotoBest.Models;
    using MotoBest.Services.DTOs;

    public interface IRegionsService
    {
        IEnumerable<RegionDto> GetAll();

        Region GetOrCreate(string regionName);
    }
}
