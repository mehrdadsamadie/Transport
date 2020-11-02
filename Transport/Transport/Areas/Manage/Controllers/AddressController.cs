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

namespace Transport.Areas.Manage.Controllers
{
    [Authorize]
    public class AddressController : Controller
    {
        private AddressRepository AddressRepository = new AddressRepository();
        public AddressController()
        { }
        public AddressController(ApplicationUserManager userManager)
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
        // GET: Manage/Address
        public ActionResult Index()
        {
            return View();
        }
        [AllowCrossSiteJson]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult AutoAddress(string searchString, bool isBegin = true)
        {
            var _userId = User.Identity.GetUserId();
            var _user = UserManager.FindById(_userId);
            var _addresses = AddressRepository.AutoAddress(searchString,_user.PersonnelId, isBegin).ToList().Select(x => new
            {
                x.CityName,
                x.CountryName,
                x.EparchyId,
                x.StateId,
                x.RegionName,
                x.PostalCode,
                Address = x.Address,
                x.EparchyName
            });
            return Json(_addresses, JsonRequestBehavior.AllowGet);
        }
        //[AllowCrossSiteJson]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //public JsonResult AutoAddressMobile(string searchString, bool isBegin = true)
        //{
        //    var _userId = User.Identity.GetUserId();
        //    var _user = UserManager.FindById(_userId);
        //    var _addresses = AddressRepository.AutoAddress(searchString,,isBegin).ToList().Select(x => new
        //    {
        //        x.CityName,
        //        x.CountryName,
        //        x.EparchyId,
        //        x.StateId,
        //        x.RegionName,
        //        x.PostalCode,
        //        Address = x.Address,
        //    });
        //    return Json(_addresses, JsonRequestBehavior.AllowGet);
        //}
    }
}