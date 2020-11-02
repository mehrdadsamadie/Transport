using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Transport.Areas.Manage.Models;

namespace Transport.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin,TranasportManager")]
    public class TaxiCompanyController : Controller
    {
        private TaxiCompanyRepository TaxiCompanyRepository = new TaxiCompanyRepository();

        // GET: Manage/TaxiCompany
        public ActionResult Index()
        {
            var _taxiDriver = TaxiCompanyRepository.All(null,false).Select(x => new TaxiCompanyIndex
            {
                Id = x.Id,
                Name = x.Name,
                CallPhone = x.CellPhone,
                EconomicalCode = x.EconomicalCode,
                ManagerName = x.ManagerName,
                MobilePhone = x.MobilePhone,
                RegistrationNumber = x.RegistrationNumber,
               
            });
            return View(_taxiDriver);
        }
        public ActionResult Create(int? id)
        {
            var _TaxiCompanyView = new TaxiSave();
            if (id != null)
            {
                var _taxiDriver = TaxiCompanyRepository.GetById(id.Value);
                if (_taxiDriver != null)
                {
                    _TaxiCompanyView.Id = _taxiDriver.Id;
                    _TaxiCompanyView.Name = _taxiDriver.Name;
                    _TaxiCompanyView.CellPhone = _taxiDriver.CellPhone;
                    _TaxiCompanyView.EconomicalCode = _taxiDriver.EconomicalCode;
                    _TaxiCompanyView.ManagerName = _taxiDriver.ManagerName;
                    _TaxiCompanyView.MobilePhone = _taxiDriver.MobilePhone;
                    _TaxiCompanyView.RegistrationNumber = _taxiDriver.RegistrationNumber;
                }
            }

            return View(_TaxiCompanyView);
        }
        [HttpPost]
        public ActionResult Create(TaxiSave model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    try
                    {
                        var _taxiDriver = new TaxiCompany();
                        _taxiDriver.Name = model.Name;
                        _taxiDriver.CellPhone = model.CellPhone;
                        _taxiDriver.EconomicalCode = model.EconomicalCode;
                        _taxiDriver.ManagerName = model.ManagerName;
                        _taxiDriver.MobilePhone = model.MobilePhone;
                        _taxiDriver.RegistrationNumber = model.RegistrationNumber;
         
                        TaxiCompanyRepository.Add(_taxiDriver);
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
                        var _taxiDriver = TaxiCompanyRepository.GetById(model.Id);
                        _taxiDriver.Name = model.Name;
                        _taxiDriver.CellPhone = model.CellPhone;
                        _taxiDriver.EconomicalCode = model.EconomicalCode;
                        _taxiDriver.ManagerName = model.ManagerName;
                        _taxiDriver.MobilePhone = model.MobilePhone;
                        _taxiDriver.RegistrationNumber = model.RegistrationNumber;

                        TaxiCompanyRepository.Edit(_taxiDriver);
                        return RedirectToAction("Index");
                    }
                    catch(Exception e)
                    {
                        ModelState.AddModelError("", "خطا در ذخیره سازی");
                    }
                }

            }
            return View( model);

        }
        public ActionResult Delete(int id)
        {
            var _taxiDriver = TaxiCompanyRepository.GetById(id);
            _taxiDriver.IsDelete = true;
            TaxiCompanyRepository.Edit(_taxiDriver);
            return RedirectToAction("Index");
        }

    }
}