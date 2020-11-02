using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Transport.Areas.Manage.Models
{
    public class DailyDriverViewModel
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        [Required(ErrorMessage = "لطفا فیلد نام راننده را پر کنید")]
        [Display(Name = "راننده", Order = 1)]
        public int DriverId { get; set; }
        [Display(Name = "نام راننده", Order = 1)]
        public string DriverName { get; set; }
        [Display(Name = "وضعیت", Order = 1)]
        public string StatusTypeName { get; set; }
        [Display(Name = "نوع موقت", Order = 1)]
        public string DriverTypeName { get; set; }

        [Display(Name = "تاریخ", Order = 1)]
        [Required(ErrorMessage = "لطفا فیلد تاریخ را پر کنید")]
        public string Date { get; set; }
        [Display(Name = "زمان ورود", Order = 1)]
        [Required(ErrorMessage = "لطفا فیلد زمان ورود را پر کنید")]
        public TimeSpan? StartTime { get; set; }
        [Display(Name = "زمان خروج", Order = 1)]
        [Required(ErrorMessage = "لطفا فیلد زمان خروج را پر کنید")]
        public TimeSpan? EndTime { get; set; }
        [Display(Name = "میزان تاخیر", Order = 1)]
        public TimeSpan? DelayHours { get; set; }
        [Display(Name = "میزان تعجیل", Order = 1)]
        public TimeSpan? RushHours { get; set; }
        [Display(Name = "میزان غیبت", Order = 1)]
        public TimeSpan? AbsentHours { get; set; }
        [Display(Name = "جمع کسورات کاری", Order = 1)]
        public TimeSpan? DeductionsTotal { get; set; }
        [Display(Name = "وضعیت فرد", Order = 1)]
        public int StatusTypeId { get; set; }
        [Display(Name = "نوع موقت در روز", Order = 1)]
        public int? DriverTypeId { get; set; }
    }
}