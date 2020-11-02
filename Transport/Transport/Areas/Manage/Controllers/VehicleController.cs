using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entity;
using Domain.Repository;
using Transport.Areas.Manage.Models;
using PagedList;
using System.Transactions;
using Microsoft.AspNet.Identity;

namespace Transport.Areas.Manage.Controllers
{
    public class VehicleController : Controller
    {
        // GET: Manage/Vehicle
        private VehicleRepository VehicleRepository = new VehicleRepository();
        private VehicleDriverRepository VehicleDriverRepository = new VehicleDriverRepository();
        private VehicleTypeRepository VehicleTypeRepository = new VehicleTypeRepository();
        private DriverRepository DriverRepository = new DriverRepository();
        private LicensePlateTypeRepository LicensePlateTypeRepository = new LicensePlateTypeRepository();
        public ActionResult Index(int? driverId, bool? isActive, bool? isDelete, int pageNumber = 1, int pageSize = 20)
        {
            var _vehicles = VehicleRepository.GetAll(driverId, isActive, isDelete).ToList().Select(x => new VehicleViewIndex
            {
                Id = x.Id,
                Model = x.Factory + '-' + x.ModelName + '-' + (Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.ProductYear, false, null)),
                LicensePlateNum = x.LicensePlateNum,
                VehicleType = x.VehicleType.Name,
                DriverId = VehicleDriverRepository.GetDriverOfVehicle(x.Id) == null ? (int?)null : VehicleDriverRepository.GetDriverOfVehicle(x.Id).Id,
                FullName = VehicleDriverRepository.GetDriverOfVehicle(x.Id) == null ? null : VehicleDriverRepository.GetDriverOfVehicle(x.Id).Personnel.FullName,
            });
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("VehiclesList", _vehicles.ToPagedList(pageNumber, pageSize))
            : View(_vehicles.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create(int? id, int? driverId)
        {
            var _vehicleSave = new VehicleSave();
            if (id != null)
            {
                var _vehicle = VehicleRepository.GetById(id.Value);
                if (_vehicle != null)
                {
                    _vehicleSave.Id = _vehicle.Id;
                    _vehicleSave.VehicleTypeId = _vehicle.VehicleTypeId;
                    _vehicleSave.Factory = _vehicle.Factory;
                    _vehicleSave.ModelName = _vehicle.ModelName;
                    _vehicleSave.ProductYear = _vehicle.ProductYear == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_vehicle.ProductYear, false, null);
                    _vehicleSave.Capacity = _vehicle.Capacity;
                    _vehicleSave.ChassisNum = _vehicle.ChassisNum;
                    _vehicleSave.Color = _vehicle.Color;
                    _vehicleSave.VehicleGroup = _vehicle.VehicleGroup;
                    _vehicleSave.MotorNum = _vehicle.MotorNum;
                    _vehicleSave.LicensePlateNum = _vehicle.LicensePlateNum;
                    _vehicleSave.LicensePlateTypeId = _vehicle.LicensePlateTypeId;
                    _vehicleSave.HasTechnicalCheckup = _vehicle.HasTechnicalCheckup;
                    _vehicleSave.TechnicalCheckupNumber = _vehicle.TechnicalCheckupNumber;
                    _vehicleSave.TechnicalCheckupStartDate = _vehicle.TechnicalCheckupStartDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_vehicle.TechnicalCheckupStartDate.Value, false, null);
                    _vehicleSave.TechnicalCheckupEndDate = _vehicle.TechnicalCheckupEndDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_vehicle.TechnicalCheckupEndDate.Value, false, null);
                    _vehicleSave.PersonInsuranceStartDate = _vehicle.PersonInsuranceStartDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_vehicle.PersonInsuranceStartDate.Value, false, null);
                    _vehicleSave.PersonInsuranceNum = _vehicle.PersonInsuranceNum;
                    _vehicleSave.PersonInsuranceEndDate = _vehicle.PersonInsuranceEndDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_vehicle.PersonInsuranceEndDate.Value, false, null);
                    _vehicleSave.VehicleInsuranceNum = _vehicle.VehicleInsuranceNum;
                    _vehicleSave.VehicleInsuranceStartDate = _vehicle.VehicleInsuranceStartDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_vehicle.VehicleInsuranceStartDate.Value, false, null);
                    _vehicleSave.VehicleInsuranceEndDate = _vehicle.VehicleInsuranceEndDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_vehicle.VehicleInsuranceEndDate.Value, false, null);
                    _vehicleSave.IsActive = _vehicle.IsActive;
                    _vehicleSave.DriverId = VehicleDriverRepository.GetDriverOfVehicle(_vehicle.Id) == null ? (int?)null : VehicleDriverRepository.GetDriverOfVehicle(_vehicle.Id).Id;
                    _vehicleSave.DriverFullName = VehicleDriverRepository.GetDriverOfVehicle(_vehicle.Id) == null ? null : VehicleDriverRepository.GetDriverOfVehicle(_vehicle.Id).Personnel.FullName;

                }
            }
            if (driverId != null)
            {
                var _driver = DriverRepository.GetById(driverId.Value);
                if (_driver != null)
                {
                    _vehicleSave.DriverFullName = _driver.Personnel.FullName;
                    _vehicleSave.DriverId = _driver.Id;
                }
            }
            ViewBag.VehicleGroups = Enum.GetValues(typeof(VehicleGroupView)).Cast<VehicleGroupView>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            });
            ViewBag.LicensePlateTypes = LicensePlateTypeRepository.All().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.VehicleTypes = VehicleTypeRepository.All().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            return View(_vehicleSave);
        }
        [HttpPost]
        public ActionResult Create(VehicleSave model)
        {
            if (ModelState.IsValid == true)
            {
                if (model.Id == 0)
                {
                    using (var tx = new TransactionScope())
                    {
                        try
                        {
                            var _vehicle = new Vehicle()
                            {
                                Capacity = model.Capacity,
                                ChassisNum = model.ChassisNum,
                                Color = model.Color,
                                Factory = model.Factory,
                                HasTechnicalCheckup = model.HasTechnicalCheckup,
                                IsActive = true,
                                IsDelete = true,
                                LicensePlateNum = model.LicensePlateNum,
                                ModelName = model.ModelName,
                                MotorNum = model.MotorNum,
                                LicensePlateTypeId = model.LicensePlateTypeId,
                                PersonInsuranceEndDate = string.IsNullOrEmpty(model.PersonInsuranceEndDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.PersonInsuranceEndDate, 0),
                                PersonInsuranceNum = model.PersonInsuranceNum,
                                PersonInsuranceStartDate = string.IsNullOrEmpty(model.PersonInsuranceStartDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.PersonInsuranceStartDate, 0),
                                ProductYear = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.ProductYear, 0),
                                TechnicalCheckupEndDate = string.IsNullOrEmpty(model.TechnicalCheckupEndDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.TechnicalCheckupEndDate, 0),
                                TechnicalCheckupNumber = model.TechnicalCheckupNumber,
                                TechnicalCheckupStartDate = string.IsNullOrEmpty(model.TechnicalCheckupStartDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.TechnicalCheckupStartDate, 0),
                                TimeCreated = DateTime.Now,
                                UserCreated = User.Identity.GetUserId(),
                                VehicleGroup = model.VehicleGroup,
                                VehicleInsuranceEndDate = string.IsNullOrEmpty(model.VehicleInsuranceEndDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.VehicleInsuranceEndDate, 0),
                                VehicleInsuranceNum = model.VehicleInsuranceNum,
                                VehicleInsuranceStartDate = string.IsNullOrEmpty(model.VehicleInsuranceEndDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.VehicleInsuranceEndDate, 0),
                                VehicleTypeId = model.VehicleTypeId,

                            };
                            _vehicle.Id = VehicleRepository.Add(_vehicle);
                            if (model.DriverId != null)
                            {
                                var _vehicleDriver = new VehicleDriver()
                                {
                                    DriverId = model.DriverId.Value,
                                    IsActive = true,
                                    IsDelete = false,
                                    UserCreated = User.Identity.GetUserId(),
                                    TimeCreated = DateTime.Now,
                                    VehicleId = _vehicle.Id,
                                };
                                VehicleDriverRepository.Add(_vehicleDriver);
                            }
                            tx.Complete();
                            return RedirectToAction("Index");
                        }
                        catch (Exception e)
                        {
                            tx.Dispose();
                            ModelState.AddModelError("", "خطا در ذخیره سازی");
                        }

                    }
                }
                else
                {
                    using (var tx = new TransactionScope())
                    {
                        try
                        {
                            var _vehicle = VehicleRepository.GetById(model.Id);
                            _vehicle.Capacity = model.Capacity;
                            _vehicle.ChassisNum = model.ChassisNum;
                            _vehicle.Color = model.Color;
                            _vehicle.Factory = model.Factory;
                            _vehicle.HasTechnicalCheckup = model.HasTechnicalCheckup;
                            _vehicle.LicensePlateNum = model.LicensePlateNum;
                            _vehicle.LicensePlateTypeId = model.LicensePlateTypeId;
                            _vehicle.ModelName = model.ModelName;
                            _vehicle.MotorNum = model.MotorNum;
                            _vehicle.PersonInsuranceEndDate = string.IsNullOrEmpty(model.PersonInsuranceEndDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.PersonInsuranceEndDate, 0);
                            _vehicle.PersonInsuranceNum = model.PersonInsuranceNum;
                            _vehicle.PersonInsuranceStartDate= string.IsNullOrEmpty(model.PersonInsuranceStartDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.PersonInsuranceStartDate, 0);
                            _vehicle.ProductYear =Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.PersonInsuranceStartDate, 0);
                            _vehicle.TechnicalCheckupEndDate = string.IsNullOrEmpty(model.TechnicalCheckupEndDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.TechnicalCheckupEndDate, 0);
                            _vehicle.TechnicalCheckupNumber = model.TechnicalCheckupNumber;
                            _vehicle.TechnicalCheckupStartDate= string.IsNullOrEmpty(model.TechnicalCheckupStartDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.TechnicalCheckupStartDate, 0);
                            _vehicle.TimeEdited = DateTime.Now;
                            _vehicle.UserEdited = User.Identity.GetUserId();
                            _vehicle.VehicleGroup = model.VehicleGroup;
                            _vehicle.VehicleInsuranceEndDate= string.IsNullOrEmpty(model.VehicleInsuranceEndDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.VehicleInsuranceEndDate, 0);
                            _vehicle.VehicleInsuranceNum = model.VehicleInsuranceNum;
                            _vehicle.VehicleInsuranceStartDate = string.IsNullOrEmpty(model.VehicleInsuranceStartDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.VehicleInsuranceStartDate, 0);
                            _vehicle.VehicleTypeId = model.VehicleTypeId;
                            _vehicle.IsActive = model.IsActive;
                            VehicleRepository.Edit(_vehicle);
                            if (model.DriverId != null)
                            {
                                var _driver = VehicleDriverRepository.GetDriverOfVehicle(model.Id);
                                if (_driver == null)
                                {
                                    
                                    var _vehicleDrivernew = new VehicleDriver()
                                    {
                                        DriverId = model.DriverId.Value,
                                        IsActive = true,
                                        IsDelete = false,
                                        UserCreated = User.Identity.GetUserId(),
                                        TimeCreated = DateTime.Now,
                                        VehicleId = _vehicle.Id,
                                    };
                                    VehicleDriverRepository.Add(_vehicleDrivernew);
                                }
                                else if(model.Id != _driver.Id)
                                {
                                    var _vehicleDriveredit = VehicleDriverRepository.GetVehicleDriver(model.Id);
                                    _vehicleDriveredit.DriverId = model.DriverId.Value;
                                    _vehicleDriveredit.UserEdited = User.Identity.GetUserId();
                                    _vehicleDriveredit.TimeEdited = DateTime.Now;
                                    VehicleDriverRepository.Edit(_vehicleDriveredit);
                                }
                            }
                            tx.Complete();
                            return RedirectToAction("Index");
                        }
                        catch (Exception e)
                        {
                            tx.Dispose();
                            ModelState.AddModelError("", "خطا در ذخیره سازی");
                        }
                    }
                }
            }
            ViewBag.VehicleGroups = Enum.GetValues(typeof(VehicleGroupView)).Cast<VehicleGroupView>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            });
            ViewBag.LicensePlateTypes = LicensePlateTypeRepository.All().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.VehicleTypes = VehicleTypeRepository.All().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var _vehicle = VehicleRepository.GetById(id);
            _vehicle.IsDelete = !_vehicle.IsDelete;
            _vehicle.UserEdited = User.Identity.GetUserId();
            _vehicle.TimeEdited = DateTime.Now;
            VehicleRepository.Edit(_vehicle);
            return RedirectToAction("Index");
        }
    }
}