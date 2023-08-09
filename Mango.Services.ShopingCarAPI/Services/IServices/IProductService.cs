using Mango.Services.ShopingCarAPI.Models.Dto;

namespace Mango.Services.ShopingCarAPI.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
    }
}
