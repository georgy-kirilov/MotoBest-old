namespace MotoBest.Services
{
    using System.Linq;
    using System.Collections.Generic;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Services.DTOs;
    using MotoBest.Services.Contracts;

    public class ColorsService : IColorsService
    {
        private readonly ApplicationDbContext dbContext;

        public ColorsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<ColorDto> GetAll()
        {
            return dbContext.Colors
                            .OrderBy(color => color.Name)
                            .Select(color => new ColorDto { Id = color.Id, Name = color.Name })
                            .ToList();
        }

        public Color GetOrCreate(string colorName)
        {
            if (colorName == null)
            {
                return null;
            }

            return dbContext.Colors.FirstOrDefault(color => color.Name == colorName) ?? new Color { Name = colorName };
        }
    }
}
