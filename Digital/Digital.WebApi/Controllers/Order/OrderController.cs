using AutoMapper.QueryableExtensions;
using Digital.Base.Response;
using Digital.Operation.Services;
using Digital.Schema.Dto.Category;
using Digital.Schema.Dto.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace Digital.WebApi.Controllers.Order
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        // Sipariş Oluşturmak için kullanılır. Kullanıcının kim olduğu Jwt tokeni aracılığıyla alınır.

        [HttpPost]
        [JwtUser]
        public ApiResponse AddOrder(OrderRequest orderRequest)
        {
            var userId = Digital.Base.Extensions.Extension.GetUserIdFromContext(HttpContext);
            var response = orderService.Add(orderRequest, userId);
            return response;
        }
        // Kişinin kendisine ait siparişleri getirmesi sağlanır.
        [HttpGet("GetAllMyOrder")]
        [JwtUser]
        public ApiResponse<List<OrderResponse>> GetAllMyOrder()
        {
            var userId = Digital.Base.Extensions.Extension.GetUserIdFromContext(HttpContext);
            var response = orderService.GetAllMyOrder(userId);
            return response;
        }

    }
}
