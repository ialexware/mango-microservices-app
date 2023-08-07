using System.ComponentModel.DataAnnotations;

namespace Mango.Services.CouponAPI.Models
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }
        [Required]
        public required string CouponCode { get; set; }
        [Required]
        public double DsicountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
