using Digital.Operation.Services.Token;
using Digital.Operation.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Digital.Base.Response;
using Digital.Schema.Dto.User;
using Digital.Operation.Services.Admin;
using Microsoft.AspNetCore.Authorization;

namespace Digital.WebApi.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        private readonly IUserService userService;


        public AdminController(IAdminService adminService, IUserService userService)
        {
            this.adminService = adminService;
            this.userService = userService;
        }

        // Adminlerin kayıt olabilmesi için kullanılır. Admin kayıtını başka bir admin yapabilir.
        [HttpPost("Register")]
        public ApiResponse Register([FromBody] UserRequest request)
        {
            var response = adminService.Insert(request);
            return response;
        }

        // Adminlerin kullanıcı silmesi için kullanılır.
        [HttpDelete("DeleteUser")]
        public ApiResponse DeleteUser(int userId)
        {
            var response = userService.Delete(userId);
            return response;
        }

        // Adminlerin kullanıcı düzenlemesi için kullanılır.
        [HttpPut("UpdateUser")]
        public ApiResponse UpdateUser(int userId, AdminUserRequest request)
        {
            var response = userService.AdminUpdateUser(userId, request);
            return response;
        }
        // Bütün kullanıcıları çekmek için kullanılır.
        [HttpGet("GetAllUser")]
        public ApiResponse<List<AdminUserResponse>> GetAllUser()
        {
            var response = userService.GetAllUser();
            return response;
        }



    }
}
