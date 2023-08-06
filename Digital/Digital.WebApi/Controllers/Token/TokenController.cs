using Digital.Base.Jwt;
using Digital.Base.Response;
using Digital.Data.Model;
using Digital.Operation.Services;
using Digital.Operation.Services.Token;
using Digital.Schema.Dto.Token;
using Digital.Schema.Dto.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Digital.WebApi.Controllers.Token
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService tokenService;
        private readonly IUserService userService;


        public TokenController(ITokenService tokenService, IUserService userService)
        {
            this.tokenService = tokenService;
            this.userService = userService;
        }

        // Kayıt işlemi için kullanılır.
        [HttpPost("Register")]
        public ApiResponse Register([FromBody] UserRequest request)
        {
            var response = userService.Insert(request);
            return response;
        }
        // Kullanıcı giriş işlemi için kullanılır.
        [HttpPost("Login")]
        public ApiResponse<TokenResponse> Login([FromBody] TokenRequest request)
        {
            return tokenService.GetToken(request);
        }



    }
}
