using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{

    [Route("api/product")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _responseDto;
        private IMapper _mapper;

        public ProductAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Product> objList = _db.Products.ToList();
                _responseDto.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Product objProduct = _db.Products.First(u => u.ProductId == id);
                _responseDto.Result = _mapper.Map<ProductDto>(objProduct);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post(ProductDto productDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDto);
                _db.Products.Add(product);
                _db.SaveChanges();

                if (productDto.Image != null)
                {
                    string fileName = $"{product.ProductId}{Path.GetExtension(productDto.Image.FileName)}";
                    string filePath = $@"wwwroot\ProductImages\{fileName}";
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (FileStream fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        productDto.Image.CopyTo(fileStream);
                    }

                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";

                    product.ImageUrl = $"{baseUrl}/ProductImages/{fileName}";
                    product.ImageLocalPathUrl = filePath;
                }
                else
                {
                    productDto.ImageUrl = "https://placehold.co//600x400";
                }
                _db.Products.Update(product);
                _db.SaveChanges();

                _responseDto.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put(ProductDto productDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDto);

                if (productDto.Image != null)
                {
                    if (!string.IsNullOrEmpty(product.ImageLocalPathUrl))
                    {
                        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPathUrl);
                        FileInfo fileInfo = new FileInfo(oldFilePathDirectory);
                        if (fileInfo.Exists)
                        {
                            fileInfo.Delete();
                        }
                    }

                    string fileName = $"{product.ProductId}{Path.GetExtension(productDto.Image.FileName)}";
                    string filePath = $@"wwwroot\ProductImages\{fileName}";
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (FileStream fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        productDto.Image.CopyTo(fileStream);
                    }

                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";

                    product.ImageUrl = $"{baseUrl}/ProductImages/{fileName}";
                    product.ImageLocalPathUrl = filePath;
                }

                _db.Products.Update(product);
                _db.SaveChanges();
                _responseDto.Result = product;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        [Route("{id:int}")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Product product = _db.Products.FirstOrDefault(u => u.ProductId == id);
                if (product == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Product not found";
                    return _responseDto;
                }

                if (!string.IsNullOrEmpty(product.ImageLocalPathUrl))
                {
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPathUrl);
                    if (System.IO.File.Exists(filePathDirectory))
                    {
                        System.IO.File.Delete(filePathDirectory);
                    }
                }
                _db.Products.Remove(product);
                _db.SaveChanges();
                _responseDto.Result = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
    }
}
