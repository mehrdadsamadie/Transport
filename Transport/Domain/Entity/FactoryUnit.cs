namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FactoryUnit")]
    public partial class FactoryUnit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FactoryUnit()
        {
            FactoryUserManagers = new HashSet<FactoryUserManager>();
            Personnels = new HashSet<Personnel>();
            Requests = new HashSet<Request>();
            ManagerPersonels = new HashSet<ManagerPersonel>();
        }

        public int Id { get; set; }

        public int FactoryId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Telephone { get; set; }

        public string Address { get; set; }

        public virtual Factory Factory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactoryUserManager> FactoryUserManagers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Personnel> Personnels { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<ManagerPersonel> ManagerPersonels { get;set;}
    }
}
