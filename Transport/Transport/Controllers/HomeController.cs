using Domain.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using Transport.Infrastructure;
using Transport.Models;

namespace Transport.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        PersonnelRepository PersonnelRepository = new PersonnelRepository();
        public HomeController()
        {

        }
        public HomeController(ApplicationUserManager userManager)
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

        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return  RedirectToAction("Index", "Home", new { area = "Manage" });
            }
            else if (User.IsInRole("Driver"))
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Request");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowCrossSiteJson]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult GetDate()
        {

            var _date = Tavanmand.Classes.PersianCalendarConverter.MiladiToFarsiDate(DateTime.Now, false, null);
            return Json(_date,JsonRequestBehavior.AllowGet);
        }

        [AllowCrossSiteJson]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult GetPersonelInfo()
        {
            if (User.Identity.IsAuthenticated)
            {
                var _userId = User.Identity.GetUserId();
                var _user = UserManager.FindById(_userId);
                var _personel = PersonnelRepository.GetById(_user.PersonnelId.Value);
                var _personelview = new PersonelView()
                {
                    FullName = _personel.FullName,
                    Gender = _personel.Gender,
                    PersonnelCode = _personel.PersonnelCode,
                    Mobile = _personel.Mobile,
                };
                return Json(_personelview);
            }
            else
                return null;
        }
    }
}