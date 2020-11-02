using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Transport.Areas.Manage.Models
{
    public class VehicleViewIndex
    {
        public int? DriverId { get; set; }
        public string FullName { get; set; }
        public int Id { get; set; }
        public string LicensePlateNum { get; set; }
        public string Model { get; set; }
        public string VehicleType { get; set; }
    }
    public class VehicleSave
    {
        [Display(Name = "ظرفیت")]
        [Required(ErrorMessage = " ورود ظرفیت الزامی است")]
        public int? Capacity { get; set; }

        [Display(Name = "شماره شاسی")]
        public string ChassisNum { get; set; }
        [Display(Name = "رنگ")]
        [Required(ErrorMessage = "ورود رنگ الزامی است")]
        public string Color { get; set; }
        public string DriverFullName { get; internal set; }
        [HiddenInput(DisplayValue =false)]
        public int? DriverId { get; internal set; }
        [Display(Name = "شرکت سازنده")]
        [Required(ErrorMessage = "ورود کارخانه الزامی است")]
        public string Factory { get; set; }
        [Display(Name = "معاینه فنی دارد؟")]
        public bool HasTechnicalCheckup { get; set; }

        public int Id { get; set; }
        [Display(Name = "فعال است")]
        public bool IsActive { get; set; }
        [Display(Name = "پلاک خودرو")]
        [Required(ErrorMessage = "ورود پلاک الزامی است")]
        public string LicensePlateNum { get; set; }
        [Required(ErrorMessage = "ورود نوع پلاک الزامی است")]
        [Display(Name = "نوع پلاک")]
        public int LicensePlateTypeId { get; set; }
        [Display(Name = "مدل خودرو")]
        [Required(ErrorMessage = "ورود مدل خودرو الزامی است")]
        public string ModelName { get; set; }

        [Display(Name = "شماره موتور")]
        public string MotorNum { get; set; }

        [Display(Name = " پایان بیمه شخص ثالث")]
        [Required(ErrorMessage = "ورود پایان بیمه شخص ثالث الزامی است")]
        public string PersonInsuranceEndDate { get; set; }

        [Required(ErrorMessage = "ورود شماره بیمه شخص ثالث الزامی است")]
        [Display(Name = "شماره بیمه شخص ثالث")]
        public string PersonInsuranceNum { get; set; }

        [Required(ErrorMessage = "ورود شروع بیمه ملت الزامی است")]
        [Display(Name = "شروع بیمه شخص ثالث")]
        [PersonInsuranceStartDate]
        public string PersonInsuranceStartDate { get; set; }

        [Display(Name = "تاریخ تولید")]
        [Required(ErrorMessage = "ورود تاریخ ساخت الزامی است")]
        public string ProductYear { get; set; }

        [Display(Name = "پایان معاینه فنی")]
        public string TechnicalCheckupEndDate { get; set; }

        [Display(Name = "کد معاینه فنی")]
        public decimal TechnicalCheckupNumber { get; set; }

        [Display(Name = "شروع معاینه فنی")]
        [TechnicalCheckupStartDateAttribute]
        public string TechnicalCheckupStartDate { get; set; }

        [Display(Name = "گروه خودرو")]
        [Required(ErrorMessage ="ورود گروه خوردو الزامی است")]
        public int? VehicleGroup { get; set; }

        [Display(Name = "پایان بیمه خودرو")]
        public string VehicleInsuranceEndDate { get; set; }

        [Display(Name = "کد بیمه")]
        public string VehicleInsuranceNum { get; set; }

        [Display(Name = "شروع بیمه خودرو")]
        [VehicleInsuranceStartDate]
        public string VehicleInsuranceStartDate { get; set; }

        [Display(Name = "نوع وسیله نقلیه")]
        [Required(ErrorMessage ="ورود نوع خودرو الزامی است")]
        public int? VehicleTypeId { get; set; }
    }
    public class PersonInsuranceStartDateAttribute : ValidationAttribute
    {
       
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            VehicleSave _vehicleSave = (VehicleSave)validationContext.ObjectInstance;

            DateTime StartDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(_vehicleSave.PersonInsuranceStartDate, 0);
            DateTime EndDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(_vehicleSave.PersonInsuranceEndDate, 0);

            if (StartDate >= EndDate)
            {
                return new ValidationResult("تاریخ شروع باید کوچکتر از تاریخ پایان باشد");
            }

            return ValidationResult.Success;
        }
    }
    public class TechnicalCheckupStartDateAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            VehicleSave _vehicleSave = (VehicleSave)validationContext.ObjectInstance;

            DateTime? StartDate = string.IsNullOrEmpty(_vehicleSave.TechnicalCheckupStartDate)==true?(DateTime?)null: Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(_vehicleSave.TechnicalCheckupStartDate, 0);
            DateTime? EndDate = string.IsNullOrEmpty(_vehicleSave.TechnicalCheckupEndDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(_vehicleSave.TechnicalCheckupEndDate, 0);

            if (StartDate!=null && EndDate!=null && StartDate >= EndDate)
            {
                return new ValidationResult("تاریخ شروع باید کوچکتر از تاریخ پایان باشد");
            }

            return ValidationResult.Success;
        }
    }
    public class VehicleInsuranceStartDateAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            VehicleSave _vehicleSave = (VehicleSave)validationContext.ObjectInstance;

            DateTime? StartDate = string.IsNullOrEmpty(_vehicleSave.VehicleInsuranceStartDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(_vehicleSave.VehicleInsuranceStartDate, 0);
            DateTime? EndDate = string.IsNullOrEmpty(_vehicleSave.VehicleInsuranceEndDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(_vehicleSave.VehicleInsuranceEndDate, 0);

            if (StartDate != null && EndDate != null && StartDate >= EndDate)
            {
                return new ValidationResult("تاریخ شروع باید کوچکتر از تاریخ پایان باشد");
            }

            return ValidationResult.Success;
        }
    }
}