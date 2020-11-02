namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FactoryUserManager")]
    public partial class FactoryUserManager
    {
        public int Id { get; set; }

        public int PersonnelId { get; set; }

        public int FactoryUnitId { get; set; }

        public bool IsDelete { get; set; }

        public virtual FactoryUnit FactoryUnit { get; set; }

        public virtual Personnel Personnel { get; set; }
    }
}
