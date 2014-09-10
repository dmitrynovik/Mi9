using DmitryNovik.Mi9.Lib.Models;
using DmitryNovik.Mi9.Lib.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DmitryNovik.Mi9.Controllers
{
    public class FilterController : Controller
    {
        const int BAD_REQUEST = 400;
        private readonly ShowsFilterService _filter = new ShowsFilterService();

        public ActionResult FilterShows()
        {
            Response.StatusCode = BAD_REQUEST;
            return Json("Get not allowed", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FilterShows(string sRequest)
        {
            // Cannot rely on .NET MVC serializer as it converts a valid empty array (a valid schema!) into NULL,
            // use low-level input stream instead of model:
            Request.InputStream.Seek(0, SeekOrigin.Begin);

            var response = _filter.Filter(Request.InputStream, s => s.drm && s.episodeCount > 0);
            if (!string.IsNullOrWhiteSpace(response.error))
            {
                Response.StatusCode = BAD_REQUEST;
            }
            return Serialize(response);
        }

        private ActionResult Serialize(ShowResponse response)
        {
            // Use Json.Net's serializer instead of ASP.NET MVC one (gives more control on output)
            return new ContentResult()
            {
                ContentType = "application/json",
                Content = ShowsFilterService.Serialize(response)
            };
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            Response.StatusCode = BAD_REQUEST;
            filterContext.Result = Serialize(ShowResponse.Invalid());
        }
    }
}
