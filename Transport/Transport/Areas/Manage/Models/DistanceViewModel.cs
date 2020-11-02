using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Transport.Areas.Manage.Models
{
    public class DistanceViewIndex
    {
        public int Id { get; set; }
        [Display(Name = "کیلومتر")]
        public int kilometer { get; set; }
    }
    public class DistanceSave
    {
        public int Id { get; set; }
      
        [Display(Name = "کیلومتر")]
        [Required(ErrorMessage ="ورود مسافت الزامی است")]
        public int? kilometer { get; set; }
    }
}