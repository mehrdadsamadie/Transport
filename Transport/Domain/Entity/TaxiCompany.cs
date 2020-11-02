namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaxiCompany")]
    public partial class TaxiCompany
    {
        public TaxiCompany()
        {
            Services = new HashSet<Service>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(12)]
        public string CellPhone { get; set; }

        [StringLength(12)]
        public string MobilePhone { get; set; }

        [StringLength(50)]
        public string ManagerName { get; set; }

        [StringLength(50)]
        public string EconomicalCode { get; set; }

        [StringLength(50)]
        public string RegistrationNumber { get; set; }

        public bool IsDelete { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
