using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService) => _orderService = orderService;

        [Authorize]
        public IActionResult OrderIndex()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeaderDto> orderHeaderDtos;
            string userId = string.Empty;
            if (!User.IsInRole(SD.RoleAdmin))
            {
                userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            }
            ResponseDto responseDto = _orderService.GetAllOrder(userId).GetAwaiter().GetResult();
            if (responseDto != null && responseDto.IsSuccess)
            {
                orderHeaderDtos = JsonConvert.DeserializeObject<IEnumerable<OrderHeaderDto>>(Convert.ToString(responseDto.Result));
                switch (status)
                {
                    case "approved":
                        orderHeaderDtos = orderHeaderDtos.Where(u => u.Status == SD.Status_Approved);
                        break;
                    case "readyforpickup":
                        orderHeaderDtos = orderHeaderDtos.Where(u => u.Status == SD.Status_RedyForPickup);
                        break;
                    case "cancelled":
                        orderHeaderDtos = orderHeaderDtos.Where(u => u.Status == SD.Status_Cancelled || u.Status == SD.Status_Refunded);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                orderHeaderDtos = new List<OrderHeaderDto>();
            }
            return Json(new { data = orderHeaderDtos });
        }

        [Authorize]
        public async Task<IActionResult> OrderDetail(int orderId)
        {
            OrderHeaderDto orderHeaderDto = new();
            string userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;

            ResponseDto responseDto = await _orderService.GetOrder(orderId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(responseDto.Result));
            }
            if (!User.IsInRole(SD.RoleAdmin) && userId != orderHeaderDto.UserId)
            {
                return NotFound();
            }
            return View(orderHeaderDto);
        }

        [HttpPost("OrderReadyForPickup")]
        public async Task<IActionResult> OrderReadyForPickup(int orderId)
        {
            ResponseDto responseDto = await _orderService.UpdateOrderStatus(orderId, SD.Status_RedyForPickup);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["result"] = "Order is ready for pickup";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
            return View();
        }

        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            ResponseDto responseDto = await _orderService.UpdateOrderStatus(orderId, SD.Status_Completed);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["result"] = "Order completed successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
            return View();
        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            ResponseDto responseDto = await _orderService.UpdateOrderStatus(orderId, SD.Status_Cancelled);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["result"] = "Order was cancelled successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
            return View();
        }

    }
}
