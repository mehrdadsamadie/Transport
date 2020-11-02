using Domain.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Transport.Areas.Manage.Models
{
    public class DelayPriceViewIndex
    {
        public int DriverTypeId { get; set; }
        public string DriverTypeName { get; set; }
        public string EndDate { get; set; }
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int DelayTime { get; set; }
        public string StartDate { get; set; }
    }
    public class DelayPriceSave
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "انتخاب نوع راننده الزامی است")]
        [Display(Name = "ورود نوع راننده الزامی است")]
        public int DriverTypeId { get; set; }

        [Display(Name = "تاریخ پایان")]
        [Required(ErrorMessage  = "ورود تاریخ پایان الزامی است")]
        public string EndDate { get; set; }

        [Required(ErrorMessage = "ورود قیمت الزامی است")]
        [Display(Name = "قیمت")]
        public decimal Price { get; set; }

        [Required(ErrorMessage ="ورود زمان الرامی است")]
        [Display(Name ="زمان")]
        public int DelayTime { get; set; }

        [Required(ErrorMessage = "ورود تاریخ شروع الزامی است")]
        [Display(Name = "زمان شروع")]
        [NoOverlapDelayPrice(ErrorMessage = "این تاریخ با تاریخ های قبلی تداخل دارد")]
        public string StartDate { get; set; }
    }
    public class NoOverlapDelayPriceAttribute : ValidationAttribute
    {
        DelayPriceRepository DelayPriceRepository = new DelayPriceRepository();
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            DelayPriceSave _delayPriceSave = (DelayPriceSave)validationContext.ObjectInstance;

            DateTime StartDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(_delayPriceSave.StartDate, 0);
            DateTime EndDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(_delayPriceSave.EndDate, 0);

            if (StartDate >= EndDate)
            {
                return new ValidationResult("تاریخ شروع باید کوچکتر از تاریخ پایان باشد");
            }
            var HasOverlap = DelayPriceRepository.HasOverlap(_delayPriceSave.Id, _delayPriceSave.DriverTypeId, StartDate, EndDate);
            if (HasOverlap)
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}