﻿namespace Charity.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public class ErrorController : Controller
    {
        public ViewResult Index()
        {
            return View("Error");
        }

        public ViewResult NotFound()
        {
            Response.StatusCode = 404;  //you may want to set this to 200

            return View("NotFound");
        }
    }
}