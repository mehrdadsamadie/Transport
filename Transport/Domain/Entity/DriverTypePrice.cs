namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DriverTypePrice")]
    public partial class DriverTypePrice
    {
        public int Id { get; set; }

        public decimal? DailyPrice { get; set; }

        public int DriverTypeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
       

        [StringLength(128)]
        public string UserCreated { get; set; }

        public DateTime? TimeCreated { get; set; }

        [StringLength(128)]
        public string UserEdited { get; set; }

        public DateTime? TimeEdited { get; set; }

        public bool IsDelete { get; set; }
        public bool IsLock { get; set; }
        public virtual DriverType DriverType { get; set; }
    }
}
