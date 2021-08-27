namespace MotoBest.Services.Contracts
{
    using System.Collections.Generic;

    using MotoBest.Models;
    using MotoBest.Services.DTOs;

    public interface IColorsService
    {
        Color GetOrCreate(string colorName);

        IEnumerable<ColorDto> GetAll();
    }
}
