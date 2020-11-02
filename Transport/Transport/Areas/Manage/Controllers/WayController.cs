using Domain.Entity;
using Domain.Repository;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Transport.Areas.Manage.Models;

namespace Transport.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin,TranasportManager,Tranasport")]
    public class WayController : Controller
    {
        WayRepository WayRepository = new WayRepository();
        PathRepository PathRepository = new PathRepository();

        // GET: Manage/Way
        public ActionResult Index(string searchString, int? pathId, int pageNumber = 1, int pageSize = 100)
        {
            ViewBag.Pathes = PathRepository.All(null, false).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            var _waies = WayRepository.All(searchString, pathId).OrderByDescending(x => x.Path.Name).Select(x => new WayIndex
            {
                Id = x.Id,
                Name = x.Name,
                PathName = x.Path.Name,
            }).ToList();
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("WayList", _waies.ToPagedList(pageNumber, pageSize))
                : View(_waies.ToPagedList(pageNumber, pageSize));
        }

        // GET: Manage/Way/Create
        public ActionResult Create(int? id)
        {
            var _wayView = new WaySave();
            ViewBag.Pathes = PathRepository.All(null, false).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            if (id != null)
            {
                var _way = WayRepository.GetById(id.Value);
                if (_way != null)
                {
                    _wayView.Id = _way.Id;
                    _wayView.Name = _way.Name;
                    _wayView.PathId = _way.PathId;

                }
            }
            return View(_wayView);
        }

        // POST: Manage/Way/Create
        [HttpPost]
        public ActionResult Create(WaySave model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id == 0)
                    {
                        var _way = new Way()
                        {
                            Name = model.Name,
                            PathId = model.PathId,
                            TimeCreated = DateTime.Now,
                            UserCreated = User.Identity.GetUserId()
                        };
                        WayRepository.Add(_way);
                    }
                    else
                    {
                        var _way = WayRepository.GetById(model.Id);
                        _way.Name = model.Name;
                        _way.PathId = model.PathId;
                        _way.UserEdited = User.Identity.GetUserId();
                        _way.TimeEdited = DateTime.Now;
                        WayRepository.Edit(_way);
                    }
                    // TODO: Add insert logic here

                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
            }
            ViewBag.Pathes = PathRepository.All(null, false).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return View(model);

        }

        // GET: Manage/Way/Delete/5
        public ActionResult Delete(int id)
        {
            var _way = WayRepository.GetById(id);
            if (_way != null)
            {
                try
                {
                    WayRepository.Delete(_way.Id);
                  
                }
                catch (Exception e)
                {
                    var i = 0;
                }
            }
            return RedirectToAction("Index");

        }


    }
}
