namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SalaryDay")]
    public partial class SalaryDay
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int DailyDriverId { get; set; }

        public int? CountService { get; set; }

        public decimal? SumService { get; set; }

        public decimal? DailySalary { get; set; }
    }
}
