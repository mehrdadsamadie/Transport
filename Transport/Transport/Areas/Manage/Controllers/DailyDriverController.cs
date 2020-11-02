using Domain.Entity;
using Domain.Repository;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Transport.Areas.Manage.Models;

namespace Transport.Areas.Manage.Controllers
{


    public class DailyDriverController : Controller
    {
        DailyDriverRepository _dailyDriverRepository = new DailyDriverRepository();
        // GET: Manage/DailyDriver
        public ActionResult Index(string Date, int? DriverTypeId, int? StatusTypeId, int pageNumber = 1, int pageSize = 20)
        {
            DateTime? DateM = string.IsNullOrWhiteSpace(Date) ? (DateTime?)null :
            Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(Date, 0);
            PersonStatusTypeRepository _personStatusTypeRepository = new PersonStatusTypeRepository();
            DriverTypeRepository _driverTypeRepository = new DriverTypeRepository();
            var StatusTypes = _personStatusTypeRepository.All().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.StatusName }).ToList();
            var DriverTypes = _driverTypeRepository.All().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            StatusTypes.Insert(0, new SelectListItem() { Value = null, Text = null });
            DriverTypes.Insert(0, new SelectListItem() { Value = null, Text = null });
            ViewBag.StatusTypes = StatusTypes;
            ViewBag.DriverTypes = DriverTypes;
            var _DailyDrivers = _dailyDriverRepository.All(DateM, DriverTypeId, StatusTypeId).ToList().Select(x => new DailyDriverViewModel
            {
                Id = x.Id,
                DriverId = x.DriverId,
                DriverName =x.Driver.Personnel.FullName,
                Date = Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.Date, false),
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                DelayHours = x.DelayHours,
                RushHours = x.RushHours,
                AbsentHours = x.AbsentHours,
                DeductionsTotal = x.DeductionsTotal,
                StatusTypeId = x.StatusTypeId,
                StatusTypeName = x.PersonStatusType.StatusName,
            DriverTypeId = x.DriverTypeId,
                DriverTypeName = x.DriverTypeId == null ? null : x.DriverType.Name
        });
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("DailyDriverList", _DailyDrivers.ToPagedList(pageNumber, pageSize))
                : View(_DailyDrivers.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create(int? id)
        {
            PersonStatusTypeRepository _personStatusTypeRepository = new PersonStatusTypeRepository();
            DriverTypeRepository _driverTypeRepository = new DriverTypeRepository();
            var StatysTypes = _personStatusTypeRepository.All().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.StatusName });
            var DriverTypes = _driverTypeRepository.All().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            DriverTypes.Insert(0,new SelectListItem() { Value = null, Text = null });
            ViewBag.StatusTypes = StatysTypes;
            ViewBag.DriverTypes = DriverTypes;

            var _dailyDriverView = new DailyDriverViewModel();
            if (id != null)
            {
                var _dailyDriver = _dailyDriverRepository.GetById(id.Value);
                if (_dailyDriver != null)
                {
                    _dailyDriverView.Id = _dailyDriver.Id;
                    _dailyDriverView.DriverId = _dailyDriver.DriverId;
                    _dailyDriverView.DriverName = _dailyDriver.Driver.Personnel.FullName;
                    _dailyDriverView.Date = Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_dailyDriver.Date, false);
                    _dailyDriverView.StartTime = _dailyDriver.StartTime;
                    _dailyDriverView.EndTime = _dailyDriver.EndTime;
                    _dailyDriverView.DelayHours = _dailyDriver.DelayHours;
                    _dailyDriverView.RushHours = _dailyDriver.RushHours;
                    _dailyDriverView.AbsentHours = _dailyDriver.AbsentHours;
                    _dailyDriverView.DeductionsTotal = _dailyDriver.DeductionsTotal;
                    _dailyDriverView.StatusTypeId = _dailyDriver.StatusTypeId;
                    _dailyDriverView.StatusTypeName = _dailyDriver.PersonStatusType.StatusName;
                    _dailyDriverView.DriverTypeId = _dailyDriver.DriverTypeId;
                    _dailyDriverView.DriverTypeName = _dailyDriver.DriverTypeId == null ? null : _dailyDriver.DriverType.Name;
                }
            }
            return View(_dailyDriverView);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(DailyDriverViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id != 0)
                {
                    var _dailyDriver = this._dailyDriverRepository.GetById(model.Id);
                    
                    _dailyDriver.Date = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.Date, 0);
                    _dailyDriver.StartTime = model.StartTime;
                    _dailyDriver.EndTime = model.EndTime;
                    _dailyDriver.DelayHours = model.DelayHours;
                    _dailyDriver.RushHours = model.RushHours;
                    _dailyDriver.AbsentHours = model.AbsentHours;
                    _dailyDriver.DeductionsTotal = model.DeductionsTotal;

                    _dailyDriver.DriverId = model.DriverId;
                    _dailyDriver.StatusTypeId = model.StatusTypeId;
                    _dailyDriver.DriverTypeId = model.DriverTypeId;


                    _dailyDriverRepository.Edit(_dailyDriver);
                    return RedirectToAction("Index");
                }
                else
                {
                    var _dailyDriver = new DailyDriver()
                    {
                        Date = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.Date, 0),
                        StartTime = model.StartTime,
                        EndTime = model.EndTime,
                        DelayHours = model.DelayHours,
                        RushHours = model.RushHours,
                        AbsentHours = model.AbsentHours,
                        DeductionsTotal = model.DeductionsTotal,
                        DriverId = model.DriverId,
                        StatusTypeId = model.StatusTypeId,
                        DriverTypeId = model.DriverTypeId
                    };

                    this._dailyDriverRepository.Add(_dailyDriver);
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var _dailyTime = _dailyDriverRepository.GetById(id);
            _dailyTime.IsDelete = true;
            _dailyDriverRepository.Edit(_dailyTime);
            return RedirectToAction("Index");
        }

    }
}