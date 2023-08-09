using Mango.Services.ShopingCarAPI.Models.Dto;
using Mango.Services.ShopingCarAPI.Services.IServices;
using Newtonsoft.Json;

namespace Mango.Services.ShopingCarAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _clientFactory;

        public CouponService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<CouponDto> GetCouponAsync(string couponCode)
        {
            var client = _clientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"/api/coupon/GetByCode/{couponCode}");
            var content = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (resp != null && resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
            }
            return null;
        }
    }
}
