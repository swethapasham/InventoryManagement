using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestProject.Models
{
    public class Package
    {
        [Key]
        public int CartonId { get; set; }

        public string Barcode { get; set; }

        [Required]
        public string PickupLocation { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PickupDate { get; set; }

        public string PickupDateString {
            get
            {
                return PickupDate.ToShortDateString();
            }
        }
    }
}