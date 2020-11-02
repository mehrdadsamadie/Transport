using Domain.Entity;
using Domain.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Transport.Areas.Manage.Models;
using Transport.Models;

namespace Transport.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PersonnelController : Controller
    {
        private PersonnelRepository PersonnelRepository = new PersonnelRepository();
        private StateRepository StateRepository = new StateRepository();
        private EparchyRepository EparchyRepository = new EparchyRepository();
        private FactoryRepository FactoryRepository = new FactoryRepository();
        private FactoryUnitRepository FactoryUnitRepository = new FactoryUnitRepository();
        private AddressRepository AddressRepository = new AddressRepository();
        private ManagerPersonelRepository ManagerPersonelRepository = new ManagerPersonelRepository();
        public PersonnelController()
        {

        }
        public PersonnelController(ApplicationUserManager userManager)
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
        // GET: Personnel
        public ActionResult Index(string searchString, bool? isDelete = false, bool? isActive = true, int pageNumber = 1, int pageSize = 50)
        {
            var _personels = PersonnelRepository.All(searchString, isDelete, !isActive).ToList().Select(x => new PersonnelView
            {
                BirthDate = x.BirthDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.BirthDate.Value, false, null),
                CellPhone = x.CellPhone,
                Email = x.Email,
                EmergencyPhone = x.EmergencyPhone,
                Gender = x.Gender,
                Id = x.Id,
                Imag = x.Image,
                IsArchive = x.IsArchive,
                IsDelete = x.IsDelete,
                Mobile = x.Mobile,
                FullName = x.FullName,
                NationalCode = x.NationalCode,
                PersonnelCode = x.PersonnelCode,
            }).ToList();
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("PersonnelList", _personels.ToPagedList(pageNumber, pageSize))
            : View(_personels.ToPagedList(pageNumber, pageSize));
        }
        public JsonResult List(string searchString, bool? isDelete = false, bool? isActive = true, Enums.OrderEnumPersonnels order = Enums.OrderEnumPersonnels.LastName, bool isAsc = true, int pageNumber = 0, int pageSize = 50)
        {
            var _totalCount = PersonnelRepository.All(searchString, isDelete, !isActive, order, isAsc).Count();
            var _personels = PersonnelRepository.All(searchString, isDelete, !isActive, order, isAsc).Skip(pageNumber * pageSize).Take(pageSize).ToList().Select(x => new PersonnelView
            {
                BirthDate = x.BirthDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.BirthDate.Value, false, null),
                CellPhone = x.CellPhone,
                Email = x.Email,
                EmergencyPhone = x.EmergencyPhone,
                Gender = x.Gender,
                Id = x.Id,
                Imag = x.Image,
                IsArchive = x.IsArchive,
                IsDelete = x.IsDelete,
                Mobile = x.Mobile,
                FullName = x.FullName,
                NationalCode = x.NationalCode,
                PersonnelCode = x.PersonnelCode,
            }).ToList();
            return Json(new { Personels = _personels, TotalCount = _totalCount });
        }
        public ActionResult Create(int? id)
        {
            var _personnelView = new PersonnelSave();
            ViewBag.Factories = FactoryRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.States = StateRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Eparchies = EparchyRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.FactoryUnits = FactoryUnitRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            if (id != null)
            {
                var _personnel = PersonnelRepository.GetById(id.Value);
                if (_personnel != null)
                {
                    _personnelView.CellPhone = _personnel.CellPhone;
                    _personnelView.Email = _personnel.Email;
                    _personnelView.EmergencyPhone = _personnel.EmergencyPhone;
                    _personnelView.FactoryUnitId = _personnel.FactoryUnitId;
                    _personnelView.FactoryId = _personnel.FactoryUnit.FactoryId;
                    _personnelView.Gender = _personnel.Gender;
                    _personnelView.Id = _personnel.Id;
                    _personnelView.Image = _personnel.Image;
                    _personnelView.IsUserId = UserManager.Users.FirstOrDefault(x => x.PersonnelId == _personnel.Id) == null ? false : true;
                    _personnelView.LastName = _personnel.LastName;
                    _personnelView.Mobile = _personnel.Mobile;
                    _personnelView.Name = _personnel.Name;
                    _personnelView.NationalCode = _personnel.NationalCode;
                    _personnelView.PersianBirthDate = _personnel.BirthDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_personnel.BirthDate.Value, false, null);
                    _personnelView.PersonnelCode = _personnel.PersonnelCode;
                    _personnelView.UserId = _personnel.PersonnelCode == null ? null : (UserManager.FindByName(_personnel.PersonnelCode) == null ? null : UserManager.FindByName(_personnel.PersonnelCode).Id);
                    ////////////////////////////////////////////////////////////////////
                    if (_personnel.AddressId != null)
                    {
                        var _address = AddressRepository.GetById(_personnel.AddressId.Value);
                        if (_address != null)
                        {
                            _personnelView.AddressId = _personnel.AddressId;
                            _personnelView.CountryName = _address.CountryName;
                            _personnelView.CityName = _address.CityName;
                            _personnelView.PostalCode = _address.PostalCode;
                            _personnelView.RegionName = _address.RegionName;
                            _personnelView.EparchyId = _address.EparchyId;
                            _personnelView.StateId = _address.Eparchy.StateId;
                            _personnelView.Address = _address.Address1;
                        }
                    }
                }
            }
            return View(_personnelView);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(PersonnelSave model)
        {

            if (ModelState.IsValid)
            {
                if (model.Id != 0)
                {

                    try
                    {
                        var _personel = PersonnelRepository.GetById(model.Id);
                        _personel.BirthDate = string.IsNullOrEmpty(model.PersianBirthDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.PersianBirthDate, 0);
                        _personel.CellPhone = model.CellPhone;
                        _personel.Email = model.Email;
                        _personel.EmergencyPhone = model.EmergencyPhone;
                        _personel.FactoryUnitId = model.FactoryUnitId;
                        _personel.Gender = model.Gender;
                        _personel.Image = model.Image;
                        _personel.IsArchive = _personel.IsArchive;
                        _personel.IsDelete = _personel.IsDelete;
                        _personel.LastName = model.LastName;
                        _personel.Mobile = model.Mobile;
                        _personel.Name = model.Name;
                        _personel.NationalCode = model.NationalCode;
                        _personel.PersonnelCode = model.PersonnelCode;
                        var _user = UserManager.Users.FirstOrDefault(x => x.PersonnelId == _personel.Id);
                        if (model.IsUserId == true && _user == null)
                        {
                            ////ساخت یوزر

                            var _existUSer = UserManager.FindByNameAsync(model.PersonnelCode).Result;
                            if (_existUSer != null)
                            {
                                _existUSer.PersonnelId = _personel.Id;
                                UserManager.UpdateAsync(_existUSer);
                            }
                            else
                            {
                                var _newuser = new ApplicationUser { UserName = model.PersonnelCode, Email = model.Email, PersonnelId = _personel.Id };
                                var _adminresult = UserManager.CreateAsync(_newuser, model.Mobile);
                                if (!_adminresult.Result.Succeeded)
                                {
                                    foreach (var item in _adminresult.Result.Errors)
                                        ModelState.AddModelError("", item);
                                    ViewBag.Factories = FactoryRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
                                    ViewBag.States = StateRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }); ;
                                    ViewBag.Eparchies = EparchyRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
                                    ViewBag.FactoryUnits = FactoryUnitRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, });
                                    return View(model);
                                }
                            }
                        }
                        if (model.CityName != null)
                        {
                            if (_personel.AddressId == null)
                            {
                                var _address = new Domain.Entity.Address()
                                {
                                    CityName = model.CityName,
                                    CountryName = model.CountryName,
                                    EparchyId = model.EparchyId.Value,
                                    PostalCode = model.PostalCode,
                                    RegionName = model.RegionName,
                                    Address1 = model.Address,
                                };
                                _personel.AddressId = AddressRepository.Add(_address);
                                //ساخت آدرس
                            }
                            else
                            {
                                var _address = AddressRepository.GetById(model.AddressId.Value);
                                _address.Address1 = model.Address;
                                _address.CityName = model.CityName;
                                _address.CountryName = model.CountryName;
                                _address.EparchyId = model.EparchyId.Value;
                                _address.PostalCode = model.PostalCode;
                                _address.RegionName = model.RegionName;
                                AddressRepository.Edit(_address);
                                //بروز رسانی آدرس
                            }
                        }
                        PersonnelRepository.Edit(_personel);

                        return RedirectToAction("Index");
                    }
                    catch (TransactionException ex)
                    {

                        ModelState.AddModelError("", "خطا در ذخیره سازی");
                    }



                }
                else
                {

                    try
                    {
                        var _personnel = new Personnel()
                        {
                            BirthDate = string.IsNullOrEmpty(model.PersianBirthDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.PersianBirthDate, 0),
                            CellPhone = model.CellPhone,
                            Email = model.Email,
                            EmergencyPhone = model.EmergencyPhone,
                            Gender = model.Gender,
                            FactoryUnitId = model.FactoryUnitId,
                            Image = model.Image,
                            IsArchive = false,
                            IsDelete = false,
                            LastName = model.LastName,
                            Mobile = model.Mobile,
                            Name = model.Name,
                            NationalCode = model.NationalCode,
                            PersonnelCode = model.PersonnelCode,
                        };


                        if (model.CityName != null)
                        {
                            var _address = new Domain.Entity.Address()
                            {
                                CityName = model.CityName,
                                CountryName = model.CountryName,
                                EparchyId = model.EparchyId.Value,
                                PostalCode = model.PostalCode,
                                RegionName = model.RegionName,
                                Address1 = model.Address,
                            };
                            _personnel.AddressId = AddressRepository.Add(_address);
                            //ساخت آدرس
                        }
                        var _personnelId = PersonnelRepository.Add(_personnel);
                        if (model.IsUserId == true)
                        {
                            ////ساخت یوزر
                            var _existUSer = UserManager.FindByNameAsync(model.PersonnelCode).Result;
                            if (_existUSer != null)
                            {
                                _existUSer.PersonnelId = _personnelId;
                                UserManager.UpdateAsync(_existUSer);
                            }
                            else
                            {
                                var _newuser = new ApplicationUser { UserName = model.PersonnelCode, Email = model.Email, PersonnelId = _personnelId };
                                var _adminresult = UserManager.CreateAsync(_newuser, model.Mobile);
                                if (!_adminresult.Result.Succeeded)
                                {
                                    foreach (var item in _adminresult.Result.Errors)
                                        ModelState.AddModelError("", item);
                                    ViewBag.Factories = FactoryRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
                                    ViewBag.States = StateRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }); ;
                                    ViewBag.Eparchies = EparchyRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
                                    ViewBag.FactoryUnits = FactoryUnitRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, });
                                    return View(model);
                                }
                            }
                        }

                        return RedirectToAction("index");
                    }
                    catch (TransactionException ex)
                    {

                        ModelState.AddModelError("", "خطا در ذخیره سازی");
                    }
                }

            }
            ViewBag.Factories = FactoryRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.States = StateRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }); ;
            ViewBag.Eparchies = EparchyRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.FactoryUnits = FactoryUnitRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, });
            return View(model);
        }
        [AllowAnonymous]
        public JsonResult AutoPersonel(string searchString)
        {
            var _personels = new List<AutoPersonnel>();
            if (User.IsInRole("Admin") || User.IsInRole("Transport"))
            {
                _personels = PersonnelRepository.All(searchString, false, false).ToList().Take(10).Select(x => new AutoPersonnel { FullName = x.FullName, Id = x.Id, Text = x.FullName + "-" + x.PersonnelCode }).ToList();
            }
            else if (User.IsInRole("FactorManager"))
            {
                var _user = UserManager.FindById(User.Identity.GetUserId());
                _personels = ManagerPersonelRepository.GetPersonnelOfManager(_user.PersonnelId.Value, searchString, false, false).ToList().Take(10).Select(x => new AutoPersonnel { FullName = x.Personnel.FullName, Id = x.Personnel.Id, Text = x.Personnel.FullName + "-" + x.Personnel.PersonnelCode }).ToList();

            }
            return Json(_personels, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult GetEparchy(int id)
        {
            var _eparchies = EparchyRepository.All().Where(x => x.StateId == id).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).AsEnumerable();
            return Json(_eparchies, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult GetFactoryUnit(int id)
        {
            var _eparchies = FactoryUnitRepository.All().Where(x => x.FactoryId == id).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, }).AsEnumerable();
            return Json(_eparchies, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Personel/Details
        public ActionResult Detail(int id)
        {
            var _personel = PersonnelRepository.GetById(id);
            return View(_personel);
        }
        public ActionResult Delete(int id)
        {
            var _personel = PersonnelRepository.GetById(id);
            _personel.IsDelete = !_personel.IsDelete;
            PersonnelRepository.Edit(_personel);
            return RedirectToAction("Index");
        }
    }
}