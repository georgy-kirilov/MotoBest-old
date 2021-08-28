namespace MotoBest.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MotoBest.Data.Models.Common;

    public class Advert : BaseModel<Guid>
    {
        public Advert()
        {
            Images = new HashSet<Image>();
            SavedUsers = new HashSet<ApplicationUser>();
        }

        [Required]
        public string RemoteId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public DateTime? ManufacturingDate { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public int? HorsePowers { get; set; }

        public long? Kilometrage { get; set; }

        public bool IsNewImport { get; set; }

        public bool? HasFourDoors { get; set; }

        public bool IsEuroStandardExact { get; set; }

        public bool IsExteriorMetallic { get; set; }

        public int? Views { get; set; }

        public int AdvertProviderId { get; set; }

        public virtual AdvertProvider AdvertProvider { get; set; }

        public int BodyStyleId { get; set; }

        public virtual BodyStyle BodyStyle { get; set; }

        public int? BrandId { get; set; }

        public virtual Brand Brand { get; set; }

        public int? ColorId { get; set; }

        public virtual Color Color { get; set; }

        public int ConditionId { get; set; }

        public virtual Condition Condition { get; set; }

        public int EngineId { get; set; }

        public virtual Engine Engine { get; set; }

        public int? EuroStandardId { get; set; }

        public virtual EuroStandard EuroStandard { get; set; }

        public int? ModelId { get; set; }

        public virtual Model Model { get; set; }

        public int RegionId { get; set; }

        public virtual Region Region { get; set; }

        public int? TownId { get; set; }

        public virtual Town Town { get; set; }

        public int TransmissionId { get; set; }

        public virtual Transmission Transmission { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<ApplicationUser> SavedUsers { get; set; }
    }
}
