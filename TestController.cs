using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using TestProject.DAL;
using TestProject.Models;
using System.IO;
using System.Web.Hosting;

namespace TestProject.Controllers
{
    public class TestController : Controller
    {
        private InventoryRepository inventoryRepository = new InventoryRepository();

        private static Random random = new Random();
        private static List<string> cities = new List<string>()
        {
            "Tampa",
            "Phoenix",
            "Seattle",
            "New York",
            "Miami",
            "Dallas",
            "Austin",
            "Denver",
            "Atlanta"
        };

        private static Timer timer = new Timer((e) =>
        {
            AutoAddPackages();
        }, null, 0, 500);

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetPackages()
        {
            var packages = inventoryRepository.GetPackages();
            return Json(new { data = packages }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddNewPackage()
        {
            var viewModel = new Package();
            viewModel.PickupDate = DateTime.Now;
            return View("PackageDetails", viewModel);
        }

        public ActionResult SavePackageDetails(Package viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Barcode = Guid.NewGuid().ToString();
                inventoryRepository.AddPackage(viewModel);
                return RedirectToAction("Index");
            }
            return View("PackageDetails", viewModel);
        }

        public ActionResult ViewCameraFeed()
        {
            var captures = inventoryRepository.GetLatestCaptures().Select(x => x.FileName);
            ViewBag.Captures = captures;
            return View();
        }

        public ActionResult SaveCapturedImage(string data)
        {
            var fileName = DateTime.Now.ToString("dd-MM-yy hh-mm-ss") + ".jpg";
            byte[] imageBytes = Convert.FromBase64String(data.Split(',')[1]);
            var rootPath = HostingEnvironment.MapPath("~/Captures");
            var filePath = Path.Combine(rootPath, fileName);
            System.IO.File.WriteAllBytes(filePath, imageBytes);

            var capture = new CameraCapture()
            {
                FilePath = filePath,
                FileName = fileName,
                CapturedDate = DateTime.Now
            };
            inventoryRepository.SaveCameraCapture(capture);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private static void AutoAddPackages()
        {
            var _inventoryContext = new InventoryContext();
            SqlParameter barcode = new SqlParameter("@barcode", Guid.NewGuid().ToString());
            SqlParameter location = new SqlParameter("@location", GetRandomCity());
            SqlParameter pickupDate = new SqlParameter("@pickupDate", DateTime.Now.AddDays(random.Next(-365, 365)));
            _inventoryContext.Database.ExecuteSqlCommand(
                "usp_AddPackage @barcode, @location, @pickupDate",
                barcode, location, pickupDate);
        }

        private static string GetRandomCity()
        {
            int index = random.Next(cities.Count);
            return cities[index];
        }
    }
}