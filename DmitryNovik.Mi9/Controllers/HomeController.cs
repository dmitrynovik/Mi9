using DmitryNovik.Mi9.Lib.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DmitryNovik.Mi9.Controllers
{
    public class HomeController : Controller
    {
        const int BAD_REQUEST = 400;
        private readonly IShowsFilterService _filter = new ShowsDrmEpisodesFilterService();

        // 
        public ActionResult Index()
        {
            Response.StatusCode = BAD_REQUEST;
            return Json("Get not allowed", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(ShowRequest request)
        {
            string error;
            var response = _filter.Filter(request, out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                Response.StatusCode = BAD_REQUEST;
            }
            return new ContentResult() { Content = response, ContentType = "application/json" };
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            Response.StatusCode = BAD_REQUEST;
            filterContext.Result = Index(null);
        }
    }
}
