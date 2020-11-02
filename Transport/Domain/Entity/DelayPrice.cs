namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DelayPrice")]
    public partial class DelayPrice
    {
        public int Id { get; set; }

        public int DriverTypeId { get; set; }

        public decimal Price { get; set; }
        public int DelayTime { get; set; }
        public DateTime TimeCreate { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        [StringLength(128)]
        public string UserCreated { get; set; }

        public DateTime? TimeEdited { get; set; }

        [StringLength(128)]
        public string UserEdited { get; set; }

        public bool IsDelete { get; set; }

        public virtual DriverType DriverType { get; set; }
    }
}
