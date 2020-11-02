using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Transport.Areas.Manage.Models
{
    public class PathViewIndex
    {
        public int Id { get; set; }
        [Display(Name = "نام")]
        public string Name { get; set; }
        [Display(Name = "کیلومتر")]
        public int kilometer { get; set; }
    }
    public class PathViewSave
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "ورود نام الزامی است .")]
        [Display(Name = "نام")]
        public string Name { get; set; }
        [Display(Name ="مسافت")]
        [Required(ErrorMessage ="ورود مسافت الزامی است")]
        public int DistanceId { get; set; }



    }
}