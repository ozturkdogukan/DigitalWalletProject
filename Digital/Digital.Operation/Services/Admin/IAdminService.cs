using Digital.Data.Model;
using Digital.Operation.Base;
using Digital.Schema.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Operation.Services.Admin
{
    public interface IAdminService : IBaseService<User, UserRequest, UserResponse>
    {

    }
}
