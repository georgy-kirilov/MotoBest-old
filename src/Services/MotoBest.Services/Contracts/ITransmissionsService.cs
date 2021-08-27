namespace MotoBest.Services.Contracts
{
    using System.Collections.Generic;

    using MotoBest.Models;
    using MotoBest.Services.DTOs;

    public interface ITransmissionsService
    {
        IEnumerable<TransmissionDto> GetAll();

        Transmission GetOrCreate(string transmissionType);
    }
}
