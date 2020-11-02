using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Transport.Areas.Manage.Models
{
    public class ServiceIndexView
    {
        public ServiceIndexView()
        {
            Requests = new List<RequestViewInService>();
        }
        public string Beginning { get; set; }
        public string Date { get; set; }
        public string Destination { get; set; }
        public string DriverType { get; set; }
        public string FullName { get; set; }
        public int Id { get; set; }
        public bool? IsAcceptTranasportManager { get; internal set; }
        public bool IsLocal { get;  set; }
        public string Name { get; set; }
        public int? RequestId { get; set; }
        public string RequestName { get; internal set; }
        public string RequestUnit { get; internal set; }
        public int ServiceStatus { get;  set; }
        public List<RequestViewInService> Requests{ get; set; }
        public string StartTime { get; internal set; }
        public string EndTime { get; internal set; }
        public bool IsDelete { get; internal set; }
        public bool IsLock { get; internal set; }
    }
    public class RequestViewInService
    {
        public string Factory { get; internal set; }
        public string FullName { get; internal set; }
        public int Id { get; internal set; }
        public int? Number { get;  set; }
        public string Begining { get;  set; }
        public string Destination { get;  set; }
        public string Mobile { get; internal set; }
    }
    public class ServiceSaveView
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]

        [Display(Name = "نام راننده")]
        [Driver]
        public int? DriverId { get; set; }
        [Display(Name = "تاکسی تلفنی")]
        public int? TaxiCompanyId { get; set; }
        [Display(Name = "نام راننده تاکسی تلفنی")]
        public string DriverName { get; set; }
        [Display(Name = "تعداد همراهان")]
        public int? GussetNumber { get; set; } = 0;
        [Display(Name = "تاریخ سفر")]
        [Required(ErrorMessage = "ورود سفر الزامی است")]
        public string Date { get; set; }
        [Display(Name = "زمان شروع")]
        public string StartTime { get; set; }
        [Display(Name = "زمان پایان")]
        public string EndTime { get; set; }
        [Display(Name = "مسیر")]
        public int? PathId { get; set; }
        [Display(Name = "مبلغ سفر")]
        public decimal? DistancePrice { get; set; }
        [Display(Name = "درخواست کننده")]
        public int? RequestId { get; set; }

        [Display(Name = "مبدا")]
        public int? BiginningId { get; set; }

        [Display(Name = "مقصد")]
        public int? DestinationId { get; set; }

        public int? DailyDriverId { get; set; }

        [Display(Name = "نوع راننده")]
        public int? DriverTypeId { get; set; }

        [Display(Name = "مسافت دقیق ")]
        public decimal? RealDistance { get; set; }

        [Display(Name = "مسافت")]
        [Distance]
        public decimal? Distance { get; set; }

        [Display(Name = "تاخیر")]
        public string DelayTime { get; set; }

        public AddressSaveView Biginning { get; set; }
        public AddressSaveView Destination { get; set; }
        [Display(Name = "سفر داخل شهرک")]
        public bool IsLocal { get; set; } = true;
        [Required(ErrorMessage ="انتخاب وضعیت سفر الزامی است")]
        public int ServiceStatusId { get;  set; } = 1;
        public bool BackRequest { get; set; } = false;
        [Display(Name = "نوع سفر")]
        [Required(ErrorMessage = "انتخاب نوع سفر الزامی است")]
        public int? ServiceTypeId { get; set; }
        public string FullName { get;  set; }
        [Display(Name = "توضیحات")]
        public string Description { get;  set; }
    }

    public class AddressSaveView
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int id { get; set; }

        [StringLength(20)]
        [Display(Name = "کشور", Order = 10)]

        public string CountryName { get; set; } = "ایران";

        [Display(Name = "شهرستان", Order = 10)]
        [Required(ErrorMessage = "ورود شهرستان الزامی است")]
        public int? EparchyId { get; set; } = 640;

        [Display(Name = "استان", Order = 10)]
        public int? StateId { get; set; } = 5;

        [StringLength(100)]
        [Display(Name = "شهر", Order = 10)]
        public string CityName { get; set; }

        [StringLength(100)]
        [Display(Name = "منطقه", Order = 10)]

        public string RegionName { get; set; }

        [StringLength(256)]
        [Display(Name = "آدرس", Order = 10)]
        [AddressValid]
        public string Address { get; set; }

        [Display(Name = "کدپستی", Order = 10)]
        [StringLength(10)]
        public string PostalCode { get; set; }

    }
    public class DriverAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ServiceSaveView ServiceSaveView = (ServiceSaveView)validationContext.ObjectInstance;

            if (ServiceSaveView.DriverId == null && ServiceSaveView.TaxiCompanyId == null)
            {
                return new ValidationResult("ورود راننده الزامی است");
                // return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
    public class DistanceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ServiceSaveView ServiceSaveView = (ServiceSaveView)validationContext.ObjectInstance;

            if (ServiceSaveView.PathId == null && ServiceSaveView.Distance == null && ServiceSaveView.IsLocal==false)
            {
                return new ValidationResult("انتخاب مسیر یا ورود مسافت الزامی میباشد");
                // return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
    public class AddressValidAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            AddressSaveView AddressSaveView = (AddressSaveView)validationContext.ObjectInstance;

            if (AddressSaveView.EparchyId!=null || string.IsNullOrEmpty(AddressSaveView.Address) == false)
            {
                if(AddressSaveView.EparchyId == null || string.IsNullOrEmpty(AddressSaveView.Address) == true)
                return new ValidationResult("ورود شهر و شهرستان و آدرس  الزامی است");
            }

            return ValidationResult.Success;
        }
    }
}