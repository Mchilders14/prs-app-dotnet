using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace prs_app_dotnet.Models
{
    public class Vendor
    {
        public int Id { get; set; }

        [StringLength(10), Required]
        public string Code { get; set; }

        [StringLength(60), Required]
        public string Name { get; set; }

        [StringLength(40), Required]
        public string Address { get; set; }

        [StringLength(40), Required]
        public string City { get; set; }

        [StringLength(30), Required]
        public string State { get; set; }

        [StringLength(20), Required]
        public string Zip { get; set; }

        [StringLength(20), Required]
        public string Phone { get; set; }

        [StringLength(50), Required]
        public string Email { get; set; }
    }
}
