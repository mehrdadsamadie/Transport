using Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Transport.Areas.Manage.Models
{
    public class PersonnelView
    {
        public string BirthDate { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string EmergencyPhone { get; set; }
        public string FullName { get; set; }
        public bool Gender { get; set; }
        public int Id { get; set; }
        public string Imag { get; set; }
        public bool IsArchive { get; set; }
        public bool IsDelete { get; set; }
        public string Mobile { get; set; }
        public string NationalCode { get; set; }
        public string PersonnelCode { get; set; }
        public string UserId { get;  set; }
    }
    public class PersonnelSave
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Display(Name = "کارخانه", Order = 10)]
        public int? FactoryId { get; set; }

        [Display(Name = "واحد", Order = 10)]

        public int? FactoryUnitId { get; set; }

        [Required(ErrorMessage = "ورود نام الزامی است .")]
        [StringLength(100)]
        [Display(Name = "نام", Order = 1)]
        public string Name { get; set; }

        [Required(ErrorMessage = "ورود نام خانوادگی الزامی است .")]
        [StringLength(100)]
        [Display(Name = "نام خانوادگی", Order = 3)]
        public string LastName { get; set; }

        [Display(Name = "جنسیت", Order = 10)]
        [Required(ErrorMessage = "ورود جنسیت الزامی است")]
        public bool Gender { get; set; }

        [Display(Name = "ایا حساب کاربری دارد", Order = 10)]

        public bool IsUserId { get; set; }

        [StringLength(50)]
        [Display(Name = "کد پرسنلی", Order = 6)]
        [UserCreate(ErrorMessage = "ورود کد پرسنلی برای ساخت نام کاربری الزامی است")]
        public string PersonnelCode { get; set; }

        [StringLength(10)]
        [Display(Name = "کد ملی", Order = 5)]
        public string NationalCode { get; set; }

        [Display(Name = "تاریخ تولد", Order = 7)]
        public string PersianBirthDate { get; set; }

        [StringLength(11)]
        [Display(Name = "تلفن ثابت", Order = 8)]

        public string CellPhone { get; set; }

        [StringLength(11)]
        [Display(Name = "تلفن همراه", Order = 9)]
        [UserCreate(ErrorMessage = "ورود تلفن همراه برای ساخت نام کاربری الزامی است")]
        public string Mobile { get; set; }

        [StringLength(12)]
        [Display(Name = "تلفن ضروری", Order = 10)]

        public string EmergencyPhone { get; set; }
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int? AddressId { get; set; }

        [StringLength(100,ErrorMessage ="طول ایمیل بیش از اندازه")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل اشتباه است")]
        [Display(Name = "ایمیل", Order = 10)]

        public string Email { get; set; }

        [Display(Name = "تصویر", Order = 10)]
        public string Image { get; set; }

        [StringLength(256)]
        [Display(Name = "عنوان مسیر", Order = 10)]

        public string Title { get; set; }

        [StringLength(20)]
        [Display(Name = "کشور", Order = 10)]
        [Adress(ErrorMessage ="ورود کشور الزامی است")]
        public string CountryName { get; set; }

        [Display(Name = "شهرستان", Order = 10)]
        [Adress(ErrorMessage = "ورود شهر الزامی است")]
        public int? EparchyId { get; set; }

        [Display(Name = "استان", Order = 10)]
        public int? StateId { get; set; }

        [StringLength(100)]
        [Display(Name = "شهر", Order = 10)]
        [Adress(ErrorMessage = "ورود شهر الزامی است")]
        public string CityName { get; set; }

        [StringLength(100)]
        [Display(Name = "منطقه", Order = 10)]

        public string RegionName { get; set; }

        [StringLength(256)]
        [Display(Name = "آدرس", Order = 10)]
        [Adress(ErrorMessage = "ورود آدرس الزامی است")]

        public string Address { get; set; }

        [Display(Name = "کدپستی", Order = 10)]
        [StringLength(10)]
        public string PostalCode { get; set; }
        public string UserId { get; internal set; }
    }

    public class FactoryView
    {
        public string Value { get;  set; }
        public string Text { get;  set; }
    }
    public class FactoryUnitView
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class EparchyView
    {
        public string Value { get;  set; }
        public string Text { get;  set; }
    }

    public class StateView
    {
        public string Value { get;  set; }
        public string Text { get; set; }
    }

    public class PersonnelDetail
    {
    }

    public class AutoPersonnel
    {
        public string FullName { get; internal set; }
        public int Id { get; internal set; }
        public string Text { get; internal set; }
    }

    public class UserCreateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PersonnelSave PersonnelSave = (PersonnelSave)validationContext.ObjectInstance;

            if (PersonnelSave.IsUserId == true && value == null)
            {
                //return new ValidationResult("انتخاب دسته یا کالا الزامی میباشد");
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
    public class IDCardAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool result = true;
            if (!Regex.IsMatch(value.ToString(), @"^\d{10}$"))
                result = false;
            if (result == true)
            {
                var check = Convert.ToInt32(value.ToString().Substring(9, 1));
                var sum = Enumerable.Range(0, 9)
                    .Select(x => Convert.ToInt32(value.ToString().Substring(x, 1)) * (10 - x))
                    .Sum() % 11;

                result = sum < 2 && check == sum || sum >= 2 && check + sum == 11;
            }
            if (result == true)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }
        }
    }
    public class AdressAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PersonnelSave PersonnelSave = (PersonnelSave)validationContext.ObjectInstance;

            if ((PersonnelSave.AddressId != null 
                ||!string.IsNullOrEmpty(PersonnelSave.CountryName)
                ||!string.IsNullOrEmpty(PersonnelSave.CityName)
                ||!string.IsNullOrEmpty(PersonnelSave.Address)
                ||PersonnelSave.EparchyId!=null) && value==null)
            {
                //return new ValidationResult("انتخاب دسته یا کالا الزامی میباشد");
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}