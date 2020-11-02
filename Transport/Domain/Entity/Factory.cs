namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Factory")]
    public partial class Factory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Factory()
        {
            FactoryUnits = new HashSet<FactoryUnit>();
        }

        public int Id { get; set; }

        public int? AddressId { get; set; }

        [StringLength(40)]
        public string Name { get; set; }

        [StringLength(10)]
        public string CellPhone { get; set; }

        public virtual Address Address { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactoryUnit> FactoryUnits { get; set; }
    }
}
