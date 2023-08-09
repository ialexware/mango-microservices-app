using Mango.Services.ShopingCarAPI.Models.Dto;

namespace Mango.Services.ShopingCarAPI.Services.IServices
{
    public interface ICouponService
    {
        Task<CouponDto> GetCouponAsync(string couponCode);
    }
}
