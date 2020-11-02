using Domain.Entity;
using Domain.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Transport.Areas.Manage.Models;
using Transport.Hubs;
using Transport.Infrastructure;
using Transport.Models;

namespace Transport.Models.Controllers
{
    public static class ExtensionMethods
    {
        // returns the number of milliseconds since Jan 1, 1970 (useful for converting C# dates to JS dates)
        public static double UnixTicks(this DateTime dt)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = dt.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return ts.TotalMilliseconds;
        }
    }
    [System.Web.Mvc.Authorize]
    public class RequestController : Controller
    {
        public RequestController()
        {


        }
        public RequestController(ApplicationUserManager userManager)
        {
            UserManager = userManager;

        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public RequestRepository RequestRepository = new RequestRepository();
        public EparchyRepository EparchyRepository = new EparchyRepository();
        private StateRepository StateRepository = new StateRepository();
        private AddressRepository AddressRepository = new AddressRepository();
        private PersonnelRepository PersonnelRepository = new PersonnelRepository();
        private FactoryUserManagerRepository FactoryUserManagerRepository = new FactoryUserManagerRepository();
        private DriverTypeRepository DriverTypeRepository = new DriverTypeRepository();
        private PathRepository PathRepository = new PathRepository();
        private TaxiCompanyRepository TaxiCompanyRepository = new TaxiCompanyRepository();
        private ServiceRepository ServiceRepository = new ServiceRepository();
        private ServiceTypeRepository ServiceTypeRepository = new ServiceTypeRepository();
        private ManagerPersonelRepository ManagerPersonelRepository = new ManagerPersonelRepository();
        private DailyTimeRepository DailyTimeRepository = new DailyTimeRepository();
        public ActionResult Index(string startDate, string endDate, string startTime, string endTime, bool? workTime, bool? today = true)
        {
            if (today == true || today == null)
            {
                var _dailyTime = DailyTimeRepository.GetByDate(DateTime.Now);
                startTime = _dailyTime.StartTime.Value.ToString(@"hh\:mm");
                endTime = _dailyTime.EndTime.Value.ToString(@"hh\:mm");
                ViewBag.StartTime = startTime;
                ViewBag.EndTime = endTime;
            }
            return View();
        }
        [AllowCrossSiteJson]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult List(string startDate, string endDate, string startTime, string endTime, int[] managerStatus, int[] transportStatus, bool? isDelete, bool? isCancel, int pageNumber = 0, int pageSize = 20, int? RequestPersonel = null, Enums.OrderEnumRequest order = Enums.OrderEnumRequest.RequestDate, bool isAsc = true)
        {
            var _userId = User.Identity.GetUserId();
            var _user = UserManager.Users.FirstOrDefault(x => x.Id == _userId);
            var _personel = PersonnelRepository.GetById(_user.PersonnelId.Value);
            var _startDate = string.IsNullOrEmpty(startDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(startDate, 0);
            var _endDate = string.IsNullOrEmpty(endDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(endDate, 0);
            var _managerStatus = (managerStatus == null ? Enumerable.Empty<int>() : managerStatus.Select(x => x));
            var _transportStatus = (transportStatus == null ? Enumerable.Empty<int>() : transportStatus.Select(x => x));
            int _totalCount = 0;
            var _startTime = string.IsNullOrEmpty(startTime) == true ? (TimeSpan?)null : TimeSpan.Parse(startTime);
            var _endTime = string.IsNullOrEmpty(endTime) == true ? (TimeSpan?)null : TimeSpan.Parse(endTime);
            var _Requests = new List<Request>();
            if (User.IsInRole("Admin") || User.IsInRole("Transport") || User.IsInRole("TranasportManager"))
            {
                _Requests = RequestRepository.TrnasportRequests(_personel.Id, _startDate, _endDate, _startTime, _endTime, isDelete, isCancel, _transportStatus, _managerStatus, RequestPersonel, null, order, isAsc).Skip((pageNumber) * pageSize).Take(pageSize).ToList();
                _totalCount = RequestRepository.TrnasportRequests(_personel.Id, _startDate, _endDate, _startTime, _endTime, isDelete, isCancel, _transportStatus, _managerStatus, RequestPersonel, null, order, isAsc).Count();
            }
            else if (User.IsInRole("FactorManager"))
            {
                _Requests = RequestRepository.ManagerRequests(_personel.Id, _startDate, _endDate, _startTime, _endTime, isDelete, isCancel, _transportStatus, _managerStatus, RequestPersonel, null, order, isAsc).Skip((pageNumber) * pageSize).Take(pageSize).ToList();
                _totalCount = RequestRepository.ManagerRequests(_personel.Id, _startDate, _endDate, _startTime, _endTime, isDelete, isCancel, _transportStatus, _managerStatus, RequestPersonel, null, order, isAsc).Count();
            }
            else
            {
                _Requests = RequestRepository.UserRequests(_personel.Id, _startDate, _endDate, _startTime, _endTime, isDelete, isCancel, _transportStatus, _managerStatus, RequestPersonel, null, order, isAsc).Skip((pageNumber) * pageSize).Take(pageSize).ToList();
                _totalCount = RequestRepository.UserRequests(_personel.Id, _startDate, _endDate, _startTime, _endTime, isDelete, isCancel, _transportStatus, _managerStatus, RequestPersonel, null, order, isAsc).Count();
            }

            if (_Requests.Count > 0)
            {
                var _requestView = _Requests.ToList().Select(x => new
                {
                    StartTime = x.StartTime == null ? null : x.StartTime.Value.ToString(@"hh\:mm"),
                    EndTime = x.EndTime,
                    IsLock = x.IsLock,
                    IsLocal = x.IsLocal,
                    IsAcceptTranasport = x.IsAcceptTranasport,
                    IsAcceptManager = x.IsAcceptManager,
                    Biginning = x.BeginingAddress == null ? null : x.BeginingAddress.Eparchy.Name + " " + x.BeginingAddress.Address1,
                    Destination = x.DestinationAddress == null ? null : x.DestinationAddress.Eparchy.Name + " " + x.DestinationAddress.Address1,
                    IsEmergancy = x.IsEmergancy,
                    PersonelName = x.Personnel.FullName+ "-" +x.Personnel.PersonnelCode,
                    Id = x.Id,
                    RequestDate = x.RequestDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDateTime(x.RequestDate.Value, false),
                    ServiceDate = x.ServiceDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.ServiceDate.Value, false, null),
                    ServiceId = x.ServiceId,
                    IsCanceled = x.IsCanceled,
                    IsDelete = x.IsDelete,
                    Editable = Editable(x),
                    Cancelable = Cancelable(x),
                    Deleteable = Deleteable(x),
                }).ToList();
                var _totalPages = (int)Math.Ceiling((float)_totalCount / (float)pageNumber);
                return Json(new { Request = _requestView, TotalCount = _totalCount, Page = pageNumber, TotalPages = _totalPages }, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        public ActionResult ExportData(string startDate, string endDate, string startTime, string endTime, int[] managerStatus, int[] transportStatus, bool? isDelete, bool? isCancel, int? RequestPersonel = null, Enums.OrderEnumRequest order = Enums.OrderEnumRequest.ServiceDate, bool isAsc = true)
        {
            var _userId = User.Identity.GetUserId();
            var _user = UserManager.Users.FirstOrDefault(x => x.Id == _userId);
            var _personel = PersonnelRepository.GetById(_user.PersonnelId.Value);
            var _startDate = string.IsNullOrEmpty(startDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(startDate, 0);
            var _endDate = string.IsNullOrEmpty(endDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(endDate, 0);
            var _managerStatus = (managerStatus == null ? Enumerable.Empty<int>() : managerStatus.Select(x => x));
            var _transportStatus = (transportStatus == null ? Enumerable.Empty<int>() : transportStatus.Select(x => x));
            var _startTime = string.IsNullOrEmpty(startTime) == true ? (TimeSpan?)null : TimeSpan.Parse(startTime);
            var _endTime = string.IsNullOrEmpty(endTime) == true ? (TimeSpan?)null : TimeSpan.Parse(endTime);
            var _Requests = new List<Request>();
            if (User.IsInRole("Admin") || User.IsInRole("Transport"))
            {
                _Requests = RequestRepository.TrnasportRequests(_personel.Id, _startDate, _endDate, _startTime, _endTime, isDelete, isCancel, _transportStatus, _managerStatus, RequestPersonel, null, order, isAsc).ToList();

            }
            else if (User.IsInRole("FactorManager"))
            {
                _Requests = RequestRepository.ManagerRequests(_personel.Id, _startDate, _endDate, null, null, isDelete, isCancel, _transportStatus, _managerStatus, RequestPersonel, null, order, isAsc).ToList();

            }
            else
            {
                _Requests = RequestRepository.UserRequests(_personel.Id, _startDate, _endDate, null, null, isDelete, isCancel, _transportStatus, _managerStatus, RequestPersonel, null, order, isAsc).ToList();

            }

            GridView gv = new GridView();
            if (_Requests.Count < 0) { return View(); }
            else
            {
                gv.DataSource = _Requests.Select(x => new
                {
                    StartTime = x.StartTime == null ? null : x.StartTime.Value.ToString(@"hh\:mm"),
                    EndTime = x.EndTime,
                    IsLock = x.IsLock,
                    IsLocal = x.IsLocal,
                    IsAcceptTranasport = x.IsAcceptTranasport,
                    IsAcceptManager = x.IsAcceptManager,
                    Biginning = x.BeginingAddress.Address1,
                    Destination = x.DestinationAddress.Address1,
                    IsEmergancy = x.IsEmergancy,
                    PersonelName = x.Personnel.FullName,
                    Id = x.Id,
                    RequestDate = x.RequestDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.RequestDate.Value, false, null),
                    ServiceDate = x.ServiceDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.ServiceDate.Value, false, null),
                    ServiceId = x.ServiceId,
                    IsCanceled = x.IsCanceled,
                    IsDelete = x.IsDelete,

                }).ToList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=درخواست.xls");
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
        }
        public ActionResult Create(int? id,bool? isLocal)
        {

            ViewBag.States = StateRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Eparchies = EparchyRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, });
            ViewBag.ServiceTypes = ServiceTypeRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            var _RequestView = GetRequest(id);
            if (id == null)
            {
                _RequestView.IsLocal = (isLocal == false ? false : true);
            }
            return View(_RequestView);
        }
        [AllowCrossSiteJson]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult CreateRequestMobile(int? id)
        {
            var _RequestView = GetRequest(id);
            DateTime TimeTo = DateTime.Now;
            DateTime TimeFrom = TimeTo.AddMinutes(10);
            if (id == null)
            {
                _RequestView.IsMobile = true;
                _RequestView.IsLocal = true;
                _RequestView.IsEmergancy = false;

                _RequestView.StartTimeStr = ExtensionMethods.UnixTicks((new DateTime(TimeFrom.Year, TimeFrom.Month, TimeFrom.Day, TimeFrom.Hour, TimeFrom.Minute, TimeFrom.Second))).ToString();
            }
            else
            {
                _RequestView.IsMobile = true;
                _RequestView.StartTimeStr = _RequestView.StartTime == null ? null : ExtensionMethods.UnixTicks((new DateTime(TimeTo.Year, TimeTo.Month, TimeTo.Day, _RequestView.StartTime.Value.Hours, _RequestView.StartTime.Value.Minutes, _RequestView.StartTime.Value.Seconds))).ToString();
            }
            return Json(_RequestView, JsonRequestBehavior.AllowGet);
        }

        private RequestSave GetRequest(int? id)
        {
            var _RequestView = new RequestSave();
            if (id != null)
            {

                var _Request = RequestRepository.GetById(id.Value);
                if (_Request != null)
                {
                    var BeginnigAddress = new AddressSaveView()
                    {
                        Address = _Request.BeginingAddress.Address1,
                        CityName = _Request.BeginingAddress.CityName,
                        CountryName = _Request.BeginingAddress.CountryName,
                        EparchyId = _Request.BeginingAddress.EparchyId.Value,
                        id = _Request.BeginingAddress.Id,
                        PostalCode = _Request.BeginingAddress.PostalCode,
                        RegionName = _Request.BeginingAddress.RegionName,
                        StateId = _Request.BeginingAddress.Eparchy.StateId,
                    };
                    var DestinationAddress = new AddressSaveView()
                    {
                        Address = _Request.DestinationAddress.Address1,
                        CityName = _Request.DestinationAddress.CityName,
                        CountryName = _Request.DestinationAddress.CountryName,
                        EparchyId = _Request.DestinationAddress.EparchyId.Value,
                        id = _Request.DestinationAddress.Id,
                        PostalCode = _Request.DestinationAddress.PostalCode,
                        RegionName = _Request.DestinationAddress.RegionName,
                        StateId = _Request.DestinationAddress.Eparchy.StateId,
                    };

                    _RequestView.Id = _Request.Id;
                    _RequestView.IsLocal = _Request.IsLocal;
                    _RequestView.StartTime = _Request.StartTime;
                    _RequestView.EndTime = _Request.EndTime;
                    _RequestView.BiginningAddress = BeginnigAddress;
                    _RequestView.Description = _Request.Description;
                    _RequestView.DestinationAddress = DestinationAddress;
                    _RequestView.IsEmergancy = _Request.IsEmergancy;
                    _RequestView.ServiceDate = _Request.ServiceDate == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_Request.ServiceDate.Value, false, null);
                    _RequestView.PersonelId = _Request.PersonelId;
                    _RequestView.FullName = _Request.Personnel.FullName;
                    _RequestView.ServiceId = _Request.ServiceId;
                    _RequestView.ServiceTypeId = _Request.ServiceTypeId;
                    _RequestView.GussetNumber = _Request.GussetNumber;
                    _RequestView.Editable = Editable(_Request);
                }


            }
            else
            {
                var _userId = User.Identity.GetUserId();
                _RequestView.Editable = true;
                var _user = UserManager.Users.FirstOrDefault(x => x.Id == _userId);
                var _personnel = PersonnelRepository.GetById(_user.PersonnelId.Value);
                _RequestView.PersonelId = _personnel.Id;
                _RequestView.FullName = _personnel.FullName;
                _RequestView.ServiceDate = Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(DateTime.Now, false, null);
                _RequestView.BiginningAddress = new AddressSaveView();
                _RequestView.DestinationAddress = new AddressSaveView();

            }
            return _RequestView;
        }
        public bool Editable(Request request)
        {
            var _user = UserManager.FindById(User.Identity.GetUserId());
            var _managers = ManagerPersonelRepository.GetManagerOfPersonnel(_user.PersonnelId.Value);
            if (request.PersonelId == _user.PersonnelId || _managers.Any(x => x.ManagerPersonelId == _user.PersonnelId))
            {
                if (!(request.IsAcceptTranasport == (int)Enums.IsAcceptTransport.Accepted ||
                    (request.IsAcceptManager == (int)Enums.IsAcceptManager.Accepted && request.IsLocal == false)) ||
                    (request.IsAcceptManager == (int)Enums.IsAcceptManager.Accepted && request.IsLocal == true && request.IsAcceptTranasport != (int)Enums.IsAcceptTransport.Accepted))
                {
                    return true;
                }
            }
            return false;
        }

        [HttpPost]
        public ActionResult Create(RequestSave model)
        {
            var _userId = User.Identity.GetUserId();
            var _user = UserManager.Users.FirstOrDefault(x => x.Id == _userId);
            var _personnel = PersonnelRepository.GetById(_user.PersonnelId.Value);
            bool _iseditable = true;
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    using (var tx = new TransactionScope())
                    {
                        try
                        {


                            var _beginningAddress = new Address()
                            {
                                Address1 = model.BiginningAddress.Address,
                                CityName = model.BiginningAddress.CityName,
                                CountryName = model.BiginningAddress.CountryName,
                                EparchyId = model.BiginningAddress.EparchyId,
                                PostalCode = model.BiginningAddress.PostalCode,
                                RegionName = model.BiginningAddress.RegionName,

                            };

                            var _destinationAddress = new Address()
                            {
                                Address1 = model.DestinationAddress.Address,
                                CityName = model.DestinationAddress.CityName,
                                CountryName = model.DestinationAddress.CountryName,
                                EparchyId = model.DestinationAddress.EparchyId,
                                PostalCode = model.DestinationAddress.PostalCode,
                                RegionName = model.DestinationAddress.RegionName,
                            };



                            var _Request = new Request()
                            {

                                BeginingAddress = _beginningAddress,
                                DestinationAddress = _destinationAddress,
                                Description = model.Description,
                                EndTime = model.EndTime,
                                IsLocal = model.IsLocal,
                                IsEmergancy = model.IsEmergancy,
                                IsCanceled = false,
                                RequestDate = DateTime.Now,
                                ServiceDate = string.IsNullOrEmpty(model.ServiceDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.ServiceDate, 0),
                                StartTime = model.StartTime,
                                PersonelId = model.PersonelId,
                                ServiceTypeId = model.ServiceTypeId,
                                GussetNumber = model.GussetNumber,
                                UserCreated = User.Identity.GetUserId(),
                                TimeCreated = DateTime.Now,
                                IsAcceptManager = model.IsLocal == true ? (int)Enums.IsAcceptManager.Accepted : (int)Enums.IsAcceptManager.NotChecked,
                                UserAcceptManager = model.IsLocal == true ? User.Identity.GetUserId() : null,
                                AcceptManagerTime = model.IsLocal == true ? DateTime.Now : (DateTime?)null,
                            };

                            var id = RequestRepository.Add(_Request);

                            var hubContext = GlobalHost.ConnectionManager.GetHubContext<RequestHub>();
                            hubContext.Clients.All.addNewMessageToPage(RequestHubView(id));
                            tx.Complete();
                            if (model.IsMobile == false)
                            {
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                return Json(new { success = true });
                            }
                        }

                        catch (DbEntityValidationException ex)
                        {
                            tx.Dispose();
                            ModelState.AddModelError("", "خطا در ذخیره سازی");

                        }
                    }
                }
                else
                {
                    using (var tx = new TransactionScope())
                    {
                        try
                        {
                            var _Request = RequestRepository.GetById(model.Id);
                            var _managers = ManagerPersonelRepository.GetManagerOfPersonnel(_user.PersonnelId.Value);
                            _iseditable = Editable(_Request);
                            if (_Request.PersonelId == _user.PersonnelId || _managers.Any(x => x.ManagerPersonelId == _user.PersonnelId))
                            {
                                if (!(_Request.IsAcceptTranasport == (int)Enums.IsAcceptTransport.Accepted || (_Request.IsAcceptManager == (int)Enums.IsAcceptManager.Accepted && _Request.IsLocal == false)) || (_Request.IsAcceptManager == (int)Enums.IsAcceptManager.Accepted && _Request.IsLocal == true && _Request.IsAcceptTranasport != (int)Enums.IsAcceptTransport.Accepted))
                                {
                                    _Request.BeginingAddress.Address1 = model.BiginningAddress.Address;
                                    _Request.BeginingAddress.CityName = model.BiginningAddress.CityName;
                                    _Request.BeginingAddress.CountryName = model.BiginningAddress.CountryName;
                                    _Request.BeginingAddress.EparchyId = model.BiginningAddress.EparchyId;
                                    _Request.BeginingAddress.PostalCode = model.BiginningAddress.PostalCode;
                                    _Request.BeginingAddress.RegionName = model.BiginningAddress.RegionName;
                                    _Request.DestinationAddress.Address1 = model.DestinationAddress.Address;
                                    _Request.DestinationAddress.CityName = model.DestinationAddress.CityName;
                                    _Request.DestinationAddress.CountryName = model.DestinationAddress.CountryName;
                                    _Request.DestinationAddress.EparchyId = model.DestinationAddress.EparchyId;
                                    _Request.DestinationAddress.PostalCode = model.DestinationAddress.PostalCode;
                                    _Request.DestinationAddress.RegionName = model.DestinationAddress.RegionName;
                                    _Request.Description = model.Description;
                                    if (_Request.IsLocal == true && model.IsLocal == false)
                                    {
                                        _Request.IsAcceptManager = (int)Enums.IsAcceptManager.NotChecked;
                                        _Request.UserAcceptManager = null;
                                        _Request.AcceptManagerTime = null;
                                    }
                                    else if (_Request.IsLocal == false && model.IsLocal == true)
                                    {
                                        _Request.IsAcceptManager = (int)Enums.IsAcceptManager.Accepted;
                                        _Request.UserAcceptManager = User.Identity.GetUserId();
                                        _Request.AcceptManagerTime = DateTime.Now;
                                    }
                                    _Request.ServiceTypeId = model.ServiceTypeId;
                                    _Request.IsLocal = model.IsLocal;
                                    _Request.GussetNumber = model.GussetNumber;
                                    _Request.EndTime = model.EndTime;
                                    _Request.IsEmergancy = model.IsEmergancy;
                                    _Request.RequestDate = DateTime.Now;
                                    _Request.ServiceDate = string.IsNullOrEmpty(model.ServiceDate) == true ? (DateTime?)null : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(model.ServiceDate, 0);
                                    _Request.StartTime = model.StartTime;
                                    _Request.TimeEdited = System.DateTime.Now;
                                    _Request.UserEdited = User.Identity.GetUserId();

                                    RequestRepository.Edit(_Request);
                                    tx.Complete();
                                    if (model.IsMobile == false)
                                    {
                                        return RedirectToAction("Index");
                                    }
                                    else
                                    {
                                        return Json(new { success = true });
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("", "درخواست تایید شده قابل ویرایش نمیباشد");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "فرد ویرایش کننده با درخواست کننده یا ایجاد کننده یکسان نمیباشد");
                            }

                        }
                        catch (Exception e)
                        {
                            tx.Dispose();
                            ModelState.AddModelError("", "خطا در ذخیره سازی");

                        }
                    }
                }

            }
            model.Editable = model.Id == 0 ? true : _iseditable;
            model.PersonelId = _personnel.Id;
            model.FullName = _personnel.FullName;
            if (model.IsMobile == false)
            {

                ViewBag.Eparchies = EparchyRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
                ViewBag.States = StateRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
                ViewBag.ServiceTypes = ServiceTypeRepository.All().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
                return View(model);
            }
            else
            {
                return Json(new
                {
                    success = false,
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                       .Select(m => m.ErrorMessage).ToArray(),
                });
            }

        }
        public RequestHubView RequestHubView(int? requestId)
        {
            if (requestId != null)
            {
                var _request = RequestRepository.GetById(requestId.Value);
                if (_request != null)
                {
                    var _requesthub = new RequestHubView()
                    {
                        FullName = _request.Personnel.FullName,
                        StartTime = _request.StartTime == null ? null : _request.StartTime.Value.ToString(@"hh\:mm"),
                        IsLocal = _request.IsLocal,
                    };
                    return _requesthub;
                }
                else { return null; }
            }
            else { return null; }

        }
        [System.Web.Mvc.Authorize(Roles = "Admin,Transport,FactorManager")]
        public JsonResult Confirm(int[] requests, bool isAccept)
        {
            var _isAccept = isAccept == true ? (int)Enums.IsAcceptManager.Accepted : (int)Enums.IsAcceptManager.NotAccepted;
            if (requests != null && requests.Count() > 0)
            {
                using (var tx = new TransactionScope())
                {
                    try
                    {
                        foreach (var item in requests)
                        {
                            var _request = RequestRepository.GetById(item);
                            if (User.IsInRole("Admin") || User.IsInRole("Transport"))
                            {
                                if (_request.ServiceId == null || _request.Service.IsLock != true || _request.IsAcceptTranasport == null)
                                {
                                    _request.IsAcceptTranasport = _isAccept;
                                    _request.UserAcceptTranasport = User.Identity.GetUserId();
                                    _request.AcceptTranasportTime = DateTime.Now;
                                    RequestRepository.Edit(_request);
                                }
                                else
                                {
                                    tx.Dispose();
                                    return Json(new { result = false, Message = "به این در خواست سرویس متصل است قابل تغییر نمیباشد" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            if (User.IsInRole("FactorManager"))
                            {
                                if (_request.IsAcceptManager != (int)Enums.IsAcceptManager.Accepted || _request.AcceptManagerTime.Value.AddDays(-2) < DateTime.Now)
                                {
                                    var _managers = ManagerPersonelRepository.GetManagerOfPersonnel(_request.PersonelId);
                                    var _user = UserManager.FindById(User.Identity.GetUserId());
                                    if (_managers.Any(x => x.ManagerPersonelId == _user.PersonnelId))
                                    {
                                        _request.IsAcceptManager = _isAccept;
                                        _request.UserAcceptManager = User.Identity.GetUserId();
                                        _request.AcceptManagerTime = DateTime.Now;
                                        RequestRepository.Edit(_request);
                                    }
                                    else
                                    {
                                        tx.Dispose();
                                        return Json(new { result = false, Message = "دسترسی برای تایید این در خواست ندارید" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                else
                                {
                                    tx.Dispose();
                                    return Json(new { result = false, Message = "به این در خواست سرویس متصل است قابل تغییر نمیباشد" }, JsonRequestBehavior.AllowGet);
                                }
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
        public bool Deleteable(Request request)
        {
            var _userId = User.Identity.GetUserId();
            if (_userId == request.UserCreated)
            {
                if (request.IsLock != true && request.IsDelete == false && request.IsAcceptManager != (int)Enums.IsAcceptManager.Accepted && request.IsAcceptTranasport != (int)Enums.IsAcceptTransport.Accepted)
                {
                    return true;
                }
            }
            return false;
        }
        [AllowCrossSiteJson]
        [AllowAnonymous]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public ActionResult Delete(int id, bool isMobile = false)
        {
            var _request = RequestRepository.GetById(id);
            var _isdelete = false;
            if (Deleteable(_request))
            {
                _request.IsDelete = true;
                _request.TimeEdited = DateTime.Now;
                _request.UserEdited = User.Identity.GetUserId();
                RequestRepository.Edit(_request);
                _isdelete = true;

            }
            if (!isMobile)
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (_isdelete)
                    return Json(true);
                else
                    return Json(false);
            }
        }
        public bool Cancelable(Request request)
        {
            var _userId = User.Identity.GetUserId();
            if (_userId == request.UserCreated)
            {
                if ((request.ServiceId == null || !request.Service.IsLock) && request.IsCanceled == false)
                {
                    return true;
                }
            }
            return false;
        }
        public JsonResult Cancel(int id)
        {
            var _user = User.Identity.GetUserId();
            var _request = RequestRepository.GetById(id);
            if (_user == _request.UserCreated)
            {
                if (_request != null)
                {
                    if (Cancelable(_request))
                    {
                        _request.IsCanceled = true;
                        _request.TimeEdited = DateTime.Now;
                        _request.UserEdited = User.Identity.GetUserId();
                        RequestRepository.Edit(_request);
                        return Json(new { success = true });
                    }
                }
                return Json(new { success = false, message = "به این درخواست سرویسی قفل شده متصل است" });
            }
            else
            {
                return Json(new { success = false, message = "فرد درخواست دهنده با فرد تغییر دهنده متقاوت می باشد" });
            }
        }
        public JsonResult GetPersonById(int id)
        {
            var _person = PersonnelRepository.GetById(id);
            if (_person != null)
            {
                return Json(_person.FullName, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }
        [AllowCrossSiteJson]
        [AllowAnonymous]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult GetCreateRequestData()
        {
            var _states = StateRepository.All().Select(x => new { Value = x.Id, Text = x.Name });
            var _eparchies = EparchyRepository.All().Select(x => new { Value = x.Id, Text = x.Name, StateId = x.StateId });
            var _serviceTypes = ServiceTypeRepository.All().Select(x => new { Value = x.Id, Text = x.Name });
            return Json(new { States = _states, Eparchies = _eparchies, ServiceTypes = _serviceTypes }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.Authorize(Roles = "Admin,Transport")]
        public JsonResult GetActiveServices(string date)
        {
            IEnumerable<int> _serviceStatus = new List<int> { (int)Enums.ServiceStatus.NotStart, (int)Enums.ServiceStatus.Servicing };
            var _date = string.IsNullOrEmpty(date) ? DateTime.Now : Tavanmand.Classes.PersianCalendarConverter.FarsiDateToMiladi(date, 0);
            var _services = ServiceRepository.All(null, _serviceStatus, _date, _date,null,null, null, false, false).ToList().Select(x => new
            {
                Id = x.Id,
                FullName = x.DriverId == null ? x.TaxiCompany.Name : x.Driver.Personnel.FullName,
                StartTime = x.StartTime == null ? null : x.StartTime.Value.ToString(@"hh\:mm"),
                Date = x.Date == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.Date.Value, false, null),
                BeginingAddress = x.BeginingAddress == null ? null : x.BeginingAddress.Eparchy.Name + " - " + x.BeginingAddress.Address1,
                DestinationAddress = x.DestinationAddress == null ? null : x.DestinationAddress.Eparchy.Name + " - " + x.DestinationAddress.Address1,
                ServiceStatu = x.ServiceStatu == null ? null : x.ServiceStatu.Name,
                Requests = RequestRepository.GetRequestsByServiceId(x.Id).ToList().Select(y => new RequestViewInService
                {
                    Id = y.Id,
                    FullName = y.Personnel.FullName,
                    Factory = y.Personnel.FactoryUnit.Factory.Name + "-" + y.Personnel.FactoryUnit.Name,
                    Number = y.ServiceTypeId == (int)Enums.ServiceType.Services ? 0 : y.GussetNumber == null ? 1 : y.GussetNumber + 1
                }).ToList(),
            });
            if (_services.Count() > 0)
            {
                return Json(_services);
            }
            else
            {
                return Json(null);
            }
        }
        [System.Web.Mvc.Authorize(Roles = "Admin,Transport")]
        public JsonResult GetActiveServiceById(int id)
        {
            var _service = ServiceRepository.GetById(id);
            if (_service != null)
            {
                return Json(new
                {
                    Id = _service.Id,
                    FullName = _service.DriverId == null ? _service.TaxiCompany.Name : _service.Driver.Personnel.FullName,
                    StartTime = _service.StartTime == null ? null : _service.StartTime.Value.ToString(@"hh\:mm"),
                    Date = _service.Date == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(_service.Date.Value, false, null)
                });
            }
            else { return Json(null); }
        }
        [System.Web.Mvc.Authorize(Roles = "Admin,Transport")]
        public JsonResult RemoveService(int requestId)
        {
            var _request = RequestRepository.GetById(requestId);
            if (_request != null)
            {
                if (_request.Service != null)
                {
                    if (_request.Service.IsLock == false || User.IsInRole("Admin"))
                    {
                        _request.ServiceId = null;
                        RequestRepository.Edit(_request);
                        return Json(new { Result = true, Message = "حذف انجام شد" });
                    }
                    else
                    {
                        return Json(new { Result = false, Message = "سرویس قفل شده است و قابل حذف نمیباشد" });
                    }
                }
                else return Json(new { Result = false, Message = "به این درخواست سرویسی متصل نمی باشد" });
            }
            else
            {
                return Json(new { Result = false, Message = "درخواستی یافت نشد" });
            }
        }
        [System.Web.Mvc.Authorize(Roles = "Admin,Transport")]
        public JsonResult AutoService(string searchStr)
        {
            var services = ServiceRepository.GetServiceByAutoId(searchStr).ToList().Select(x => new
            {
                Id = x.Id,
                FullName = x.DriverId == null ? x.TaxiCompany.Name : x.Driver.Personnel.FullName,
                StartTime = x.StartTime == null ? null : x.StartTime.Value.ToString(@"hh\:mm"),
                Date = x.Date == null ? null : Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(x.Date.Value, false, null),
                BeginingAddress = x.BeginingAddress == null ? null : x.BeginingAddress.Eparchy.Name + " - " + x.BeginingAddress.Address1,
                DestinationAddress = x.DestinationAddress == null ? null : x.DestinationAddress.Eparchy.Name + " - " + x.DestinationAddress.Address1,
                ServiceStatu = x.ServiceStatu == null ? null : x.ServiceStatu.Name,

                Requests = RequestRepository.GetRequestsByServiceId(x.Id).ToList().Select(y => new RequestViewInService
                {
                    Id = y.Id,
                    FullName = y.Personnel.FullName,
                    Factory = y.Personnel.FactoryUnit.Factory.Name + "-" + y.Personnel.FactoryUnit.Name,
                    Number = y.ServiceTypeId == (int)Enums.ServiceType.Services ? 0 : y.GussetNumber == null ? 1 : y.GussetNumber + 1
                }).ToList(),
            }).ToList();
            return Json(services, JsonRequestBehavior.AllowGet);
        }
        [System.Web.Mvc.Authorize(Roles = "Admin,Transport")]
        [HttpPost]
        public JsonResult AddServiceToRequest(int? requestId, int? serviceId)
        {
            var _request = RequestRepository.GetById(requestId.Value);
            if (_request != null && _request.ServiceId == null)
            {
                var _service = ServiceRepository.GetById(serviceId.Value);
                if (_service != null && (_service.IsLock == false || User.IsInRole("Admin")))
                {
                    _request.ServiceId = _service.Id;
                    _request.AcceptTranasportTime = DateTime.Now;
                    _request.IsAcceptTranasport = (int)Enums.IsAcceptTransport.Accepted;
                    _request.UserAcceptTranasport = User.Identity.GetUserId();
                    _request.TimeEdited = DateTime.Now;
                    _request.UserEdited = User.Identity.GetUserId();
                    RequestRepository.Edit(_request);
                    return Json(new { Result = true, Message = "حذف انجام شد" });
                }
                else
                {
                    return Json(new { Result = false, Message = "این سرویس غیر مجاز می باشد" });
                }
            }
            else return Json(new { Result = false, Message = "به این درخواست سرویسی متصل می باشد" });
        }

    }
}