using System;
using System.Web.Mvc;
using Contracts.Commands;
using NServiceBus;
using NServiceBus.Logging;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private ILog logger = LogManager.GetLogger<HomeController>();
        public IBus Bus { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendCommand(string message)
        {
            Bus.Send<Ping>(ping =>
            {
                ping.Message = message;
                ping.Timestamp = DateTime.UtcNow;
            });
            logger.Info("WebApp - sent a ping with message: " + message);

            return View("Index");
        }
    }
}