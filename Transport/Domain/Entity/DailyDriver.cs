namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DailyDriver")]
    public partial class DailyDriver
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DailyDriver()
        {
            Services = new HashSet<Service>();
        }

        public int Id { get; set; }

        public int DriverId { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public TimeSpan? DelayHours { get; set; }

        public TimeSpan? RushHours { get; set; }

        public TimeSpan? AbsentHours { get; set; }

        public TimeSpan? DeductionsTotal { get; set; }

        public int StatusTypeId { get; set; }

        public int? DailyDayId { get; set; }

        public int? DriverTypeId { get; set; }

        public bool IsDelete { get; set; }

        public virtual DailyDay DailyDay { get; set; }

        public virtual Driver Driver { get; set; }

        public virtual DriverType DriverType { get; set; }

        public virtual PersonStatusType PersonStatusType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Service> Services { get; set; }
    }
}
