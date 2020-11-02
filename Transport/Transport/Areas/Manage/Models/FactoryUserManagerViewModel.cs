using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Transport.Areas.Manage.Models
{
    public class FactoryUserManagerViewIndex
    {
        public int FactoryUnitId { get;  set; }
        public string FullName { get;  set; }
        public int Id { get;  set; }
        public string Name { get;  set; }
        public int PersonnelId { get;  set; }
    }
    public class FactoryUserManagerSave
    {

        [Display(Name = "کارخانه")]
        [Required(ErrorMessage ="انتخاب کارخانه الزامی است")]
        public int FactoryId { get;  set; }
        [Required(ErrorMessage ="انتخاب واحد الزامی است")]
        [Display(Name ="واحد الزامی است")]
        public int FactoryUnitId { get;  set; }
        [Display(Name ="نام")]
        public string FullName { get;  set; }
        public int Id { get;  set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        [Required(ErrorMessage ="ورود نام پرسنل الزامی است")]
        public int PersonnelId { get;  set; }
    }
}