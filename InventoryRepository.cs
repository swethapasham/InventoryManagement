using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TestProject.Models;

namespace TestProject.DAL
{
    public class InventoryRepository
    {
        private readonly InventoryContext _inventoryContext = new InventoryContext();

        public List<Package> GetPackages()
        {
            return _inventoryContext.Packages
                .OrderByDescending(x => x.CartonId)
                .ToList();
        }

        public void AddPackage(Package package)
        {
            SqlParameter barcode = new SqlParameter("@barcode", package.Barcode);
            SqlParameter location = new SqlParameter("@location", package.PickupLocation);
            SqlParameter pickupDate = new SqlParameter("@pickupDate", package.PickupDate);
            _inventoryContext.Database.ExecuteSqlCommand(
                "usp_AddPackage @barcode, @location, @pickupDate",
                barcode, location, pickupDate);
        }

        public List<CameraCapture> GetLatestCaptures()
        {
            return _inventoryContext.CameraCaptures
                .OrderByDescending(x => x.Id)
                .Take(5)
                .ToList();
        }

        public void SaveCameraCapture(CameraCapture cameraCapture)
        {
            _inventoryContext.CameraCaptures.Add(cameraCapture);
            _inventoryContext.SaveChanges();
        }
    }
}