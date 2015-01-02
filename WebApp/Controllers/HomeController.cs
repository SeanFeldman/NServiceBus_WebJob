using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Contracts.Commands;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SendCommand()
        {
            MvcApplication.startableBus.Send<Ping>(ping => ping.Message = "ping from web");
            Trace.WriteLine("WebApp - sent a message");
            return View("Index");
        }
    }
}