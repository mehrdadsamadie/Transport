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
    public class OverTimeController : Controller
    {
        OverTimeRepository _overtimeRepository = new OverTimeRepository();
        // GET: Manage/OverTime
        public ActionResult Index(string StartDate, string EndDate, int pageNumber = 1, int pageSize = 20)
        {
            DateTime? StartDateM = string.IsNullOrWhiteSpace(StartDate) ? (DateTime?)null :
            Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(StartDate, 0);
            DateTime? EndDateM = string.IsNullOrWhiteSpace(EndDate) ? (DateTime?)null :
                Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(EndDate, 0);

            var _overtimes = _overtimeRepository.All(StartDateM,EndDateM).ToList().Select(x => new OverTimeViewModel
            {
                Id = x.Id,
                StartDate = Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.StartDate,false),
                EndDate = Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.EndDate,false),
                IsLock = x.IsLock,
                Price = x.Price
            });
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("OvertimeList", _overtimes.ToPagedList(pageNumber, pageSize))
                : View(_overtimes.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create(int? id)
        {
            var _overtimeView = new OverTimeViewModel();
            if (id != null)
            {
                var _overtime = _overtimeRepository.GetById(id.Value);
                if (_overtime != null)
                {
                    _overtimeView.StartDate = Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_overtime.StartDate, false);
                    _overtimeView.EndDate = Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_overtime.EndDate, false);
                    _overtimeView.Price = _overtime.Price;
                    _overtimeView.IsLock = _overtime.IsLock;
                }
            }
            return View(_overtimeView);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(OverTimeViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id != 0)
                {
                    var _overtime = _overtimeRepository.GetById(model.Id);
                    _overtime.StartDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.StartDate, 0);
                    _overtime.EndDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.EndDate, 0);
                    _overtime.Price = model.Price;
                    _overtime.IsLock = model.IsLock;
                    _overtime.UserEdited = User.Identity.GetUserId();
                    _overtime.TimeEdited = DateTime.Now;

                    _overtimeRepository.Edit(_overtime);
                    return RedirectToAction("Index");
                }
                else
                {
                    var _overtime = new OverTime()
                    {
                        StartDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.StartDate, 0),
                        EndDate = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.EndDate,0),
                        Price = model.Price,
                        UserCreated = User.Identity.GetUserId(),
                        TimeCreated = DateTime.Now
                    };

                    _overtimeRepository.Add(_overtime);
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var _overtime = _overtimeRepository.GetById(id);
            _overtime.IsDelete = true;
            _overtimeRepository.Edit(_overtime);
            return RedirectToAction("Index");
        }

    }
}