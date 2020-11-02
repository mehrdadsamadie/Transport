namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Service")]
    public partial class Service
    {
        public Service()
        {
            Requests = new HashSet<Request>();
        }
        public int Id { get; set; }

        public int? DriverId { get; set; }

        public int? TaxiCompanyId { get; set; }

        [StringLength(50)]
        public string DriverName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public int? PathId { get; set; }

        public int? DistancePriceId { get; set; }

        public int? GussetNumber { get; set; }

        public int? ServiceTypeId { get; set; }
        public int? Biginning { get; set; }

        public int? Destination { get; set; }

        public int? DailyDriverId { get; set; }

        public int? DriverTypeId { get; set; }

        public decimal? RealDistance { get; set; }

        public decimal? CalcDistance { get; set; }

        public bool IsDelete { get; set; }

        public bool IsLock { get; set; }
        public bool IsLocal { get; set; }

        public int ServiceStatusId { get; set; }

        [Required]
        [StringLength(128)]
        public string UserCreated { get; set; }

        public DateTime TimeCreated { get; set; }

        [StringLength(128)]
        public string UserEdited { get; set; }

        public DateTime? TimeEdited { get; set; }

        public TimeSpan? DelayTime { get; set; }

        public bool? IsAcceptTranasportManager { get; set; }

        [StringLength(128)]
        public string UserAcceptTranasportManager { get; set; }

        public DateTime? TimeAcceptTranasportManager { get; set; }

        public string Description { get; set; }

        public virtual Address BeginingAddress { get; set; }

        public virtual Address DestinationAddress  { get; set; }

        public virtual DailyDriver DailyDriver { get; set; }

        public virtual DistancePrice DistancePrice { get; set; }

        public virtual Driver Driver { get; set; }

        public virtual DriverType DriverType { get; set; }

        public virtual Path Path { get; set; }

        public virtual TaxiCompany TaxiCompany { get; set; }
        public virtual ServiceStatu ServiceStatu { get; set; }
        public virtual ServiceType ServiceType { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
