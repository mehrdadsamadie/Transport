using Domain.Entity;
using Domain.Repository;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Transport.Areas.Manage.Models;

namespace Transport.Areas.Manage.Controllers
{
    public class FactoryUserManagerController : Controller
    {
        FactoryUserManagerRepository FactoryUserManagerRepository = new FactoryUserManagerRepository();
        FactoryRepository FactoryRepository = new FactoryRepository();
        FactoryUnitRepository FactoryUnitRepository = new FactoryUnitRepository();
        PersonnelRepository PersonnelRepository = new PersonnelRepository();
        // GET: Manage/FactoryUserManager
        public ActionResult Index(string searchString, int? factoryUnitId, bool? isDelete=false, int pageNumber = 1, int pageSize = 30)
        {
            var _factoryUserManager = FactoryUserManagerRepository.All(searchString, factoryUnitId, isDelete).ToList().Select(x => new FactoryUserManagerViewIndex
            {
                Id = x.Id,
                PersonnelId = x.PersonnelId,
                FullName = x.Personnel.FullName,
                Name = x.FactoryUnit.Factory.Name+"-"+ x.FactoryUnit.Name,
                FactoryUnitId = x.FactoryUnitId
            }).ToList();
            ViewBag.FactoryUnits = FactoryUnitRepository.All().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.Factories = FactoryRepository.All().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("FactoryUserManagerList", _factoryUserManager.ToPagedList(pageNumber, pageSize))
           : View(_factoryUserManager.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create(int? id)
        {
            ViewBag.FactoryUnits = FactoryUnitRepository.All().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.Factories = FactoryRepository.All().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            var _factoryUserManagerSave = new FactoryUserManagerSave();
            if (id != null)
            {
                var _factoryUserManager = FactoryUserManagerRepository.GetById(id.Value);
                _factoryUserManagerSave.Id = _factoryUserManager.Id;
                _factoryUserManagerSave.FullName = _factoryUserManager.Personnel.FullName;
                _factoryUserManagerSave.PersonnelId = _factoryUserManager.PersonnelId;
                _factoryUserManagerSave.FactoryUnitId = _factoryUserManager.FactoryUnitId;
                _factoryUserManagerSave.FactoryId = _factoryUserManager.Personnel.FactoryUnit.FactoryId;
            }
            return View(_factoryUserManagerSave);
        }

        [HttpPost]
        public ActionResult Create(FactoryUserManagerSave model)
        {
            if (ModelState.IsValid) {
                if (model.Id == 0)
                {
                    try
                    {
                        var _factorUserManager = new FactoryUserManager()
                        {
                            FactoryUnitId = model.FactoryUnitId,
                            IsDelete = false,
                            PersonnelId = model.PersonnelId
                        };
                        FactoryUserManagerRepository.Add(_factorUserManager);
                        return RedirectToAction("Index");
                    }
                    catch(Exception e)
                    {
                        ModelState.AddModelError("", "خطا در ذخیره سازی اطلاعات");
                    }
                }
                else
                {
                    try
                    {
                        var _factorUserManager = FactoryUserManagerRepository.GetById(model.Id);
                        _factorUserManager.PersonnelId = model.PersonnelId;
                        _factorUserManager.FactoryUnitId = model.FactoryUnitId;
                        FactoryUserManagerRepository.Edit(_factorUserManager);
                        return RedirectToAction("Index");
                    }
                    catch(Exception e)
                    {
                        ModelState.AddModelError("", "خطا در ذخیره سازی اطلاعات");
                    }
                }
            }
            ViewBag.FactoryUnits = FactoryUnitRepository.All().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.Factories = FactoryRepository.All().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var _factoryUserManager = FactoryUserManagerRepository.GetById(id);
            _factoryUserManager.IsDelete = !_factoryUserManager.IsDelete;
            FactoryUserManagerRepository.Edit(_factoryUserManager);
            return RedirectToAction("Index");
        }
    }
}