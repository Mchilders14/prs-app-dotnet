using System;
using System.ComponentModel.DataAnnotations;

namespace prs_app_dotnet.Models
{
    public class User
    {
        public int Id { get; set; }
        public bool Reviewer { get; set; }
        public bool Admin { get; set; }

        [StringLength(20), Required]
        public string Username { get; set; }

        [StringLength(20), Required]
        public string Password { get; set; }

        [StringLength(30), Required]
        public string FirstName { get; set; }

        [StringLength(30), Required]
        public string LastName { get; set; }

        [StringLength(15), Required]
        public string Phone { get; set; }

        [StringLength(50), Required]
        public string Email { get; set; }


    }
}
