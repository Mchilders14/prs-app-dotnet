using System;
namespace prs_app_dotnet.Models
{
    public class LineItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public int RequestId { get; set; }
        public virtual Request Request { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
