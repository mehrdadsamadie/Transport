namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Way")]
    public partial class Way
    {
        public int Id { get; set; }
        public int PathId { get; set; }
        public string Name { get; set; }
        public string UserCreated { get; set; }
        public DateTime TimeCreated { get; set; }
        public string UserEdited { get; set; }
        public DateTime? TimeEdited { get; set; }
        public virtual Path Path { get; set; }
    }
}
