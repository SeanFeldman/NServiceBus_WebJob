using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Contracts.Commands;
using NServiceBus;
using NServiceBus.Logging;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private ILog logger = LogManager.GetLogger<HomeController>();
        public IEndpointInstance Bus { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SendCommand(string message)
        {
            await Bus.Send<Ping>(ping =>
            {
                ping.Message = message;
                ping.Timestamp = DateTime.UtcNow;
            }).ConfigureAwait(false);

            logger.Info("WebApp - sent a ping with message: " + message);

            return View("Index");
        }
    }
}