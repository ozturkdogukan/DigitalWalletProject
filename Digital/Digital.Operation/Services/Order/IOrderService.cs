using Digital.Base.Response;
using Digital.Data.Model;
using Digital.Operation.Base;
using Digital.Schema.Dto.Category;
using Digital.Schema.Dto.Order;
using Digital.Schema.Dto.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Operation.Services
{
    public interface IOrderService : IBaseService<Order, OrderRequest, OrderResponse>
    {
        ApiResponse Add(OrderRequest request, int userId);

        ApiResponse<List<OrderResponse>> GetAllMyOrder(int userId);


    }
}
