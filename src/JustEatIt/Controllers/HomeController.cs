using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JustEatIt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace JustEatIt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public IWebHostEnvironment hEnv { get; set; }

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult IndexRole()
        {
            if (User.IsInRole("Customer"))
            {
                return RedirectToAction("Index", "Dish");
            }
            else if (User.IsInRole("Partner"))
            {
                return RedirectToAction("Index", "Partner");
            }
            else if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction(nameof(Index));
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

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult SSLValidation(string sslName)
        {
            string filepath = "C:\\Temp\\";
            string filename = sslName;
            IFileProvider provider = new PhysicalFileProvider(filepath);
            IFileInfo fileInfo = provider.GetFileInfo(filename);
            var readStream = fileInfo.CreateReadStream();
            var mimeType = "text/plain";

            return File(readStream, mimeType);
        }

    }
}
