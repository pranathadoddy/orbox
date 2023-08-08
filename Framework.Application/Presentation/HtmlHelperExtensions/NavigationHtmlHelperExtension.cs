using System;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Framework.Application.Presentation.HtmlHelperExtensions
{
    public static class NavigationHtmlHelperExtension
    {
        public static IHtmlContent NavigationLink(
            this IHtmlHelper htmlHelper,
            string route,
            string iconClass,
            string text,
            bool differentiateByAction = false,
            string [] alsoActiveInThisUrl = null
        )
        {
            var listItem = new TagBuilder("li");
            listItem.AddCssClass("nav-item");

            var link = new TagBuilder("a");
            link.AddCssClass("nav-link");
            link.Attributes.Add("href", route);

            var icon = new TagBuilder("i");
            icon.AddCssClass(iconClass);

            var currentRoute = differentiateByAction
                ? (string) htmlHelper.ViewContext.RouteData.Values["action"]
                : (string) htmlHelper.ViewContext.RouteData.Values["controller"];

            if (route.StartsWith("/Administrator"))
            {
                route = route.Replace("/Administrator", "");
            }
            else if (route.StartsWith("/Advertiser"))
            {
                route = route.Replace("/Advertiser", "");
            }

            if (route == "/" || route == "")
            {
                route = "/Home/Index";
            }

            var splittedRoute = route.Split('/');
            var urlRoute = splittedRoute[1];
            if (differentiateByAction)
            {
                urlRoute = splittedRoute[2];
            }

            var currentFullRoute = 
                "/" + 
                (string) htmlHelper.ViewContext.RouteData.Values["controller"] + 
                "/" + 
                (string) htmlHelper.ViewContext.RouteData.Values["action"];

            if (
                string.Equals(urlRoute.ToLower(), currentRoute.ToLower()) || 
                (
                    alsoActiveInThisUrl != null &&
                    alsoActiveInThisUrl.Any(item => 
                        string.Equals(item.ToLower(), currentFullRoute.ToLower())
                    )
                )
            )
            {
                link.AddCssClass("active");
            }
            
            link.InnerHtml.AppendHtml(icon);
            link.InnerHtml.AppendHtml(" ");
            link.InnerHtml.AppendHtml(text);

            listItem.InnerHtml.AppendHtml(link);

            return listItem;
        }
    }
}