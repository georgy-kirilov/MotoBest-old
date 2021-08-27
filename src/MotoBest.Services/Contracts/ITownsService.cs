namespace MotoBest.Services.Contracts
{
    using System.Collections.Generic;

    using MotoBest.Models;
    using MotoBest.Services.DTOs;

    public interface ITownsService
    {
        IEnumerable<TownDto> GetAllTownsByRegionId(int regionId);

        Town GetOrCreate(Region region, string townName);
    }
}
