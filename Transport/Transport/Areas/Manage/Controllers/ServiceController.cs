using Domain.Repository;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Transport.Areas.Manage.Models;
using Microsoft.AspNet.Identity;
using Domain.Entity;
using System.Transactions;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Web.Http.Cors;
using Transport.Infrastructure;

namespace Transport.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin,Transport,TranasportManager")]
    public class ServiceController : Controller
    {
        private ServiceRepository ServiceRepository = new ServiceRepository();
        private DriverTypeRepository DriverTypeRepository = new DriverTypeRepository();
        private PathRepository PathRepository = new PathRepository();
        private EparchyRepository EparchyRepository = new EparchyRepository();
        private StateRepository StateRepository = new StateRepository();
        private TaxiCompanyRepository TaxiCompanyRepository = new TaxiCompanyRepository();
        private AddressRepository AddressRepository = new AddressRepository();
        private RequestRepository RequestRepository = new RequestRepository();
        private ServiceStatuRepository ServiceStatuRepository = new ServiceStatuRepository();
        private ServiceTypeRepository ServiceTypeRepository = new ServiceTypeRepository();
        private RDriverDriverTypeRepository RDriverDriverTypeRepository = new RDriverDriverTypeRepository();
        private WayRepository WayRepository = new WayRepository();
        // GET: Manage/Service
        //public ActionResult Index(int? driverId, string startDate, string endDate, bool? isDelete = false, int pageNumber = 1, int pageSize = 50)
        //{
        //    var _startDate = string.IsNullOrEmpty(startDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(startDate, 0);
        //    var _endDate = string.IsNullOrEmpty(endDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(endDate, 0);

        //    var _services = ServiceRepository.All(driverId, null, _startDate, _endDate, isDelete, null).ToList().
        //        Select(x => new ServiseIndexView
        //        {
        //            Id = x.Id,
        //            Beginning = x.BeginingAddress == null ? null : x.BeginingAddress.Address1,
        //            Destination = x.Destination == null ? null : x.DestinationAddress.Address1,
        //            Date = x.Date == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.Date.Value, false, null),
        //            FullName = x.DriverId == null ? x.TaxiCompany.Name : x.Driver.Personnel.FullName,
        //            DriverType = x.DriverType == null ? null : x.DriverType.Name,
        //            Name = x.Path == null ? x.CalcDistance.ToString() : x.Path.Name,
        //            IsLocal = x.IsLocal,
        //            RequestId = RequestRepository.GetByServiceId(x.Id) == null ? (int?)null : RequestRepository.GetByServiceId(x.Id).Id,
        //            RequestName = RequestRepository.GetByServiceId(x.Id) == null ? null : RequestRepository.GetByServiceId(x.Id).Personnel.FullName,
        //            RequestUnit = RequestRepository.GetByServiceId(x.Id) == null ? null : RequestRepository.GetByServiceId(x.Id).Personnel.FactoryUnit.Factory.Name + '-' + RequestRepository.GetByServiceId(x.Id).Personnel.FactoryUnit.Name,
        //            Requests = RequestRepository.GetRequestsByServiceId(x.Id).ToList().Select(y => new RequestViewInService { Id = y.Id, FullName = y.Personnel.FullName, Factory = y.Personnel.FactoryUnit.Factory.Name + "-" + y.Personnel.FactoryUnit.Name }).ToList(),
        //            ServiceStatus = x.ServiceStatu.Name,
        //            IsAcceptTranasportManager = x.IsAcceptTranasportManager
        //        }).ToList();
        //    ViewBag.ServiceStatues = ServiceStatuRepository.All().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
        //    return Request.IsAjaxRequest() ? (ActionResult)PartialView("ServiceList", _services.ToPagedList(pageNumber, pageSize))
        //  : View(_services.ToPagedList(pageNumber, pageSize));
        //}

        public ActionResult Index(string startDate, string endDate, string startTime, string endTime)
        {
            return View();
        }
        public JsonResult List(int? driverId, string startDate, string endDate, string startTime, string endTime, int[] serviceStatusId, bool? isAcceptTranasportManager, bool? isDelete = false, int pageNumber = 0, int pageSize = 50, Enums.OrderEnumService order = Enums.OrderEnumService.ServiceDate, bool isAsc = true)
        {
            var _startDate = string.IsNullOrEmpty(startDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(startDate, 0);
            var _endDate = string.IsNullOrEmpty(endDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(endDate, 0);
            var _startTime = string.IsNullOrEmpty(startTime) == true ? (TimeSpan?)null : TimeSpan.Parse(startTime);
            var _endTime = string.IsNullOrEmpty(endTime) == true ? (TimeSpan?)null : TimeSpan.Parse(endTime);
            var _serviceStatusId = (serviceStatusId == null ? Enumerable.Empty<int>() : serviceStatusId.Select(x => x));
            var _totalCount = ServiceRepository.All(driverId, _serviceStatusId, _startDate, _endDate, _startTime, _endTime, isAcceptTranasportManager, isDelete, null, order, isAsc).Count();
            var _services = ServiceRepository.All(driverId, _serviceStatusId, _startDate, _endDate, _startTime, _endTime, isAcceptTranasportManager, isDelete, null, order, isAsc).Skip(pageNumber * pageSize).Take(pageSize).ToList();
            var _serviceView = _services.Select(x => new ServiceIndexView
            {
                Id = x.Id,
                Beginning = x.BeginingAddress == null ? null : x.BeginingAddress.Eparchy.Name + " " + x.BeginingAddress.Address1,
                Destination = x.Destination == null ? null : x.DestinationAddress.Eparchy.Name + " " + x.DestinationAddress.Address1,
                Date = x.Date == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.Date.Value, false, null),
                StartTime = x.StartTime == null ? null : x.StartTime.Value.ToString(@"hh\:mm"),
                EndTime = x.EndTime == null ? null : x.EndTime.Value.ToString(@"hh\:mm"),
                FullName = x.DriverId == null ? x.TaxiCompany.Name : x.Driver.Personnel.FullName,
                DriverType = x.DriverType == null ? null : x.DriverType.Name,
                Name = x.Path == null ? x.CalcDistance.ToString() : x.Path.Name,
                IsLocal = x.IsLocal,
                RequestId = RequestRepository.GetByServiceId(x.Id) == null ? (int?)null : RequestRepository.GetByServiceId(x.Id).Id,
                RequestName = RequestRepository.GetByServiceId(x.Id) == null ? null : RequestRepository.GetByServiceId(x.Id).Personnel.FullName,
                RequestUnit = RequestRepository.GetByServiceId(x.Id) == null ? null : RequestRepository.GetByServiceId(x.Id).Personnel.FactoryUnit.Factory.Name + '-' + RequestRepository.GetByServiceId(x.Id).Personnel.FactoryUnit.Name,
                Requests = RequestRepository.GetRequestsByServiceId(x.Id).ToList().Select(y => new RequestViewInService {
                    Id = y.Id,
                    FullName = y.Personnel.FullName,
                    Factory = y.Personnel.FactoryUnit.Factory.Name + "-" + y.Personnel.FactoryUnit.Name,
                    Begining = y.BeginingAddress.Eparchy.Name + "-" +  y.BeginingAddress.Address1,
                    Destination=y.DestinationAddress.Eparchy.Name+"-"+y.DestinationAddress.Address1,
                    Number = y.ServiceTypeId == (int)Enums.ServiceType.Services ? 0 : y.GussetNumber == null ? 1 : y.GussetNumber + 1,
                    Mobile = y.Personnel.Mobile,
                }).ToList(),
                ServiceStatus = x.ServiceStatusId,
                IsAcceptTranasportManager = x.IsAcceptTranasportManager,
                IsDelete = x.IsDelete,
                IsLock = x.IsLock,
            }).ToList();
            return Json(new { Services = _serviceView, Total = _totalCount }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportData(int? driverId, string startDate, string endDate,string startTime,string endTime, int[] serviceStatusId, bool? isAcceptTranasportManager, bool? isDelete = false, Enums.OrderEnumService order = Enums.OrderEnumService.ServiceDate, bool isAsc = true)
        {
            var _startDate = string.IsNullOrEmpty(startDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(startDate, 0);
            var _endDate = string.IsNullOrEmpty(endDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(endDate, 0);
            var _startTime = string.IsNullOrEmpty(startTime) == true ? (TimeSpan?)null : TimeSpan.Parse(startTime);
            var _endTime = string.IsNullOrEmpty(endTime) == true ? (TimeSpan?)null : TimeSpan.Parse(endTime);
            var _serviceStatusId = (serviceStatusId == null ? Enumerable.Empty<int>() : serviceStatusId.Select(x => x));
            GridView gv = new GridView();
            gv.DataSource = ServiceRepository.All(driverId, _serviceStatusId, _startDate, _endDate,_startTime,_endTime, isAcceptTranasportManager, isDelete, null, order, isAsc).ToList().Select(x => new
            {
                Id = x.Id,
                Date = x.Date == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.Date.Value, false, null),
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                Beginning = x.BeginingAddress == null ? null : x.BeginingAddress.Address1,
                Destination = x.Destination == null ? null : x.DestinationAddress.Address1,
                FullName = x.DriverId == null ? null : x.Driver.Personnel.FullName,
                TaxiCompany = x.TaxiCompany == null ? null : x.TaxiCompany.Name,
                DriverType = x.DriverType == null ? null : x.DriverType.Name,
                PathName = x.Path == null ? x.CalcDistance.ToString() : x.Path.Name,
                Distance = x.CalcDistance,
                IsLocal = x.IsLocal,
                ServiceStatus = x.ServiceStatu.Name,
                IsAcceptTranasportManager = x.IsAcceptTranasportManager,
                Description = x.Description,
                RequestId = RequestRepository.GetByServiceId(x.Id) == null ? (int?)null : RequestRepository.GetByServiceId(x.Id).Id,
                RequestName = RequestRepository.GetByServiceId(x.Id) == null ? null : RequestRepository.GetByServiceId(x.Id).Personnel.FullName,
                RequestUnit = RequestRepository.GetByServiceId(x.Id) == null ? null : RequestRepository.GetByServiceId(x.Id).Personnel.FactoryUnit.Factory.Name + '-' + RequestRepository.GetByServiceId(x.Id).Personnel.FactoryUnit.Name,
                Requests = RequestRepository.GetRequestsByServiceId(x.Id).ToList().Select(y => new RequestViewInService { Id = y.Id, FullName = y.Personnel.FullName, Factory = y.Personnel.FactoryUnit.Factory.Name + "-" + y.Personnel.FactoryUnit.Name }).ToList(),
            }).ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=سرویس ها.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View();
        }
        public ActionResult Create(int? id)
        {
            ViewBag.DriverTypes = DriverTypeRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Pathes = PathRepository.All(null, false).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Eparches = EparchyRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.States = StateRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.TaxiCompanies = TaxiCompanyRepository.All(null, false).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.ServiceStatues = ServiceStatuRepository.All().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.ServiceTypes = ServiceTypeRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            var _serviceView = new ServiceSaveView();
            if (id != null)
            {
                var _service = ServiceRepository.GetById(id.Value);
                _serviceView.Id = _service.Id;
                _serviceView.DriverId = _service.DriverId;
                _serviceView.BiginningId = _service.Biginning;
                if (_service.Biginning != null)
                {
                    _serviceView.Biginning = new AddressSaveView()
                    {
                        Address = _service.BeginingAddress.Address1,
                        CityName = _service.BeginingAddress.CityName,
                        CountryName = _service.BeginingAddress.CountryName,
                        EparchyId = _service.BeginingAddress.EparchyId,
                        PostalCode = _service.BeginingAddress.PostalCode,
                        RegionName = _service.BeginingAddress.RegionName,
                        StateId = _service.BeginingAddress.Eparchy.StateId,
                    };
                }
                _serviceView.DestinationId = _service.Destination;
                if (_service.Destination != null)
                {
                    _serviceView.Destination = new AddressSaveView()
                    {
                        Address = _service.DestinationAddress.Address1,
                        CityName = _service.DestinationAddress.CityName,
                        CountryName = _service.DestinationAddress.CountryName,
                        EparchyId = _service.DestinationAddress.EparchyId,
                        PostalCode = _service.DestinationAddress.PostalCode,
                        RegionName = _service.DestinationAddress.RegionName,
                        StateId = _service.DestinationAddress.Eparchy.StateId,
                    };
                }
                _serviceView.Date = _service.Date == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_service.Date.Value, false, null);
                _serviceView.Distance = _service.CalcDistance;
                _serviceView.DistancePrice = _service.DistancePrice == null ? (decimal?)null : _service.DistancePrice.Price;
                _serviceView.PathId = _service.PathId;
                _serviceView.FullName = _service.Driver == null ? null : _service.Driver.Personnel == null ? null : _service.Driver.Personnel.FullName;
                _serviceView.DailyDriverId = _service.DailyDriverId;
                _serviceView.IsLocal = _service.IsLocal;
                _serviceView.GussetNumber = _service.GussetNumber;
                _serviceView.DriverName = _service.DriverName;
                _serviceView.DriverTypeId = _service.DriverTypeId;
                _serviceView.IsLocal = _service.IsLocal;
                _serviceView.ServiceStatusId = _service.ServiceStatusId;
                _serviceView.StartTime = _service.StartTime == null ? null : _service.StartTime.Value.ToString(@"hh\:mm");
                _serviceView.EndTime = _service.EndTime == null ? null : _service.EndTime.Value.ToString(@"hh\:mm");
                _serviceView.DelayTime = _service.DelayTime == null ? null : _service.DelayTime.Value.ToString(@"hh\:mm");
                _serviceView.TaxiCompanyId = _service.TaxiCompanyId;
                _serviceView.RealDistance = _service.RealDistance;
                _serviceView.Description = _service.Description;
                _serviceView.ServiceTypeId = _service.ServiceTypeId;
                _serviceView.RequestId = RequestRepository.GetById(_service.Id) == null ? (int?)null : RequestRepository.GetById(_service.Id).Id;
            }
            else
            {
                _serviceView.Date = _serviceView.Date == null ? Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(DateTime.Now, false, null) : _serviceView.Date;
                _serviceView.Biginning = new AddressSaveView();
                _serviceView.Destination = new AddressSaveView();

            }
            _serviceView.DelayTime = string.IsNullOrEmpty(_serviceView.DelayTime) == true ? "00:00" : _serviceView.DelayTime;
            return View(_serviceView);
        }
        [HttpPost]
        public ActionResult Create(ServiceSaveView model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                using (var tx = new TransactionScope())
                {
                    try
                    {
                        if (model.BiginningId == null && !string.IsNullOrEmpty(model.Biginning.Address))
                        {
                            var _beginningAddress = new Address()
                            {
                                Address1 = model.Biginning.Address,
                                CityName = model.Biginning.CityName,
                                CountryName = model.Biginning.CountryName,
                                EparchyId = model.Biginning.EparchyId,
                                PostalCode = model.Biginning.PostalCode,
                                RegionName = model.Biginning.RegionName,
                            };
                            model.BiginningId = AddressRepository.Add(_beginningAddress);
                        }
                        else if (model.BiginningId != null)
                        {
                            var _address = AddressRepository.GetById(model.BiginningId.Value);
                            _address.Address1 = model.Biginning.Address;
                            _address.CityName = model.Biginning.CityName;
                            _address.CountryName = model.Biginning.CountryName;
                            _address.EparchyId = model.Biginning.EparchyId.Value;
                            _address.PostalCode = model.Biginning.PostalCode;
                            _address.RegionName = model.Biginning.RegionName;
                            AddressRepository.Edit(_address);
                        }

                        if (model.DestinationId == null && !string.IsNullOrEmpty(model.Destination.Address))
                        {
                            var _destinationAddress = new Address()
                            {
                                Address1 = model.Destination.Address,
                                CityName = model.Destination.CityName,
                                CountryName = model.Destination.CountryName,
                                EparchyId = model.Destination.EparchyId.Value,
                                PostalCode = model.Destination.PostalCode,
                                RegionName = model.Destination.RegionName,
                            };
                            model.DestinationId = AddressRepository.Add(_destinationAddress);
                        }
                        else if (model.DestinationId != null)
                        {
                            var _address = AddressRepository.GetById(model.DestinationId.Value);
                            _address.Address1 = model.Destination.Address;
                            _address.CityName = model.Destination.CityName;
                            _address.CountryName = model.Destination.CountryName;
                            _address.EparchyId = model.Destination.EparchyId.Value;
                            _address.PostalCode = model.Destination.PostalCode;
                            _address.RegionName = model.Destination.RegionName;
                            AddressRepository.Edit(_address);
                        }
                        if (model.Id == 0)
                        {
                            var _service = new Service()
                            {
                                Destination = model.DestinationId,
                                Biginning = model.BiginningId,
                                Date = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.Date, 0),
                                DriverId = model.TaxiCompanyId == null ? model.DriverId : null,
                                DriverName = model.DriverName,
                                DriverTypeId = model.DriverTypeId,
                                IsLocal = model.IsLocal,
                                IsDelete = false,
                                IsLock = false,
                                EndTime = string.IsNullOrEmpty(model.EndTime) == true ? (TimeSpan?)null : TimeSpan.Parse(model.EndTime),
                                RealDistance = model.RealDistance,
                                CalcDistance = model.Distance,
                                PathId = model.PathId,
                                ServiceTypeId = model.ServiceTypeId,
                                GussetNumber = model.GussetNumber,
                                ServiceStatusId = model.ServiceStatusId,
                                StartTime = string.IsNullOrEmpty(model.StartTime) == true ? (TimeSpan?)null : TimeSpan.Parse(model.StartTime),
                                DelayTime = string.IsNullOrEmpty(model.DelayTime) == true ? (TimeSpan?)null : TimeSpan.Parse(model.DelayTime),
                                TaxiCompanyId = model.TaxiCompanyId,
                                TimeCreated = DateTime.Now,
                                Description = model.Description,
                                UserCreated = User.Identity.GetUserId(),

                            };

                            var _serviceId = ServiceRepository.Add(_service);
                            if (model.RequestId != null)
                            {
                                var _request = RequestRepository.GetById(model.RequestId.Value);
                                if (_request != null)
                                {
                                    _request.IsAcceptTranasport = (int)Enums.IsAcceptTransport.Accepted;
                                    _request.UserAcceptTranasport = User.Identity.GetUserId();
                                    _request.AcceptTranasportTime = DateTime.Now;
                                    _request.ServiceId = _serviceId;
                                    RequestRepository.Edit(_request);
                                }
                            }


                        }
                        else
                        {

                            var _service = ServiceRepository.GetById(model.Id);
                            if (_service != null && (_service.IsLock == false || User.IsInRole("Admin")))
                            {

                                _service.Date = Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.Date, 0);
                                _service.DriverId = model.TaxiCompanyId == null ? model.DriverId : null;
                                _service.DriverName = model.DriverName;
                                _service.DriverTypeId = model.DailyDriverId;
                                _service.EndTime = string.IsNullOrEmpty(model.EndTime) == true ? (TimeSpan?)null : TimeSpan.Parse(model.EndTime);
                                _service.StartTime = string.IsNullOrEmpty(model.StartTime) == true ? (TimeSpan?)null : TimeSpan.Parse(model.StartTime);
                                _service.TaxiCompanyId = model.TaxiCompanyId;
                                _service.RealDistance = model.RealDistance;
                                _service.CalcDistance = model.Distance;
                                _service.TimeEdited = DateTime.Now;
                                _service.DelayTime = string.IsNullOrEmpty(model.DelayTime) == true ? (TimeSpan?)null : TimeSpan.Parse(model.DelayTime);
                                _service.GussetNumber = model.GussetNumber;
                                _service.IsLocal = model.IsLocal;
                                _service.ServiceStatusId = model.ServiceStatusId;
                                _service.UserEdited = User.Identity.GetUserId();
                                _service.PathId = model.PathId;
                                _service.Description = model.Description;
                                _service.ServiceTypeId = model.ServiceTypeId;
                            }
                            ServiceRepository.Edit(_service);
                            var _request = RequestRepository.GetById(_service.Id);
                            if (_request != null)
                            {

                            }
                        }
                        tx.Complete();
                        if (model.BackRequest == false)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            //var index = Request.Params.ToString().IndexOf("ReturnUrl=");


                            //if (index >= 0)
                            //{
                            //    returnUrl = Request.Params.ToString().Substring(index + 10);
                            //    //Uri uri = new Uri(returnUrl);
                            //}
                            //       Uri uri = new Uri(returnUrl);

                            //////
                            var temp = HttpUtility.UrlDecode(ReturnUrl);
                            return Redirect(temp);
                            // return RedirectToAction("Index", "Request", new { area = "" });
                        }
                    }
                    catch (Exception e)
                    {
                        tx.Dispose();
                    }
                }
            }
            ViewBag.DriverTypes = DriverTypeRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Pathes = PathRepository.All(null, false).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Eparches = EparchyRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.States = StateRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.TaxiCompanies = TaxiCompanyRepository.All(null, false).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.ServiceStatues = ServiceStatuRepository.All().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.ServiceTypes = ServiceTypeRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.ReturnUrl = ReturnUrl;
            model.DelayTime = string.IsNullOrEmpty(model.DelayTime) == true ? "00:00" : model.DelayTime;

            return View(model);
        }
        //public ActionResult Delete(int id, string returnUrl)
        //{
        //    var _service = ServiceRepository.GetById(id);
        //    if (_service != null)
        //    {
        //        _service.IsDelete = false;
        //        _service.TimeEdited = DateTime.Now;
        //        _service.UserEdited = User.Identity.GetUserId();
        //    }
        //    return RedirectToRoute(returnUrl);
        //}
        public ActionResult ConvertRequestToService(int id)
        {
            var index = Request.QueryString.ToString().IndexOf("returnUrl=");

            var returnUrl = "";

            if (index >= 0)
            {
                returnUrl = Request.QueryString.ToString().Substring(index + 10);
                ViewBag.ReturnUrl = returnUrl;
                // ViewBag.ReturnUrl = HttpUtility.UrlDecode(returnUrl);
            }
            var _request = RequestRepository.GetById(id);

            if (_request != null)
            {
                if (_request.IsAcceptTranasport != (int)Enums.IsAcceptTransport.Accepted)
                {
                    _request.AcceptTranasportTime = DateTime.Now;
                    _request.UserAcceptTranasport = User.Identity.GetUserId();
                    _request.IsAcceptTranasport = (int)Enums.IsAcceptTransport.Accepted;
                    RequestRepository.Edit(_request);
                }
                var _serviceSave = new ServiceSaveView()
                {
                    Biginning = new AddressSaveView()
                    {
                        Address = _request.BeginingAddress.Address1,
                        CityName = _request.BeginingAddress.CityName,
                        CountryName = _request.BeginingAddress.CountryName,
                        EparchyId = _request.BeginingAddress.EparchyId.Value,
                        id = _request.BeginingAddress.Id,
                        PostalCode = _request.BeginingAddress.PostalCode,
                        RegionName = _request.BeginingAddress.RegionName,
                        StateId = _request.BeginingAddress.Eparchy.StateId,
                    },
                    Destination = new AddressSaveView()
                    {
                        Address = _request.DestinationAddress.Address1,
                        CityName = _request.DestinationAddress.CityName,
                        CountryName = _request.DestinationAddress.CountryName,
                        EparchyId = _request.DestinationAddress.EparchyId.Value,
                        id = _request.DestinationAddress.Id,
                        PostalCode = _request.DestinationAddress.PostalCode,
                        RegionName = _request.DestinationAddress.RegionName,
                        StateId = _request.DestinationAddress.Eparchy.StateId,
                    },
                    GussetNumber = _request.GussetNumber,
                    IsLocal = _request.IsLocal,
                    BiginningId = _request.BeginingAddress.Id,
                    Date = _request.ServiceDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_request.ServiceDate.Value, false, null),
                    DestinationId = _request.DestinationAddressId,
                    RequestId = _request.Id,
                    ServiceTypeId = _request.ServiceTypeId,
                    EndTime = _request.EndTime == null ? null : _request.EndTime.Value.ToString(@"hh\:mm"),
                    StartTime = _request.StartTime == null ? null : _request.StartTime.Value.ToString(@"hh\:mm"),
                    BackRequest = true,
                };
                ViewBag.DriverTypes = DriverTypeRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
                ViewBag.Pathes = PathRepository.All(null, false).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
                ViewBag.Eparches = EparchyRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
                ViewBag.States = StateRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
                ViewBag.TaxiCompanies = TaxiCompanyRepository.All(null, false).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                ViewBag.ServiceStatues = ServiceStatuRepository.All().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                ViewBag.ServiceTypes = ServiceTypeRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });

                return View("Create", _serviceSave);
            }

            return RedirectToRoute("~/Request/Index");

        }
        [HttpPost]
        public JsonResult GetAllServiceStatu()
        {
            var _serviceStatus = ServiceStatuRepository.All().Select(x => new { x.Id, x.Name }).ToList();
            return Json(_serviceStatus, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ChangeServiceStatus(int serviceStatusId, int[] services)
        {
            if (services != null && services.Count() > 0)
            {
                using (var tx = new TransactionScope())
                {
                    try
                    {
                        foreach (var item in services)
                        {

                            var _service = ServiceRepository.GetById(item);
                            if (_service.IsLock != true)
                            {
                                _service.ServiceStatusId = serviceStatusId;
                                _service.UserEdited = User.Identity.GetUserId();
                                _service.TimeEdited = DateTime.Now;
                                if (serviceStatusId == 3)
                                {
                                    _service.IsLock = true;
                                }
                                ServiceRepository.Edit(_service);
                            }
                        }
                        tx.Complete();
                        return Json(new { result = true, Message = "" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        tx.Dispose();
                        return Json(new { result = false, Message = "خطا در ذخیره سازی" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                return Json(new { result = false, Message = "لطفا درخواستی را انتخاب کنید" }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Admin,Transport,TranasportManager")]
        public JsonResult Confirm(int[] services, bool isAccept)
        {
            if (services != null && services.Count() > 0)
            {
                using (var tx = new TransactionScope())
                {
                    try
                    {
                        foreach (var item in services)
                        {
                            var _service = ServiceRepository.GetById(item);
                            _service.IsLock = true;
                            _service.IsAcceptTranasportManager = isAccept;
                            _service.UserAcceptTranasportManager = User.Identity.GetUserId();
                            _service.TimeAcceptTranasportManager = DateTime.Now;

                            ServiceRepository.Edit(_service);
                        }
                        tx.Complete();
                        return Json(new { result = true, Message = "" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        tx.Dispose();
                        return Json(new { result = false, Message = "خطا در ذخیره سازی" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                return Json(new { result = false, Message = "لطفا درخواستی را انتخاب کنید" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult Delete(int id)
        {
            var _service = ServiceRepository.GetById(id);
            if (_service.IsLock == false)
            {
                using (var tx = new TransactionScope())
                {
                    try
                    {
                        var _request = RequestRepository.GetByServiceId(_service.Id);
                        if (_request != null)
                        {
                            _request.ServiceId = null;
                            RequestRepository.Edit(_request);
                        }
                        _service.IsDelete = !_service.IsDelete;
                        _service.TimeEdited = DateTime.Now;
                        _service.UserEdited = User.Identity.GetUserId();
                        ServiceRepository.Edit(_service);
                        tx.Complete();
                        return Json(true);
                    }
                    catch
                    {
                        tx.Dispose();
                    }
                }
            }
            return Json(false);
        }
        public JsonResult GetDriverType(int driverId)
        {
            var _driverTypeId = RDriverDriverTypeRepository.GetByDriverId(driverId, true, true, false) == null ? (int?)null : RDriverDriverTypeRepository.GetByDriverId(driverId, true, true, false).DriverTypeId;
            return Json(_driverTypeId);
        }
        [HttpPost]
        public JsonResult GetPathAndWay()
        {
            var _waies = WayRepository.All(null, null).GroupBy(x => x.Path).Select(y => new
            {
                id = y.Key.Name,
                way = y.Select(x => x.Name).ToList(),

            }).OrderBy(x => x.id).ToList();
            return Json(_waies);
        }

    }
}