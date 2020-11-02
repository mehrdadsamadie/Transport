using Domain.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Transport.Areas.Manage.Models
{
    public class DailyTimeViewModel
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Required (ErrorMessage ="ورود تاریخ شروع الزامی است") ]
        [Display(Name = "تاریخ شروع", Order = 3)]
        public string StartDate { get; set; }
        [Required(ErrorMessage = "ورود تاریخ پایان الزامی است")]
        [Display(Name = "تاریخ پایان", Order = 3)]
        public string EndDate { get; set; }
        [Required(ErrorMessage = "ورود روز هفته الزامی است")]
        [Display(Name = "روز هفته", Order = 2)]
        public string WeekenDay { get; set; }
        [Required(ErrorMessage = "ورود ساعت شروع کار الزامی است")]
        [Display(Name = "ساعت شروع کار", Order = 1)]
        [NoOverlaDailyTimePrice(ErrorMessage ="این تاریخ با تاریخ های قبلی تداخل دارد")]
        public TimeSpan? StartTime { get; set; }
        [Required(ErrorMessage = "ورود ساعت پایان کار الزامی است")]
        [Display(Name = "ساعت پایان کار", Order = 1)]
        [EndTime]
        public TimeSpan? EndTime { get; set; }

    }
    public class NoOverlaDailyTimePriceAttribute : ValidationAttribute
    {
        DailyTimeRepository DailyTimeRepository = new DailyTimeRepository();
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            DailyTimeViewModel _dailyTimeViewModel = (DailyTimeViewModel)validationContext.ObjectInstance;

            DateTime StartDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(_dailyTimeViewModel.StartDate, 0);
            DateTime EndDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(_dailyTimeViewModel.EndDate, 0);

            if (StartDate >= EndDate)
            {
                return new ValidationResult("تاریخ شروع باید کوچکتر از تاریخ پایان باشد");
            }
            var HasOverlap = DailyTimeRepository.HasOverlap(_dailyTimeViewModel.Id, int.Parse(_dailyTimeViewModel.WeekenDay), StartDate, EndDate);
            if (HasOverlap)
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
    public class EndTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            DailyTimeViewModel _dailyTimeViewModel = (DailyTimeViewModel)validationContext.ObjectInstance;
            if (_dailyTimeViewModel.EndTime <= _dailyTimeViewModel.StartTime)
            {
                return new ValidationResult("تاریخ شروع باید کوچکتر از تاریخ پایان باشد");
            }
            return ValidationResult.Success;
        }
    }
}