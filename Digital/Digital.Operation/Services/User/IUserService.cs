using Digital.Base.Response;
using Digital.Data.Model;
using Digital.Operation.Base;
using Digital.Schema.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Operation.Services
{
    public interface IUserService : IBaseService<User, UserRequest, UserResponse>
    {
        public ApiResponse<UserResponse> GetUserPoint(int userId);
        public ApiResponse UpdateUser(int userId, UserRequest request);
        public ApiResponse AdminUpdateUser(int userId, AdminUserRequest request);
        public ApiResponse<List<AdminUserResponse>> GetAllUser();



    }
}
