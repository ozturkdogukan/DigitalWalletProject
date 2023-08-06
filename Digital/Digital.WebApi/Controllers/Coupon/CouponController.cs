using Digital.Base.Response;
using Digital.Operation.Services;
using Digital.Schema.Dto.Category;
using Digital.Schema.Dto.Coupon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Digital.WebApi.Controllers.Coupon
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class CouponController : ControllerBase
    {

        private readonly ICouponService couponService;

        public CouponController(ICouponService couponService)
        {
            this.couponService = couponService;
        }

        // Tüm kuponları ve bilgierini getirir.
        [HttpGet("GetAll")]
        public ApiResponse<List<CouponResponse>> GetAll()
        {
            var response = couponService.GetAll();
            return response;
        }

        // Id bilgisine göre kupon bilgisi getirir.
        [HttpGet("GetById")]
        public ApiResponse<CouponResponse> GetById(int id)
        {
            var response = couponService.GetById(id);
            return response;
        }

        // Kupon oluşturma işlemi yapılır.
        [HttpPost]
        public ApiResponse AddCoupon(CouponRequest couponRequest)
        {
            var response = couponService.Insert(couponRequest);
            return response;
        }
        // Kupon güncelleme işlemi yapılır.
        [HttpPut]
        public ApiResponse UpdateCoupon(int id, CouponRequest couponRequest)
        {
            var response = couponService.Update(id, couponRequest);
            return response;
        }

        // Kupon silme işlemi yapılır.
        [HttpDelete]
        public ApiResponse DeleteCoupon(int id)
        {
            var response = couponService.Delete(id);
            return response;
        }
    }
}
