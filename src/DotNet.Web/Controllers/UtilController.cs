using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Edu.WebUtility;
using DotNet.Helper;
using DotNet.Mvc;

namespace DotNet.Web.Controllers
{
    public class UtilController : JsonController
    {
        public ActionResult Province()
        {
            return Content(WebHelper.GetSelectOptions(RegionHelper.GetProvince(),valueMember:"Code"));
        }

        public ActionResult City(string provinceCode)
        {
            return Content(WebHelper.GetSelectOptions(RegionHelper.GetCity(provinceCode), valueMember: "Code"));
        }

        public ActionResult District(string cityCode)
        {
            return Content(WebHelper.GetSelectOptions(RegionHelper.GetDistrict(cityCode), valueMember: "Code"));
        }
    }
}