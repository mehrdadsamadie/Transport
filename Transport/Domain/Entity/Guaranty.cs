namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Guaranty")]
    public partial class Guaranty
    {
        public int Id { get; set; }

        public int ContractId { get; set; }

        public int DemandNoteType { get; set; }

        [Required]
        [StringLength(10)]
        public string Code { get; set; }

        public decimal Price { get; set; }

        public int? BankId { get; set; }

        [StringLength(30)]
        public string BankBranchName { get; set; }

        [StringLength(10)]
        public string BankBranchCode { get; set; }

        [StringLength(10)]
        public string AccountingCode { get; set; }

        public string Description { get; set; }

        [StringLength(20)]
        public string Image { get; set; }

        public virtual Bank Bank { get; set; }

        public virtual Contract Contract { get; set; }
    }
}
