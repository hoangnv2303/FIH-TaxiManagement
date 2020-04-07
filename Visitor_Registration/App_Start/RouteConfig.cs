using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Visitor_Registration
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Create Request",
                url: "create-request",
                defaults: new { controller = "Request", action = "CreateRequest", id = UrlParameter.Optional },
                namespaces: new[] { "Visitor_Registration.Controllers" }
            );

            routes.MapRoute(
                name: "My Request",
                url: "my-request",
                defaults: new { controller = "Request", action = "MyRequest", id = UrlParameter.Optional },
                namespaces: new[] { "Visitor_Registration.Controllers" }
            );

            routes.MapRoute(
             name: "Update Request",
             url: "update-request/{id}",
             defaults: new { controller = "Request", action = "UpdateRequest", id = UrlParameter.Optional },
             namespaces: new[] { "Visitor_Registration.Controllers" }
            );


            routes.MapRoute(
                name: "Approval",
                url: "approval/{levelType}",
                defaults: new { controller = "Approval", action = "Approval", id = UrlParameter.Optional },
                namespaces: new[] { "Visitor_Registration.Controllers" }
            );

            // Route with mutiple parameter
            //routes.MapRoute(
            //  "Multiple Parameter",
            //  "controller/{year}/{month}",
            //  new { controller = "Controller", action = "Action"});

            routes.MapRoute(
                name: "Product Category",
                url: "request-detail/{id}/{nationalId}",
                defaults: new { controller = "Request", action = "RequestDetail", id = UrlParameter.Optional },
                namespaces: new[] { "Visitor_Registration.Controllers" }

            );

            routes.MapRoute(
                name: "Deputy",
                url: "deputy",
                defaults: new { controller = "Setting", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Visitor_Registration.Controllers" }
            );

            routes.MapRoute(
              name: "Permission",
              url: "user-role",
              defaults: new { controller = "Setting", action = "ShowUser", id = UrlParameter.Optional },
              namespaces: new[] { "Visitor_Registration.Controllers" }
             );

            routes.MapRoute(
              name: "BlackList",
              url: "black-list",
              defaults: new { controller = "Setting", action = "BlackList", id = UrlParameter.Optional },
              namespaces: new[] { "Visitor_Registration.Controllers" }
             );

            routes.MapRoute(
              name: "Report Daily",
              url: "report",
              defaults: new { controller = "Report", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "Visitor_Registration.Controllers" }
             );

            routes.MapRoute(
              name: "Show Security",
              url: "security",
              defaults: new { controller = "Home", action = "ShowSecurity", id = UrlParameter.Optional },
              namespaces: new[] { "Visitor_Registration.Controllers" }
             );

            routes.MapRoute(
              name: "Show Receptionist",
              url: "receptionist",
              defaults: new { controller = "Home", action = "ShowReceptionist", id = UrlParameter.Optional },
              namespaces: new[] { "Visitor_Registration.Controllers" }
             );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
