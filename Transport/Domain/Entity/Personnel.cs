namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Personnel")]
    public partial class Personnel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Personnel()
        {
            Drivers = new HashSet<Driver>();
            FactoryUserManagers = new HashSet<FactoryUserManager>();
            Requests = new HashSet<Request>();
            ManagerPersonels = new HashSet<ManagerPersonel>();
            Managers = new HashSet<ManagerPersonel>();
        }

        public int Id { get; set; }

        public int? FactoryUnitId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        public bool Gender { get; set; }

        public string PersonnelCode { get; set; }

        [StringLength(10)]
        public string NationalCode { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(11)]
        public string CellPhone { get; set; }

        [StringLength(11)]
        public string Mobile { get; set; }

        [StringLength(11)]
        public string EmergencyPhone { get; set; }

        public int? AddressId { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public string Image { get; set; }
        public bool IsDelete { get; set; }
        public bool IsArchive { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return Name + " " + LastName; }
        }
        public virtual Address Address { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Driver> Drivers { get; set; }

        public virtual FactoryUnit FactoryUnit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactoryUserManager> FactoryUserManagers { get; set; }
        public virtual ICollection<ManagerPersonel> ManagerPersonels { get; set; }
        public virtual ICollection<ManagerPersonel> Managers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request> Requests { get; set; }
    }
}
