namespace MotoBest.Services.Contracts
{
    using System.Collections.Generic;

    using MotoBest.Data.Models;
    using MotoBest.Services.DTOs;

    public interface IBodyStylesService
    {
        IEnumerable<BodyStyleDto> GetAll();

        BodyStyle GetOrCreate(string bodyStyleName);
    }
}
