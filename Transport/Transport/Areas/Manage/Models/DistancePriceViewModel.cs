using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Transport.Areas.Manage.Models
{
    public class DistancePriceIndex
    {
        public int kilometer { get; set; }
        public string DriverTypeName { get; set; }
        public int Id { get; set; }
        public string PathList { get; set; }
        public decimal Price { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    public class DistancePriceSave
    {
        [Required(ErrorMessage = "ورود مسافت الزامی است")]
        [Display(Name = "مسافت")]
        public int DistanseId { get; set; }
        [Required(ErrorMessage = "ورود نوع راننده الزامی است")]
        [Display(Name = "نوع راننده")]
        public int DriverTypeId { get; set; }
        [Display(Name = "تاریخ پایان")]
        [Required(ErrorMessage ="ورود تاریخ پایان الزامی است")]
        public string EndTime { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage = "ورود قیمت الزامی است")]
        [Display(Name = "قیمت")]
        public decimal Price { get; set; }
        [Display(Name = "تاریخ شروع")]
        [NoOverlapDistancePrice(ErrorMessage ="این تاریخ با تاریخ های قبلی تداخل دارد")]
        [Required(ErrorMessage ="ورود تاریخ شروع الزامی است")]
        public string StartTime { get; set; }
    }
    public class NoOverlapDistancePriceAttribute : ValidationAttribute
    {
        DistancePriceRepository DistancePriceRepository = new DistancePriceRepository();
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            DistancePriceSave _distancePriceSave = (DistancePriceSave)validationContext.ObjectInstance;

            DateTime StartDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(_distancePriceSave.StartTime, 0);
            DateTime EndDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(_distancePriceSave.EndTime, 0);

            if (StartDate >= EndDate)
            {
                return new ValidationResult("تاریخ شروع باید کوچکتر از تاریخ پایان باشد");
            }
            var HasOverlap = DistancePriceRepository.HasOverlap(_distancePriceSave.Id,_distancePriceSave.DistanseId,_distancePriceSave.DriverTypeId, StartDate, EndDate);
            if (HasOverlap)
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}
