namespace MotoBest.Services.Contracts
{
    using System.Collections.Generic;

    using MotoBest.Models;
    using MotoBest.Services.DTOs;

    public interface IEnginesService
    {
        IEnumerable<EngineDto> GetAll();

        Engine GetOrCreate(string engineType);
    }
}
