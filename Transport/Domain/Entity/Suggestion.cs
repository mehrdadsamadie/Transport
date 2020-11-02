namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    [Table("Suggestion")]
    public partial class Suggestion
    {
        public int Id { get; set; }

        [StringLength(256)]
        public string Title { get; set; }

        [StringLength(20)]
        public string CountryName { get; set; }

        public int EparchyId { get; set; }

        [StringLength(100)]
        public string CityName { get; set; }

        [StringLength(100)]
        public string RegionName { get; set; }

        [StringLength(256)]
        public string Address { get; set; }

        [StringLength(11)]
        public string PostalCode { get; set; }

        public virtual Eparchy Eparchy { get; set; }
    }
}
