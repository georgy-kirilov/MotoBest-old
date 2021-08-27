namespace MotoBest.Services
{
    using System.Collections.Generic;

    public interface ITownsService
    {
        IEnumerable<string> GetAllTownNamesByRegionName(string regionName);
    }
}
