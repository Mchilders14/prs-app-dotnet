using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prs_app_dotnet.Models
{
    public class Request
    {
        public int Id { get; set; }
        public DateTime DateNeeded { get; set; }
        public DateTime SubmittedDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(11,2)")]
        public decimal Total { get; set; }

        [StringLength(255), Required]
        public string Description { get; set; }

        [StringLength(255), Required]
        public string Justification { get; set; }

        [StringLength(50), Required]
        public string DeliveryMode { get; set; }

        [StringLength(30), Required]
        public string Status { get; set; } = "New";

        [StringLength(255), Required]
        public string ReasonForRejection { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual IEnumerable<LineItem> LineItems { get; set; }

    }

}
