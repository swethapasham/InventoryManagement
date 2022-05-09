using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestProject.Models
{
    public class CameraCapture
    {
        [Key]
        public int Id { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public DateTime CapturedDate { get; set; }
    }
}