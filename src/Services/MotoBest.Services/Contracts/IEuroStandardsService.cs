namespace MotoBest.Services.Contracts
{
    using System.Collections.Generic;

    using MotoBest.Models;
    using MotoBest.Services.DTOs;

    public interface IEuroStandardsService
    {
        IEnumerable<EuroStandardDto> GetAll();

        EuroStandard GetOrCreate(string euroStandardType);
    }
}
