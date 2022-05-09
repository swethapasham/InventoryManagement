using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TestProject.Models;

namespace TestProject.DAL
{
    public class InventoryContext : DbContext
    {
        public InventoryContext() : base("InventoryContext")
        {
        }

        public DbSet<Package> Packages { get; set; }

        public DbSet<CameraCapture> CameraCaptures { get; set; }
    }
}