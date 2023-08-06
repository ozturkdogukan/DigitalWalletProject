using Digital.Base.Response;
using Digital.Operation.Services;
using Digital.Operation.Services.Token;
using Digital.Schema.Dto.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Digital.WebApi.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        // Kullanıcının bakiye bilgisi dönülür.
        [Authorize]
        [JwtUser]
        [HttpGet("GetUserPoint")]
        public ApiResponse<UserResponse> GetUserPoint()
        {
            var userId = Digital.Base.Extensions.Extension.GetUserIdFromContext(HttpContext);
            var response = userService.GetUserPoint(userId);
            return response;
        }
        // Kullanıcı kendisine ait bilgileri düzenleyebilir.
        [Authorize]
        [JwtUser]
        [HttpPut("UserUpdate")]
        public ApiResponse UserUpdate(UserRequest request)
        {
            var userId = Digital.Base.Extensions.Extension.GetUserIdFromContext(HttpContext);
            var response = userService.UpdateUser(userId, request);
            return response;
        }

    }
}
