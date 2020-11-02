namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Request")]
    public partial class Request
    {
        public int Id { get; set; }

        public int PersonelId { get; set; }

        public int? FactoryUnitId { get; set; }

        public DateTime? RequestDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ServiceDate { get; set; }

        public int? GussetNumber { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public string Description { get; set; }

        public bool IsLock { get; set; }

        public bool IsDelete { get; set; }

        public int IsAcceptTranasport { get; set; }

        public int IsAcceptManager { get; set; }

        public int? BiginningAddressId { get; set; }

        public int? DestinationAddressId { get; set; }

        public bool? IsEmergancy { get; set; }

        public int? ServiceTypeId { get; set; }

        public DateTime? AcceptTranasportTime { get; set; }

        public DateTime? AcceptManagerTime { get; set; }

        [StringLength(128)]
        public string UserAcceptTranasport { get; set; }

        [StringLength(128)]
        public string UserAcceptManager { get; set; }

        public bool IsLocal { get; set; }

        public DateTime? TimeEdited { get; set; }

        [StringLength(128)]
        public string UserEdited { get; set; }
        
        public DateTime TimeCreated { get; set; }

        [StringLength(128)]
        public string UserCreated { get; set; }

        public int? ServiceId { get; set; }
        public bool IsCanceled { get; set; }

        public virtual Address BeginingAddress { get; set; }

        public virtual Address DestinationAddress { get; set; }

        public virtual FactoryUnit FactoryUnit { get; set; }

        public virtual Personnel Personnel { get; set; }
        
        public virtual Service Service { get; set; }
        public virtual ServiceType ServiceType { get; set; }
    }
}
