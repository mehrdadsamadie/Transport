namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Address")]
    public partial class Address
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Address()
        {
            Factories = new HashSet<Factory>();
            Personnels = new HashSet<Personnel>();
            RequestsBeginingAddress = new HashSet<Request>();
            RequestsDestinationAddress = new HashSet<Request>();
            ServicesBeginingAddress = new HashSet<Service>();
            ServicesDestinationAddress = new HashSet<Service>();
        }

        public int Id { get; set; }
        [StringLength(20)]
        public string CountryName { get; set; }

        public int? EparchyId { get; set; }

        [StringLength(100)]
        public string CityName { get; set; }

        [StringLength(100)]
        public string RegionName { get; set; }

        [Column("Address")]
        [StringLength(256)]
        public string Address1 { get; set; }

        [StringLength(11)]
        public string PostalCode { get; set; }

        public virtual Eparchy Eparchy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Factory> Factories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Personnel> Personnels { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request> RequestsBeginingAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request> RequestsDestinationAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Service> ServicesBeginingAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Service> ServicesDestinationAddress { get; set; }
    }
}
