using Mango.Web.Utility;
using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public string? ImageLocalPathUrl { get; set; } = string.Empty;


        [Range(1, 100, ErrorMessage = "Count must be at least 1")]
        public int Count { get; set; } = 1;

        [MaxFileSizeAttribute(1)]
        [AllowedExtencionsAttribute(new string[] { ".jpg", ".png" })]
        public IFormFile Image { get; set; }
    }
}
