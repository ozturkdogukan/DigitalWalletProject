using AutoMapper;
using Digital.Data.Model;
using Digital.Schema.Dto.Category;
using Digital.Schema.Dto.Coupon;
using Digital.Schema.Dto.Order;
using Digital.Schema.Dto.OrderDetail;
using Digital.Schema.Dto.Product;
using Digital.Schema.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Digital.Schema.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<User, UserRequest>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<Category, CategoryRequest>().ReverseMap();
            CreateMap<Category, CategoryResponse>().ReverseMap();
            CreateMap<Product, ProductRequest>().ReverseMap();
            CreateMap<Product, ProductResponse>().ReverseMap();
            CreateMap<Coupon, CouponRequest>().ReverseMap();
            CreateMap<Coupon, CouponResponse>().ReverseMap();
            CreateMap<Order, OrderRequest>().ReverseMap();
            CreateMap<Order, OrderResponse>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailRequest>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailResponse>().ReverseMap();
            CreateMap<User, AdminUserRequest>().ReverseMap();
            CreateMap<User, AdminUserResponse>().ReverseMap();
        }
    }

}
