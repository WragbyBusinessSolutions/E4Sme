using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Web.Mvc;

namespace E4S.Helpers
{
  public static class HtmlHelpers
  {
    public static HtmlString IsActive(this HtmlHelper htmlHelper, string action, string controller, string activeClass, string inActiveClass = "")
    {
      var routeData = htmlHelper.ViewContext.RouteData;

      var routeAction = routeData.Values["action"].ToString();
      var routeController = routeData.Values["controller"].ToString();

      var returnActive = (controller == routeController && action == routeAction);

      return new HtmlString(returnActive ? activeClass : inActiveClass);
    }

  }
}
