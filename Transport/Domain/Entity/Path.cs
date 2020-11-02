namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Path")]
    public partial class Path
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Path()
        {
            Services = new HashSet<Service>();
            Waies = new HashSet<Way>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int DistanceId { get; set; }

        [Required]
        [StringLength(128)]
        public string UserCreated { get; set; }

        public DateTime TimeCreated { get; set; }

        [StringLength(128)]
        public string UserEdited { get; set; }

        public DateTime? TimeEdited { get; set; }

        public virtual Distance Distance { get; set; }

        public bool IsDelete { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<Way> Waies { get; set; }
    }
}
