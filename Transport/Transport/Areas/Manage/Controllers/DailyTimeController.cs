using Domain.Entity;
using Domain.Repository;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Transport.Areas.Manage.Infrastructure;
using Transport.Areas.Manage.Models;

namespace Transport.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DailyTimeController : Controller
    {
        DailyTimeRepository dailyTimeRepository = new DailyTimeRepository();
        // GET: Manage/DailyTime
        public ActionResult Index(string StartDate, string EndDate, int pageNumber = 1, int pageSize = 20)
        {
            DateTime? StartDateM = string.IsNullOrWhiteSpace(StartDate) ? (DateTime?)null:
                Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(StartDate, 0);
            DateTime? EndDateM = string.IsNullOrWhiteSpace(EndDate) ? (DateTime?)null : 
                Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(EndDate, 0);
            var _dailyTimes = dailyTimeRepository.All(StartDateM, EndDateM).ToList().Select(x => new DailyTimeViewModel
            {
                Id = x.Id,
                StartDate = x.StartDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.StartDate.Value, false),
                EndDate = x.EndDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.EndDate.Value, false),
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                WeekenDay = CodingDayOfWeek.DecodeDayOfWeek((DayOfWeek)x.WeekenDay)
            });
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("DailyTimeList", _dailyTimes.ToPagedList(pageNumber, pageSize))
            : View(_dailyTimes.ToPagedList(pageNumber, pageSize));
        }
       
        public ActionResult Create(int? id)
        {
            var _dailyTimeView = new DailyTimeViewModel();
            List<Tuple<int, string>> days = new List<Tuple<int, string>>();
            foreach (DayOfWeek item in Enum.GetValues(typeof(DayOfWeek)))
            {
                days.Add(new Tuple<int, string>((int)item, CodingDayOfWeek.DecodeDayOfWeek(item)));
            }

            #region BringSaturdayToBeginnig
            var last = days.Last();
            days.Remove(last);
            days.Insert(0, last);
            #endregion

            ViewBag.daysOfWeek = days.Select(x => new SelectListItem() { Value = x.Item1.ToString(), Text = x.Item2 });
            if (id != null)
            {
                var _dailyTime = dailyTimeRepository.GetById(id.Value);
                if (_dailyTime != null)
                {
                    _dailyTimeView.Id = _dailyTime.Id;
                    _dailyTimeView.StartDate = _dailyTime.StartDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_dailyTime.StartDate.Value, false);
                    _dailyTimeView.EndDate = _dailyTime.EndDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_dailyTime.EndDate.Value, false);
                    _dailyTimeView.StartTime = _dailyTime.StartTime;
                    _dailyTimeView.EndTime = _dailyTime.EndTime;
                    _dailyTimeView.WeekenDay = CodingDayOfWeek.DecodeDayOfWeek((DayOfWeek)_dailyTime.WeekenDay);
                }
            }
            return View(_dailyTimeView);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(DailyTimeViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id != 0)
                {
                    var _dailyTime = dailyTimeRepository.GetById(model.Id);
                    _dailyTime.StartDate = string.IsNullOrWhiteSpace(model.StartDate) ? (DateTime?)null: Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.StartDate,0);
                    _dailyTime.EndDate = string.IsNullOrWhiteSpace(model.EndDate) ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.EndDate, 0);
                    _dailyTime.StartTime = model.StartTime;
                    _dailyTime.EndTime = model.EndTime;
                    _dailyTime.WeekenDay = int.Parse(model.WeekenDay);

                    dailyTimeRepository.Edit(_dailyTime);
                    return RedirectToAction("Index");
                }
                else
                {
                    var _dailyTime = new DailyTime()
                    {
                        StartDate = string.IsNullOrWhiteSpace(model.StartDate) ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.StartDate, 0),
                        EndDate = string.IsNullOrWhiteSpace(model.EndDate) ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.EndDate, 0),
                        StartTime = model.StartTime,
                        EndTime = model.EndTime,
                        WeekenDay = int.Parse(model.WeekenDay)
                    };
                    dailyTimeRepository.Add(_dailyTime);
                    return RedirectToAction("Index");
                }
            }
            List<Tuple<int, string>> days = new List<Tuple<int, string>>();
            foreach (DayOfWeek item in Enum.GetValues(typeof(DayOfWeek)))
            {
                days.Add(new Tuple<int, string>((int)item, CodingDayOfWeek.DecodeDayOfWeek(item)));
            }

            #region BringSaturdayToBeginnig
            var last = days.Last();
            days.Remove(last);
            days.Insert(0, last);
            #endregion
            ViewBag.daysOfWeek = days.Select(x => new SelectListItem() { Value = x.Item1.ToString(), Text = x.Item2 });
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var _dailyTime = dailyTimeRepository.GetById(id);
            _dailyTime.IsDelete = true;
            dailyTimeRepository.Edit(_dailyTime);
            return RedirectToAction("Index");
        }

    }
}