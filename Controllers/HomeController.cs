using BackgroundWorkerQueue.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundWorkerQueue.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BackgroundWorkerQueues _backgroundWorkerQueue;

        public HomeController(ILogger<HomeController> logger, BackgroundWorkerQueues backgroundWorkerQueue)
        {
            _logger = logger;
            _backgroundWorkerQueue = backgroundWorkerQueue;
        }

        
        public async Task<IActionResult> Index()
        {
            await CallSlowApi();
            return View();
        }
        private async Task CallSlowApi()
        {
            _logger.LogInformation($"Starting at {DateTime.UtcNow.TimeOfDay}");
            _backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
            {
                await Task.Delay(10000);
                _logger.LogInformation($"Done at {DateTime.UtcNow.TimeOfDay}");
            });
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    

    
}
