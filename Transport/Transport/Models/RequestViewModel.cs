using Domain.Entity;
using Domain.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Transport.Areas.Manage.Models;

namespace Transport.Models
{
    public class RequestViewModelIndex
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Display(Name = "آیدی درخواست کننده")]
        public int PersonelId { get; set; }
        [Display(Name = "درخواست کننده")]
        public string PersonelName { get; set; }
        [Display(Name = "درخواست از واحد")]
        public int? FactoryUnitId { get; set; }
        [Display(Name = "تاریخ درخواست")]
        public string RequestDate { get; set; }
        [Display(Name = "تاریخ ")]
        public string ServiceDate { get; set; }
        [Display(Name = "ساعت شروع")]
        public TimeSpan? StartTime { get; set; }
        [Display(Name = "  ساعت پایان")]
        public TimeSpan? EndTime { get; set; }
        [Display(Name = "  توضیحات")]
        public string Description { get; set; }
        [Display(Name = "  قفل")]
        public bool IsLock { get; set; }
        [Display(Name = "پذیرش توسط نقلیه")]
        public bool? IsAcceptTranasport { get; set; }
        [Display(Name = "پذیرش توسط مدیریت")]
        public bool? IsAcceptManager { get; set; }
        [Display(Name = "مبدا مسیر")]
        public string Biginning { get; set; }
        [Display(Name = "مقصد مسیر")]
        public string Destination { get; set; }
        [Display(Name = "ضروری است یا نه")]
        public bool? IsEmergancy { get; set; }
        [Display(Name = "زمان پذیرش نقلیه")]
        public DateTime? AcceptTranasportTime { get; set; }
        [Display(Name = "مقصد مسیرزمان پذیرش مدیریت")]
        public DateTime? AcceptManagerTime { get; set; }
        [Display(Name = "فرد تایید کننده نقلیه")]
        [StringLength(128)]
        public string UserAcceptTranasport { get; set; }
        [Display(Name = "فرد تایید کننده مدیریت")]
        [StringLength(128)]
        public string UserAcceptManager { get; set; }
        public int? ServiceId { get;  set; }
        public bool IsLocal { get;  set; }
        public bool IsCanceled { get; set; }
    }
    public class RequestSave
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Display(Name = "درخواست کننده")]
        [Required(ErrorMessage ="ورود درخواست کننده الزامی است")]
        [PersonnelValid]
        public int PersonelId { get; set; }
        [Display(Name = "تاریخ درخواست")]
        [Required(ErrorMessage = "ورود تاریخ درخواست الزامی است")]
        public string ServiceDate { get; set; }
        [Display(Name = "ساعت شروع")]
        [Required(ErrorMessage ="ورود زمان شروع الزامی است")]
        public TimeSpan? StartTime { get; set; }
        [Display(Name = "  ساعت پایان")]
        public TimeSpan? EndTime { get; set; }
        [Display(Name = "  توضیحات")]
        public string Description { get; set; }
        [Display(Name = "مبدا مسیر")]
        public AddressSaveView BiginningAddress { get; set; }
        [Display(Name = "مقصد مسیر")]
        public AddressSaveView DestinationAddress { get; set; }
        [Display(Name = "ضروری است یا نه")]
        public bool? IsEmergancy { get; set; } = false;
        public string FullName { get;  set; }
        [Required(ErrorMessage = "ورود نوع سفر الزامی است")]
        [Display(Name = "سفر داخل شهرک")]
        public bool IsLocal { get; set; } = true;
        public int? ServiceId { get;  set; }
        [Display(Name = "تعداد همراهان")]
        public int? GussetNumber { get; set; } = 0;

        [Display(Name = "نوع سفر")]
        [Required(ErrorMessage = "انتخاب نوع سفر الزامی است")]
        public int? ServiceTypeId { get; set; } = 1;

        public bool? IsMobile { get; set; } = false;
        public string StartTimeStr { get;  set; }
        public bool Editable { get; internal set; }

        public class StateView
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }
    }
    public class PersonnelValidAttribute : ValidationAttribute
    {
        ManagerPersonelRepository ManagerPersonelRepository = new ManagerPersonelRepository();
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
         
            RequestSave RequestSave = (RequestSave)validationContext.ObjectInstance;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser currentUser = UserManager.FindById(HttpContext.Current.User.Identity.GetUserId());
            var _managers = ManagerPersonelRepository.GetManagerOfPersonnel(RequestSave.PersonelId);
            if (RequestSave.PersonelId == currentUser.PersonnelId || _managers.Any(x => x.ManagerPersonelId == currentUser.PersonnelId))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("دسترسی برای ثبت در خواست برای این کابر امکان پذیر نمیباشد");

        }
    }
}
    
