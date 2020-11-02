using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Transport.Areas.Manage.Models
{
    public class DriverView
    {
        public int DriverId { get; set; }
        [Display(Name = "نام و نام خانوادگی")]

        public string FullName { get; set; }
        [Display(Name = "کد پرسنلی")]
        public string PersonelCode { get; set; }
        public string DriverType { get; set; }
        public bool IsHistory { get; set; }
        [Display(Name = "فعال")]
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsBlack { get; set; }

    }
    public class DriverSave
    {
        [Display(Name = "پرسنل", Order = 1)]
        [Required(ErrorMessage ="انتخاب پرسنل الزامی است")]
        public int PersonelId { get; set; }
    }
    public class DriverDetail
    {
        public DriverDetail()
        {
            DriverTypes = new List<DriverTypeView>();
            DriverHistories = new List<DriverHistorySave>();
        }
        public List<DriverTypeView> DriverTypes { get; set; }
        public List<DriverHistorySave> DriverHistories { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; internal set; }
        [Display(Name = "کد پرسنلی")]
        public string PersonnelCode { get; internal set; }

        public DriverTypeView NewDriverType { get; set; }
        public DriverHistorySave NewDriverHistory { get; set; }
        public int Id { get; internal set; }
    }
    public class DriverTypeView
    {
        [Display(Name = "پایان")]
        public string EndDate { get;  set; }
        [Display(Name = "فعال")]
        public bool IsActive { get;  set; }
        [Required(ErrorMessage ="ورود نوع راننده الزامی است")]
        [Display(Name = "نوع راننده")]
        public int DriverTypeId { get;  set; }
        [Required (ErrorMessage ="ورود تاریخ الزامی است")]
        [Display(Name = "شروع")]
        public string StartDate { get;  set; }
        public int Id { get;  set; }
        public int DriverId { get; set; }
    }
    public class DriverHistorySave
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        [Display(Name ="تاریخ")]
        [Required(ErrorMessage ="ورود تاریخ الزامی است")]
        public string Date { get; set; }
        [Display(Name ="شرح")]
        [Required(ErrorMessage ="ورود شرح الزامی است")]
        public string Description { get; set; }

    }
}