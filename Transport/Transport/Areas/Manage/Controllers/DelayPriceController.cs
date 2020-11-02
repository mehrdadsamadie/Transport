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

    public class DelayPriceController : Controller
    {
        private DelayPriceRepository DelayPriceRepository = new DelayPriceRepository();
        private DriverTypeRepository DriverTypeRepository = new DriverTypeRepository();
        // GET: Manage/DelayPrice
        public ActionResult Index(int? drivertypeId, int pageNumber = 1, int pageSize = 20, bool? isDelete = false)
        {
            var _delayPrices = DelayPriceRepository.All(drivertypeId, isDelete).ToList().Select(x => new DelayPriceViewIndex
            {
                Id = x.Id,
                Price = x.Price,
                DelayTime = x.DelayTime,
                DriverTypeName = x.DriverType.Name,
                DriverTypeId = x.DriverTypeId,
                StartDate = Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.StartDate, false, null),
                EndDate = x.EndDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.EndDate.Value, false, null),
            });
            ViewBag.DriverTypes = DriverTypeRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("DelayPriceList", _delayPrices.ToPagedList(pageNumber, pageSize))
            : View(_delayPrices.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create(int? id)
        {
            var _delayPriceView = new DelayPriceSave();
            if (id != null)
            {
                var _delayPrice = DelayPriceRepository.GetById(id.Value);
                if (_delayPrice != null)
                {
                    _delayPriceView.Id = _delayPrice.Id;
                    _delayPriceView.DriverTypeId = _delayPrice.DriverTypeId;
                    _delayPriceView.DelayTime = _delayPrice.DelayTime;
                    _delayPriceView.StartDate = Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_delayPrice.StartDate, false, null);
                    _delayPriceView.EndDate = _delayPrice.EndDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_delayPrice.EndDate.Value, false, null);
                    _delayPriceView.Price = _delayPrice.Price;
                }
            }
            ViewBag.DriverTypes = DriverTypeRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return View(_delayPriceView);
        }
        [HttpPost]
        public ActionResult Create(DelayPriceSave model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id != 0)
                {
                    try
                    {
                        var _delayPrice = DelayPriceRepository.GetById(model.Id);
                        _delayPrice.Price = model.Price;
                        _delayPrice.DelayTime = model.DelayTime;
                        _delayPrice.DriverTypeId = model.DriverTypeId;
                        _delayPrice.StartDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.StartDate, 0);
                        _delayPrice.EndDate = string.IsNullOrEmpty(model.StartDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.EndDate, 0);
                        _delayPrice.UserEdited = User.Identity.GetUserId();
                        _delayPrice.TimeEdited = DateTime.Now;
                        DelayPriceRepository.Edit(_delayPrice);
                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "خطا در ذخیره سازی");
                    }
                }
                else
                {
                    try
                    {
                        var _delayPrice = new DelayPrice()
                        {
                            DriverTypeId = model.DriverTypeId,
                            EndDate = string.IsNullOrEmpty(model.EndDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.EndDate, 0),
                            IsDelete = false,
                            Price = model.Price,
                            StartDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.StartDate, 0),
                            TimeCreate = DateTime.Now,
                            UserCreated = User.Identity.GetUserId()
                        };
                        DelayPriceRepository.Add(_delayPrice);
                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "خطا در ذخیره سازی");
                    }
                }
            }
            ViewBag.DriverTypes = DriverTypeRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            var _delayPrice = DelayPriceRepository.GetById(id);
            if (_delayPrice != null)
            {
                _delayPrice.IsDelete = !_delayPrice.IsDelete;
                _delayPrice.TimeEdited = DateTime.Now;
                _delayPrice.UserEdited = User.Identity.GetUserId();
            }
            DelayPriceRepository.Edit(_delayPrice);
            return RedirectToAction("Index");
        }
    }
}