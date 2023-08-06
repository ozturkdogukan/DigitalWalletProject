using Digital.Base.Response;
using Digital.Data.Model;
using Digital.Operation.Base;
using Digital.Schema.Dto.Order;
using Digital.Schema.Dto.OrderDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Operation.Services
{
    public interface IOrderDetailService : IBaseService<OrderDetail, OrderDetailRequest, OrderDetailResponse>
    {
        public ApiResponse<List<OrderDetailResponse>> GetAllMyOrderDetail(string orderNumber);
    }
}
