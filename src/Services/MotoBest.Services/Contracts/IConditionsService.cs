namespace MotoBest.Services.Contracts
{
    using System.Collections.Generic;

    using MotoBest.Models;
    using MotoBest.Services.DTOs;

    public interface IConditionsService
    {
        IEnumerable<ConditionDto> GetAll();

        Condition GetOrCreate(string conditionType);
    }
}
