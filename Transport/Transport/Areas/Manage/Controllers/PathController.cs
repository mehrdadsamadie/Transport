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
    [Authorize(Roles = "Admin,TranasportManager")]
    public class PathController : Controller
    {
        private PathRepository PathRepository = new PathRepository();
        private DistanceRepository DistanceRepository = new DistanceRepository();

        // GET: Manage/path
        public ActionResult Index()
        {
            var _paths = PathRepository.All(null, false).Select(x => new PathViewIndex
            {
                Id = x.Id,
                Name = x.Name,
                kilometer = x.Distance.kilometer,

            });
            return View(_paths);
        }

        public ActionResult Create(int? id)
        {
            var _PathView = new PathViewSave();
            ViewBag.Distance = DistanceRepository.All(false).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.kilometer.ToString() });
            if (id != null)
            {
                var _path = PathRepository.GetById(id.Value);
                if (_path != null)
                {
                    _PathView.Id = _path.Id;
                    _PathView.Name = _path.Name;
                    _PathView.DistanceId = _path.DistanceId;
                }
            }
            return View(_PathView);
        }
        [HttpPost]
        public ActionResult Create(PathViewSave model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    try
                    {
                        var _path = new Path();
                        _path.Name = model.Name;
                        _path.DistanceId = model.DistanceId;
                        _path.TimeCreated = System.DateTime.Now;
                        _path.UserCreated = User.Identity.GetUserId();

                        PathRepository.Add(_path);
                        return RedirectToAction("Index", model);
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
                        var _path = PathRepository.GetById(model.Id);
                        _path.DistanceId = model.DistanceId;
                        _path.Name = model.Name;
                        _path.TimeEdited = System.DateTime.Now;
                        _path.UserEdited = User.Identity.GetUserId();

                        PathRepository.Edit(_path);
                        return RedirectToAction("Index", model);
                    }
                    catch(Exception e)
                    {
                        ModelState.AddModelError("", "خطا در ذخیره سازی");
                    }
                }

            }
            ViewBag.Distance = DistanceRepository.All(false).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.kilometer.ToString() });
            return View(model);

        }

        public ActionResult Delete(int id)
        {
            var _path = PathRepository.GetById(id);
            if (_path != null)
            {
                _path.IsDelete = false;
                _path.TimeEdited = DateTime.Now;
                _path.UserEdited = User.Identity.GetUserId();
            }
            PathRepository.Edit(_path);
            return RedirectToAction("Index");

        }


    }
}