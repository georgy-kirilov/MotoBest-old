﻿namespace MotoBest.Models.Common
{
    using System.ComponentModel.DataAnnotations;

    public abstract class BaseModel<T>
    {
        [Key]
        public T Id { get; set; }
    }
}
