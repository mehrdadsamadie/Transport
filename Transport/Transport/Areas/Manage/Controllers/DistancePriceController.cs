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
    [Authorize(Roles = "Admin")]
    public class DistancePriceController : Controller
    {
        private DistancePriceRepository DistancePriceRepository = new DistancePriceRepository();
        private DriverTypeRepository DriverTypeRepository = new DriverTypeRepository();
        private DistanceRepository DistanceRepository = new DistanceRepository();
        // GET: Manage/DistancePrice
        public ActionResult Index(int? driverTypeId, int? distanceId, bool? isLast,int pageNumber=1,int pageSize=1)
        {
            var _distancePrices = DistancePriceRepository.All(distanceId, driverTypeId).ToList().Select(x => new DistancePriceIndex
            {
                Id = x.Id,
                Price = x.Price,
                DriverTypeName = x.DriverType.Name,
                kilometer = x.Distance.kilometer,
                PathList = x.Distance.Paths == null ? null :string.Join(",", x.Distance.Paths.Select(y => y.Name).ToArray()),
                StartTime = x.StartTime==null?null:Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.StartTime,false,null),
                EndTime=x.EndTime==null?null:Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.EndTime.Value,false,null),
            }).ToList();
            ViewBag.DriverTypes = DriverTypeRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Distances = DistanceRepository.All(false).OrderByDescending(x => x.kilometer).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.kilometer.ToString() });
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("DistancePriceList", _distancePrices.ToPagedList(pageNumber, pageSize))
            : View(_distancePrices.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create(int? id)
        {
            var _distancePriceView = new DistancePriceSave();
            if (id != null)
            {
                var _distancePrice = DistancePriceRepository.GetById(id.Value);
                _distancePriceView.DistanseId = _distancePrice.DistanseId;
                _distancePriceView.DriverTypeId = _distancePrice.DriverTypeId;
                _distancePriceView.Id = _distancePrice.Id;
                _distancePriceView.Price = _distancePrice.Price;
                _distancePriceView.EndTime = _distancePrice.EndTime == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_distancePrice.EndTime.Value, false, null);
                _distancePriceView.StartTime = _distancePrice.StartTime == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_distancePrice.StartTime, false, null);
            }
            ViewBag.DriverTypes = DriverTypeRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Distances = DistanceRepository.All(false).OrderByDescending(x=>x.kilometer).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.kilometer.ToString() });
            return View(_distancePriceView);
        }
        [HttpPost]
        public ActionResult Create(DistancePriceSave model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                   
                    var _distancePrice = new DistancePrice();
                    _distancePrice.DistanseId = model.DistanseId;
                    _distancePrice.DriverTypeId = model.DriverTypeId;
                    _distancePrice.EndTime = string.IsNullOrEmpty(model.EndTime) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.EndTime, 0);
                    _distancePrice.Price = model.Price;
                    _distancePrice.StartTime = string.IsNullOrEmpty(model.StartTime) == true ? DateTime.Now : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.StartTime, 0);
                    _distancePrice.TimeCreated = DateTime.Now;
                    _distancePrice.UserCreated = User.Identity.GetUserId();
                    _distancePrice.IsDelete = false;
                    _distancePrice.IsLock = false;
                    DistancePriceRepository.Add(_distancePrice);
                    return RedirectToAction("Index");

                }
                else
                {
                    var _distancePrice = DistancePriceRepository.GetById(model.Id);
                    _distancePrice.StartTime = string.IsNullOrEmpty(model.StartTime) == true ? _distancePrice.StartTime : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.StartTime, 0);
                    _distancePrice.TimeEdited = DateTime.Now;
                    _distancePrice.UserEdited = User.Identity.GetUserId();
                    _distancePrice.Price = model.Price;
                    _distancePrice.EndTime = string.IsNullOrEmpty(model.EndTime) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.EndTime, 0);
                    DistancePriceRepository.Edit(_distancePrice);
                    return RedirectToAction("Index");

                }
            }
            ViewBag.DriverTypes = DriverTypeRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Distances = DistanceRepository.All(false).OrderByDescending(x=>x.kilometer).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.kilometer.ToString() });
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            var _distancePrice = DistancePriceRepository.GetById(id);
            if (_distancePrice != null)
            {
                _distancePrice.IsDelete = true;
                _distancePrice.TimeEdited = DateTime.Now;
                _distancePrice.UserEdited = User.Identity.GetUserId();
                DistancePriceRepository.Edit(_distancePrice);
            }
            return RedirectToAction("Index");
        }
    }
}