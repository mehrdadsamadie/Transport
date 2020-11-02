using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Domain.Entity;
using Domain.Repository;

namespace Transport.Areas.Manage.Models
{
    public class OverTimeViewModel
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display (Name = "تاریخ شروع")]
        [NoOverlap(ErrorMessage ="تاریخ شروع و پایان با داده های قبلی همپوشانی دارد")]
        public string StartDate { get; set; }
        [Display(Name = "تاریخ پایان")]
        [NoOverlap(ErrorMessage = "تاریخ شروع و پایان با داده های قبلی همپوشانی دارد")]
        public string EndDate { get; set; }

        [Display(Name = "نرخ اضافه کاری")]
        public decimal Price { get; set; }
        [Display(Name = "قفل شده")]
        public bool IsLock { get; set; }
    }




    public class NoOverlapAttribute : ValidationAttribute
    {
        OverTimeRepository _overtimeRepository = new OverTimeRepository();
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            OverTimeViewModel _overtimeViewModel = (OverTimeViewModel)validationContext.ObjectInstance;

            DateTime StartDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(_overtimeViewModel.StartDate, 0);
            DateTime EndDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(_overtimeViewModel.EndDate, 0);

            if(StartDate >= EndDate)
            {
                return new ValidationResult("تاریخ شروع باید کوچکتر از تاریخ پایان باشد");
            }

            if(_overtimeRepository.HasOverlap(_overtimeViewModel.Id, StartDate,EndDate))
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }


}