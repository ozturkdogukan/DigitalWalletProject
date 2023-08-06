using Digital.Base.Response;
using Digital.Operation.Services;
using Digital.Schema.Dto.Order;
using Digital.Schema.Dto.OrderDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Digital.WebApi.Controllers.OrderDetail
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            this.orderDetailService = orderDetailService;
        }

        // Verilen sipariş numarasına göre sipariş detayları getirilir.
        [HttpGet("GetAll")]
        public ApiResponse<List<OrderDetailResponse>> GetAllMyOrder(string orderNumber)
        {
            var response = orderDetailService.GetAllMyOrderDetail(orderNumber);
            return response;
        }

    }
}
