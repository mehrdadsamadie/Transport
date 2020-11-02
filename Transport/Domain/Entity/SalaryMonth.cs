namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SalaryMonth")]
    public partial class SalaryMonth
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? DriverId { get; set; }

        public int? TaxiCompanyId { get; set; }

        public DateTime CalcDate { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public decimal? SumDay { get; set; }

        public decimal? PriceDay { get; set; }

        public decimal? SumHours { get; set; }

        public decimal? SumService { get; set; }

        public decimal? PriceService { get; set; }

        public decimal? SumOverTime { get; set; }

        public decimal? PriceOverTime { get; set; }

        public decimal? SumDelay { get; set; }

        public decimal? PriceDelay { get; set; }

        public decimal? SumRush { get; set; }

        public decimal? PriceRush { get; set; }

        public decimal? SumDeductions { get; set; }

        public decimal? PriceDeductions { get; set; }

        public decimal? TotalPrice { get; set; }

        public bool IsLock { get; set; }

        public bool IsPay { get; set; }
    }
}
