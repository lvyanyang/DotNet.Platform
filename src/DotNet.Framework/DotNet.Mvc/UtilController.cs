using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Helper;
using DotNet.Mvc;

namespace DotNet.Mvc
{
    public class UtilController : JsonController
    {
        public ActionResult Province()
        {
            return Content(WebHelper.GetSelectOptions(RegionHelper.GetProvince(),valueMember:"Code"));
        }

        public ActionResult ProvinceJson()
        {
            return Json(RegionHelper.GetProvince());
        }

        public ActionResult City(string provinceCode)
        {
            return Content(WebHelper.GetSelectOptions(RegionHelper.GetCity(provinceCode), valueMember: "Code"));
        }

        public ActionResult CityJson(string provinceCode)
        {
            return Json(RegionHelper.GetCity(provinceCode));
        }

        public ActionResult District(string cityCode)
        {
            return Content(WebHelper.GetSelectOptions(RegionHelper.GetDistrict(cityCode), valueMember: "Code"));
        }

        public ActionResult DistrictJson(string cityCode)
        {
            return Json(RegionHelper.GetDistrict(cityCode));
        }
    }
}