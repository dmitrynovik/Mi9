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
        public ActionResult Index(string sRequest)
        {
            var request = Deserialize(_filter.Serializer);

            string error;
            var response = _filter.Filter(request, out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                Response.StatusCode = BAD_REQUEST;
            }
            return new ContentResult() { Content = response, ContentType = "application/json" };
        }

        /// <summary>
        /// Cannot rely on .NET MVC as it converts a valid empty array (a valid schema!) into NULL:
        /// </summary>
        private ShowRequest Deserialize(JsonSerializer _serializer)
        {
            Request.InputStream.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(Request.InputStream))
            {
                var sRequest = reader.ReadToEnd();
                using (var stringReader = new StringReader(sRequest))
                using (var jsonReader = new JsonTextReader(stringReader))
                {
                    return _serializer.Deserialize<ShowRequest>(jsonReader);
                }
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            Response.StatusCode = BAD_REQUEST;
            filterContext.Result = Json(new { error = "Could not decode request: JSON parsing failed" });
        }
    }
}
