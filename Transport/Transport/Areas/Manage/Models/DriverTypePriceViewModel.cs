using Domain.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Transport.Areas.Manage.Models
{
    public class DriverTypePriceIndex
    {
        public decimal? DailyPrice { get; set; }
        public string DriverTypeName { get; set; }
        public string EndDate { get; set; }
        public int Id { get; set; }
        public string StratDate { get; set; }
    }
    public class DriverTypePricesSave
    {
        [Required(ErrorMessage = "ورود مبلغ الزامی است")]
        [Display(Name = "مبلغ")]
        public decimal? DailyPrice { get; set; }
        [Required(ErrorMessage = "ورود نوع راننده الزامی است")]
        [Display(Name = "نوع راننده")]
        public int DriverTypeId { get; set; }
        [Display(Name = "تاریخ پایان")]
        public string EndDate { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage = "تاریخ شروع الزامی است")]
        [Display(Name ="تاریخ شروع")]
        [NoOverlapDriverTypePrice(ErrorMessage ="این تاریخ با تاریخ های قبلی تداخل دارد")]
        public string StartDate { get; set; }
    }
    public class NoOverlapDriverTypePriceAttribute : ValidationAttribute
    {
        DriverTypePriceRepository DriverTypePriceRepository = new DriverTypePriceRepository();
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            DriverTypePricesSave _driverTypePricesSave = (DriverTypePricesSave)validationContext.ObjectInstance;

            DateTime StartDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(_driverTypePricesSave.StartDate, 0);
            DateTime EndDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(_driverTypePricesSave.EndDate, 0);

            if (StartDate >= EndDate)
            {
                return new ValidationResult("تاریخ شروع باید کوچکتر از تاریخ پایان باشد");
            }
            var HasOverlap = DriverTypePriceRepository.HasOverlap(_driverTypePricesSave.Id, StartDate, EndDate);
            if (HasOverlap)
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}