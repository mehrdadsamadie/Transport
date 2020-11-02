namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VehicleDriver")]
    public partial class VehicleDriver
    {
        public int Id { get; set; }

        public int DriverId { get; set; }

        public int VehicleId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        [StringLength(128)]
        public string UserCreated { get; set; }

        public DateTime TimeCreated { get; set; }

        [StringLength(128)]
        public string UserEdited { get; set; }

        public DateTime? TimeEdited { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
        public virtual Driver Driver { get; set; }

        public virtual Vehicle Vehicle { get; set; }
    }
}
