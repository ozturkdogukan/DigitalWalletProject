using Digital.Base.Response;
using Digital.Schema.Dto.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Operation.Services.Token
{
    public interface ITokenService
    {
        ApiResponse<TokenResponse> GetToken(TokenRequest request);

    }
}
