using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Transport.Areas.Manage.Models
{
    public class TaxiCompanyIndex
    {
        public int Id { get; set; }
        [Display(Name = "نام")]
        public string Name { get; set; }
        [Display(Name = "تلفن ثابت")]
        public string CallPhone { get; set; }
        [Display(Name = "تلفن همراه")]
        public string MobilePhone { get; set; }
        [Display(Name = "نام مدیر")]
        public string ManagerName { get; set; }
        [Display(Name = "کد اقتصادی")]
        public string EconomicalCode { get; set; }
        [Display(Name = "کد رهگیری")]
        public string RegistrationNumber { get; set; }
    }

    public class TaxiSave
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "ورود نام الزامی است .")]
        [StringLength(100)]
        [Display(Name = "نام")]
        public string Name { get; set; }
        [StringLength(11)]
        [Display(Name = "تلفن ثابت", Order = 8)]
        [Required(ErrorMessage = "ورود شماره تلفن الزامی است .")]
        public string CellPhone { get; set; }
        [StringLength(11)]
        [Display(Name = "تلفن همراه", Order = 9)]
        public string MobilePhone { get; set; }
        [StringLength(100)]
        [Display(Name = "نام مدیر")]
        public string ManagerName { get; set; }
        
        [Display(Name = "کد اقتصادی")]
        public string EconomicalCode { get; set; }
      
        [Display(Name = "کد رهگیری")]
        public string RegistrationNumber { get; set; }

    }

}