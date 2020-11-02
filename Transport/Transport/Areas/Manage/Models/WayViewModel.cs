using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Transport.Areas.Manage.Models
{
    public class WaySave
    {
        
        public int Id { get;  set; }
        [Display(Name = "مسیر")]
        [Required(ErrorMessage = " ورود مسیر الزامی است")]
        public string Name { get;  set; }
        [Display]
        [Required(ErrorMessage ="ورود کد مسیر الزامی است")]
        public int PathId { get;  set; }
    }
    public class WayIndex
    {
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public string PathName { get; internal set; }
    }
}