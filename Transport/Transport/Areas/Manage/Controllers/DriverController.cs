using Domain.Entity;
using Domain.Repository;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Transport.Areas.Manage.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace Transport.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin,TranasportManager")]
    public class DriverController : Controller
    {
        private DriverRepository DriverRepository = new DriverRepository();
        private RDriverDriverTypeRepository RDriverDriverTypeRepository = new RDriverDriverTypeRepository();
        private PersonnelRepository PersonnelRepository = new PersonnelRepository();
        private DriverTypeRepository DriverTypeRepository = new DriverTypeRepository();
        private DriverHistoryRepository DriverHistoryRepository = new DriverHistoryRepository();
        public DriverController()
        {
        }
        public DriverController(ApplicationUserManager userManager)
        {
            UserManager = userManager;

        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Manage/Driver
        public ActionResult Index(string searchString, int pageNumber = 1, int pageSize = 50, bool? isActive = true, bool? isDelete = false, bool? isBlack = false)
        {
            var _drivers = DriverRepository.GetAll(searchString, isActive, isDelete, isBlack).OrderBy(x=>x.Personnel.LastName).ToList().Select(x => new DriverView
            {
                DriverId = x.Id,
                FullName = x.Personnel.FullName,
                PersonelCode = x.Personnel.PersonnelCode,
                IsActive = x.IsActive,
                IsBlack = x.IsBlack,
                IsDelete = x.IsDelete,
                DriverType = RDriverDriverTypeRepository.GetByDriverId(x.Id, true, true, false) == null ? null : RDriverDriverTypeRepository.GetByDriverId(x.Id, true, true, false).DriverType.Name,
            });
            ViewBag.DriverTypes = DriverTypeRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("DriverList", _drivers.ToPagedList(pageNumber, pageSize))
            : View(_drivers.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.PersonnelList = PersonnelRepository.All(null, false, false)
                .Where(x => !x.Drivers.Any(y => y.PersonnelId == x.Id))
                .Select(x => new SelectListItem { Text = x.FullName, Value = x.Id.ToString() });
            return View(new DriverSave());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DriverSave model)
        {
            if (ModelState.IsValid)
            {
                var _driver = new Driver();
                var _personnel = PersonnelRepository.GetById(model.PersonelId);
                if (_personnel.Drivers.FirstOrDefault() == null)
                {
                    if (_personnel != null)
                    {
                        try
                        {
                            _driver.PersonnelId = _personnel.Id;
                            _driver.TimeCreated = DateTime.Now;
                            _driver.UserCreated = User.Identity.GetUserId();
                            _driver.IsActive = true;
                            _driver.IsDelete = false;
                            _driver.IsBlack = false;
                            DriverRepository.Add(_driver);

                            return RedirectToAction("Index");
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError("", "خطا در ذخیره سازی");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "چنین کاربری یافت نشد");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "چنین راننده ای یافت شد");
                }
            }
            ViewBag.PersonnelList = PersonnelRepository.All(null, false, false)
                 .Where(x => !x.Drivers.Any(y => y.PersonnelId == x.Id))
                 .Select(x => new SelectListItem { Text = x.FullName, Value = x.Id.ToString() });
            return View(model);


        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var _driver = DriverRepository.GetById(id);
            if (_driver != null)
            {
                ViewBag.DriverTypes = DriverTypeRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
                var _driverEdit = new DriverDetail();
                _driverEdit.FullName = _driver.Personnel.FullName;
                _driverEdit.PersonnelCode = _driver.Personnel.PersonnelCode;
                _driverEdit.Id = _driver.Id;
                _driverEdit.NewDriverType = new DriverTypeView()
                {
                    DriverTypeId = 1,
                    DriverId = _driver.Id,
                    StartDate = Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(DateTime.Now, false, null),
                    EndDate = Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(DateTime.Now.AddYears(1), false, null),
                    IsActive = true
                };
                _driverEdit.NewDriverHistory = new DriverHistorySave()
                {
                    DriverId = _driver.Id,
                    Date = Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(DateTime.Now, false, null),
                };
                _driverEdit.DriverTypes = _driver.RDriverDriverTypes.Where(x => x.IsDelete == false).OrderByDescending(x => x.EndDate).Select(x => new DriverTypeView
                {
                    Id = x.Id,
                    DriverId = x.DriverId,
                    DriverTypeId = x.DriverTypeId,
                    StartDate = Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.StartDate, false, null),
                    EndDate = x.EndDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.EndDate.Value, false, null),
                    IsActive = x.IsActive
                }).ToList();
                _driverEdit.DriverHistories = _driver.DriverHistories.Where(x => x.IsDelete == false).OrderByDescending(x => x.Date).ToList().Select(x => new DriverHistorySave
                {
                    Date = x.Date == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.Date.Value, false, null),
                    Description = x.Description,
                    DriverId = x.DriverId,
                    Id = x.Id,
                }).ToList();
                return View(_driverEdit);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public JsonResult AddDriverType(DriverTypeView model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id == 0)
                    {
                        var _RdriverType = new RDriverDriverType()
                        {
                            DriverId = model.DriverId,
                            DriverTypeId = model.DriverTypeId,
                            IsActive = true,
                            StartDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.StartDate, 0),
                            EndDate = string.IsNullOrEmpty(model.EndDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.EndDate, 0),
                            IsDelete = false,
                            TimeCreated = DateTime.Now,
                            UserCreated = User.Identity.GetUserId(),

                        };
                        RDriverDriverTypeRepository.Add(_RdriverType);
                        return Json(new
                        {
                            success = true,
                        });
                    }
                    else
                    {
                        var _RdriverType = RDriverDriverTypeRepository.GetById(model.Id);
                        if (_RdriverType != null)
                        {
                            _RdriverType.IsActive = model.IsActive;
                            _RdriverType.DriverTypeId = model.DriverTypeId;
                            _RdriverType.TimeEdited = DateTime.Now;
                            _RdriverType.UserEdited = User.Identity.GetUserId();
                            _RdriverType.StartDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.StartDate, 0);
                            _RdriverType.EndDate = string.IsNullOrEmpty(model.EndDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.EndDate, 0);
                            RDriverDriverTypeRepository.Edit(_RdriverType);
                            return Json(new
                            {
                                success = true,
                            });
                        }
                    }
                }

                catch (Exception e)
                {
                    ModelState.AddModelError("", "خطای در ذخیره سازی");
                }
            }

            return Json(new
            {
                success = false,
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                     .Select(m => m.ErrorMessage).ToArray(),
            });
        }


        public ActionResult DeleteRDriverType(int id)
        {
            var _rDriverType = RDriverDriverTypeRepository.GetById(id);
            if (_rDriverType != null)
            {
                _rDriverType.IsDelete = true;
                _rDriverType.UserEdited = User.Identity.GetUserId();
                _rDriverType.TimeEdited = DateTime.Now;
                RDriverDriverTypeRepository.Edit(_rDriverType);
                return RedirectToAction("Edit", new { id = _rDriverType.DriverId });
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        public ActionResult Delete(int id)
        {
            var _driver = DriverRepository.GetById(id);
            if (_driver != null)
            {
                _driver.IsDelete = !_driver.IsDelete;
                _driver.UserEdited = User.Identity.GetUserId();
                _driver.TimeEdited = DateTime.Now;
                DriverRepository.Edit(_driver);
            }
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public JsonResult AutoPersonel(string searchString, bool? isActive, bool? isDelete, bool? isBlock)
        {
            var _personels = DriverRepository.GetAll(searchString, isActive, isDelete, isBlock).ToList().Select(x => new { x.Personnel.FullName, x.Id ,Text= x.Personnel.FullName + " - "+x.Personnel.PersonnelCode });
            return Json(_personels, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult GetPersonel(int Id)
        {
            var _driver = DriverRepository.GetById(Id);
            return Json(new { _driver.Personnel.FullName, _driver.Id }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult GetDriverById(int id)
        {
            var _deriver = DriverRepository.GetById(id);
            if (_deriver != null)
            {
                return Json(_deriver.Personnel.FullName, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult AddDriverHistory(DriverHistorySave model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id == 0)
                    {
                        var _driverHistory = new DriverHistory();
                        _driverHistory.Date = string.IsNullOrEmpty(model.Date) == true ? DateTime.Now : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.Date, 0);
                        _driverHistory.Description = model.Description;
                        _driverHistory.DriverId = model.DriverId;
                        _driverHistory.IsDelete = false;
                        _driverHistory.TimeCreated = DateTime.Now;
                        _driverHistory.UserCreated = User.Identity.GetUserId();
                        DriverHistoryRepository.Add(_driverHistory);
                    }
                    else
                    {
                        var _driverHistory = DriverHistoryRepository.GetById(model.Id);
                        {
                            _driverHistory.Date = string.IsNullOrEmpty(model.Date) == true ? DateTime.Now : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.Date, 0);
                            _driverHistory.Description = model.Description;
                            _driverHistory.TimeEdited = DateTime.Now;
                            _driverHistory.UserEdited = User.Identity.GetUserId();
                            DriverHistoryRepository.Edit(_driverHistory);
                        }
                    }
                    return Json(new
                    {
                        success = true,
                    });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "خطای در ذخیره سازی");
                }
            }
            return Json(new
            {
                success = false,
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                      .Select(m => m.ErrorMessage).ToArray(),
            });
        }
        public ActionResult DeleteDriverHistory(int id)
        {
            var _driverHistory = DriverHistoryRepository.GetById(id);
            {
                _driverHistory.IsDelete = (!_driverHistory.IsDelete);
                _driverHistory.TimeEdited = DateTime.Now;
                _driverHistory.UserEdited = User.Identity.GetUserId();
                DriverHistoryRepository.Edit(_driverHistory);
            }
            return RedirectToAction("Edit", new { id = _driverHistory.DriverId });
        }
    }
}