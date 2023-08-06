using AutoMapper;
using Digital.Data.Model;
using Digital.Data.UnitOfWork;
using Digital.Operation.Base;
using Digital.Schema.Dto.Category;
using Digital.Schema.Dto.Coupon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Operation.Services
{
    public class CouponService : BaseService<Coupon, CouponRequest, CouponResponse>, ICouponService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public CouponService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }


    }
}
