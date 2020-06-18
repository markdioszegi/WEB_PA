using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace PA.Helpers
{
    public static class HtmlUtility
    {
        public static string IsActive(this IHtmlHelper html, string controller, string action)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeController = (string)routeData.Values["controller"];

            var returnActive = controller == routeController && action == routeAction;

            return returnActive ? "active" : "";
        }

        public static string IsActive(this IHtmlHelper html, string controller)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeController = (string)routeData.Values["controller"];

            var returnActive = controller == routeController;

            return returnActive ? "active" : "";
        }
    }
}