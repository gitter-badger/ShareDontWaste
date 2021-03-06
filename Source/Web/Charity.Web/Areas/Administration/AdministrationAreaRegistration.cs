﻿using System.Web.Mvc;

namespace Charity.Web.Areas.Administration
{
    public class AdministrationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Administration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
             context.MapRoute(
                name: "Administration_donors_edit",
                url: "Administration/{controller}/{action}/{username}",
                defaults: new { controller = "Donors", action = "Edit", username = "" },
                namespaces: new string[] { "Charity.Web.Areas.Administration.Controllers" }
            );

            context.MapRoute(
                name: "Administration_default",
                url: "Administration/{controller}/{action}/{id}",
                defaults: new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Charity.Web.Areas.Administration.Controllers" }
            );
        }
    }
}