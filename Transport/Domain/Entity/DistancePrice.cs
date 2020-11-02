namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DistancePrice")]
    public partial class DistancePrice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DistancePrice()
        {
            Services = new HashSet<Service>();
        }

        public int Id { get; set; }

        public int DistanseId { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        public int DriverTypeId { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool IsLock { get; set; }
        public bool IsDelete { get; set; }

        [Required]
        [StringLength(128)]
        public string UserCreated { get; set; }

        public DateTime TimeCreated { get; set; }

        [StringLength(128)]
        public string UserEdited { get; set; }

        public DateTime? TimeEdited { get; set; }

        public virtual Distance Distance { get; set; }

        public virtual DriverType DriverType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Service> Services { get; set; }
    }
}
