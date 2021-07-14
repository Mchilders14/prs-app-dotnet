using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prs_app_dotnet.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(11,2)")]
        public decimal Price { get; set; }

        [StringLength(30), Required]
        public string PartNumber { get; set; }

        [StringLength(255), Required]
        public string Name { get; set; }

        [StringLength(255)]
        public string Unit { get; set; }

        [StringLength(255)]
        public string PhotoPath { get; set; }

        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
