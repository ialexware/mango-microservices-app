using AutoMapper;
using Mango.Services.ShopingCarAPI.Models;
using Mango.Services.ShopingCarAPI.Models.Dto;

namespace Mango.Services.ShopingCarAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeaderDto, CartHeader>().ReverseMap();
                config.CreateMap<CartDetailsDto, CartDetails>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
