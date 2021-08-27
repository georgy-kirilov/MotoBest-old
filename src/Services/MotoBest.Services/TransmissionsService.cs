namespace MotoBest.Services
{
    using System.Linq;
    using System.Collections.Generic;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Services.DTOs;
    using MotoBest.Services.Contracts;

    public class TransmissionsService : ITransmissionsService
    {
        private readonly ApplicationDbContext dbContext;

        public TransmissionsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<TransmissionDto> GetAll()
        {
            return dbContext.Transmissions
                            .OrderBy(transmission => transmission.Type)
                            .Select(transmission => new TransmissionDto { Id = transmission.Id, Type = transmission.Type })
                            .ToList();
        }

        public Transmission GetOrCreate(string transmissionType)
        {
            if (transmissionType == null)
            {
                return null;
            }

            return dbContext.Transmissions.FirstOrDefault(transmission => transmission.Type == transmissionType) ?? new Transmission { Type = transmissionType };
        }
    }
}
