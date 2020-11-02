namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("ServiceType")]
    public partial class ServiceType
    {
        
        public ServiceType()
        {
            Requests = new HashSet<Request>();
            Services = new HashSet<Service>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        public virtual ICollection<Request> Requests { get; set; }

        public virtual ICollection<Service> Services { get; set; }
    }
}