namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DailyDay")]
    public partial class DailyDay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DailyDay()
        {
            DailyDrivers = new HashSet<DailyDriver>();
        }

        public int Id { get; set; }

        public bool IsHoliday { get; set; }

        public int DailyTimeId { get; set; }

        public DateTime Date { get; set; }

        public virtual DailyTime DailyTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DailyDriver> DailyDrivers { get; set; }
    }
}
