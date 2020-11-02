using Domain.Entity;
using Domain.Repository;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Transport.Areas.Manage.Models;

namespace Transport.Areas.Manage.Controllers
{
    public class DriverTypePriceController : Controller
    {
        private DriverTypePriceRepository DriverTypePriceRepository = new DriverTypePriceRepository();
        private DriverTypeRepository DriverTypeRepository = new DriverTypeRepository();
        // GET: Manage/DriverTypePrice
        public ActionResult Index(int? driverTypeId, int pageNumber = 1, int pageSize = 20)
        {
            var _driverTypePrices = DriverTypePriceRepository.All(driverTypeId, false, false).ToList().Select(x => new DriverTypePriceIndex
            {
                Id = x.Id,
                DriverTypeName = x.DriverType.Name,
                DailyPrice = x.DailyPrice,
                StratDate = (Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.StartDate, false, null)),
                EndDate = (x.EndDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.EndDate.Value, false, null)),
            });
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("DriverTypePriceList", _driverTypePrices.ToPagedList(pageNumber, pageSize))
          : View(_driverTypePrices.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create(int? id)
        {
            var _driverTypePriceView = new DriverTypePricesSave();
            if (id != null)
            {
                var _driverTypePrice = DriverTypePriceRepository.GetById(id.Value);
                if (_driverTypePrice != null)
                {
                    _driverTypePriceView.DailyPrice = _driverTypePrice.DailyPrice;
                    _driverTypePriceView.DriverTypeId = _driverTypePrice.DriverTypeId;
                    _driverTypePriceView.Id = _driverTypePrice.Id;
                    _driverTypePriceView.StartDate = Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_driverTypePrice.StartDate, false, null);
                    _driverTypePriceView.EndDate = _driverTypePrice.EndDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_driverTypePrice.EndDate.Value, false, null);
                }

            }
            ViewBag.DriverTypes = DriverTypeRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return View(_driverTypePriceView);
        }

        [HttpPost]
        public ActionResult Create(DriverTypePricesSave model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    var _driverTypePrice = new DriverTypePrice()
                    {
                        DriverTypeId = model.DriverTypeId,
                        DailyPrice = model.DailyPrice,
                        IsDelete = false,
                        IsLock = false,
                        StartDate = string.IsNullOrEmpty(model.EndDate) == true ? DateTime.Now : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.StartDate, 0),
                        EndDate = string.IsNullOrEmpty(model.EndDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.EndDate, 0),
                        TimeCreated = DateTime.Now,
                        UserCreated = User.Identity.GetUserId()
                    };
                    DriverTypePriceRepository.Add(_driverTypePrice);
                    return RedirectToAction("Index");
                }
                else
                {
                    var _driverTypePrice = DriverTypePriceRepository.GetById(model.Id);
                    _driverTypePrice.DailyPrice = model.DailyPrice;
                    _driverTypePrice.StartDate = string.IsNullOrEmpty(model.EndDate) == true ? DateTime.Now : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.StartDate, 0);
                    _driverTypePrice.EndDate = string.IsNullOrEmpty(model.EndDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.EndDate, 0);
                    _driverTypePrice.UserEdited = User.Identity.GetUserId();
                    _driverTypePrice.TimeEdited = DateTime.Now;
                    DriverTypePriceRepository.Edit(_driverTypePrice);
                    return RedirectToAction("Index");
                }
            }
            ViewBag.DriverTypes = DriverTypeRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            var _driverTypePrice = DriverTypePriceRepository.GetById(id);
            if (_driverTypePrice != null)
            {
                _driverTypePrice.IsDelete = true;
                _driverTypePrice.TimeEdited = DateTime.Now;
                _driverTypePrice.UserEdited = User.Identity.GetUserId();
                DriverTypePriceRepository.Edit(_driverTypePrice);
            }
            return View("Index");
        }
    }
}