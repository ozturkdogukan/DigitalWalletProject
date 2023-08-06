using Digital.Base.Redis;
using Digital.Base.Response;
using Digital.Operation.Services;
using Digital.Operation.Services.Redis;
using Digital.Schema.Dto.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Digital.WebApi.Controllers.Cache
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IRedisService redisService;
        private readonly IUserService userService;
        public CacheController(IRedisService redisService, IUserService userService)
        {
            this.redisService = redisService;
            this.userService = userService;
        }
        // User Id ve Cüzdan değerini cachelemek için kullanılır. Örnek olması amacıyla yaptım.
        [HttpPost]
        public ApiResponse<bool> RedisUserCache()
        {
            var users = userService.GetAll()?.Response;
            bool status = redisService.SetDynamic(RedisKeys.UserWalletList, users);
            return new ApiResponse<bool>(status);
        }
        // User Id ve Cüzdan değerini cacheden çağırmak için kullanılır. Örnek olması amacıyla yaptım.

        [HttpGet]
        public ApiResponse<List<UserResponse>> RedisGetUserCache()
        {
            var cache = redisService.GetDynamic<List<UserResponse>>(RedisKeys.UserWalletList);
            return new ApiResponse<List<UserResponse>>(cache);
        }
        // Redisteki bütün verileri temizler.
        [HttpPut("Flush")]
        public ApiResponse Flush()
        {
            redisService.Flush();
            return new ApiResponse();
        }

    }
}
