namespace MotoBest.Services.Contracts
{
    using System.Collections.Generic;

    using MotoBest.Data.Models;
    using MotoBest.Services.DTOs;

    public interface IEnginesService
    {
        IEnumerable<EngineDto> GetAll();

        Engine GetOrCreate(string engineType);
    }
}
