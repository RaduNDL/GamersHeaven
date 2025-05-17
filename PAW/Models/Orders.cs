using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAW.Models
{
    public class Orders
    {
        [Key]
        public int OrderID { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
