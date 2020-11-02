using Domain.Entity;
using Domain.Repository;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Transport.Areas.Manage.Models;

namespace Transport.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DistanceController : Controller
    {
        private DistanceRepository DistanceRepository = new DistanceRepository();

        // GET: Manage/Distance
        public ActionResult Index()
        {
            var _Distance = DistanceRepository.All(false).OrderByDescending(x => x.kilometer).Select(x => new DistanceViewIndex
            {
                Id = x.Id,
                kilometer = x.kilometer,

            });
            return View(_Distance);
        }
        public ActionResult Create(int? id)
        {
            var _DistanceView = new DistanceSave();
            if (id != null)
            {
                var _Distance = DistanceRepository.GetById(id.Value);
                if (_Distance != null)
                {
                    _DistanceView.Id = _Distance.Id;
                    _DistanceView.kilometer = _Distance.kilometer;
                }
            }
            return View(_DistanceView);
        }
        [HttpPost]
        public ActionResult Create(DistanceSave model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    try
                    {
                        var _Distance = new Distance();
                        _Distance.kilometer = model.kilometer.Value;
                        _Distance.TimeCreated = System.DateTime.Now;
                        _Distance.UserCreated = User.Identity.GetUserId();
                        DistanceRepository.Add(_Distance);
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
                        var _Distance = DistanceRepository.GetById(model.Id);
                        _Distance.kilometer = model.kilometer.Value;
                        _Distance.TimeEdited = System.DateTime.Now;
                        _Distance.UserEdited = User.Identity.GetUserId();
                        DistanceRepository.Edit(_Distance);
                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "خطا در ذخیره سازی");
                    }

                }
            }
            return View(model);

        }

        public ActionResult Delete(int id)
        {
            var _Distance = DistanceRepository.GetById(id);
            if (_Distance != null)
            {
                _Distance.IsDelete = false;
                _Distance.TimeEdited = DateTime.Now;
                _Distance.UserEdited = User.Identity.GetUserId();
                DistanceRepository.Edit(_Distance);
            }
            return RedirectToAction("Index");
        }
    }
}