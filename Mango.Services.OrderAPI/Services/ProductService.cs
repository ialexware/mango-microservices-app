﻿using Mango.Services.OrderAPI.Models.Dto;
using Mango.Services.OrderAPI.Services.IServices;
using Newtonsoft.Json;

namespace Mango.Services.OrderAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProductService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var client = _clientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/product");
            var content = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.Result));
            }
            return new List<ProductDto>();

        }
    }
}
