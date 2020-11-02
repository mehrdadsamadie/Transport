namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Vehicle")]
    public partial class Vehicle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vehicle()
        {
            Contracts = new HashSet<Contract>();
            VehicleDrivers = new HashSet<VehicleDriver>();
        }
        public int Id { get; set; }

        public int? VehicleTypeId { get; set; }

        public int? VehicleGroup { get; set; }

        public int LicensePlateTypeId { get; set; }

        public string LicensePlateNum { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [StringLength(30)]
        public string ModelName { get; set; }

        [StringLength(20)]
        public string Factory { get; set; }

        [Column(TypeName = "date")]
        public DateTime ProductYear { get; set; }

        [StringLength(10)]
        public string Color { get; set; }

        public int? Capacity { get; set; }

        public bool HasTechnicalCheckup { get; set; }

        public decimal TechnicalCheckupNumber { get; set; }

        public DateTime? TechnicalCheckupStartDate { get; set; }

        public DateTime? TechnicalCheckupEndDate { get; set; }

        [StringLength(20)]
        public string MotorNum { get; set; }

        [StringLength(20)]
        public string ChassisNum { get; set; }

        [StringLength(20)]
        public string VehicleInsuranceNum { get; set; }

        public DateTime? VehicleInsuranceStartDate { get; set; }

        public DateTime? VehicleInsuranceEndDate { get; set; }

        [StringLength(20)]
        public string PersonInsuranceNum { get; set; }

        public DateTime? PersonInsuranceStartDate { get; set; }

        public DateTime? PersonInsuranceEndDate { get; set; }

        [Required]
        [StringLength(128)]
        public string UserCreated { get; set; }

        public DateTime TimeCreated { get; set; }

        [StringLength(128)]
        public string UserEdited { get; set; }

        public DateTime? TimeEdited { get; set; }

        public bool IsDelete { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contract> Contracts { get; set; }

        public virtual VehicleType VehicleType { get; set; }
        public virtual LicensePlateType LicensePlateType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VehicleDriver> VehicleDrivers { get; set; }
    }
}
