using AutoMapper;
using Digital.Base.Response;
using Digital.Data.Model;
using Digital.Data.UnitOfWork;
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
    public class OrderDetailService : BaseService<OrderDetail, OrderDetailRequest, OrderDetailResponse>, IOrderDetailService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public OrderDetailService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        // Verilen sipariş numarasına göre sipariş detayları getirilir.
        public ApiResponse<List<OrderDetailResponse>> GetAllMyOrderDetail(string orderNumber)
        {
            var orders = unitOfWork.GetRepository<OrderDetail>().GetAll(x => x.OrderNumber.Equals(orderNumber));
            var orderResponses = mapper.Map<List<OrderDetailResponse>>(orders);
            return new ApiResponse<List<OrderDetailResponse>>(orderResponses);

        }
    }
}
