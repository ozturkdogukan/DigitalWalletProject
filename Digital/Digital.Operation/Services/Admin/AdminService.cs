using AutoMapper;
using Digital.Base.Extensions;
using Digital.Base.Response;
using Digital.Data.Model;
using Digital.Data.UnitOfWork;
using Digital.Operation.Base;
using Digital.Schema.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Operation.Services.Admin
{
    public class AdminService : BaseService<User, UserRequest, UserResponse>, IAdminService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public AdminService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        // Admin kayıt işlemi yapılır. Sadece admin diğer admini kaydedebilir.
        public override ApiResponse Insert(UserRequest request)
        {
            var exist = unitOfWork.GetRepository<User>().
            Any(x => x.Email.Equals(request.Email));

            if (exist)
            {
                return new ApiResponse("Email already in use.");
            }

            try
            {
                request.Password = Extension.CreateMD5(request.Password);
                var entity = mapper.Map<UserRequest, User>(request);
                entity.CreatedAt = DateTime.UtcNow;
                entity.Status = 1;
                entity.Role = "Admin";
                unitOfWork.GetRepository<User>().Add(entity);
                if (unitOfWork.SaveChanges() > 0)
                {
                    return new ApiResponse();
                }
                return new ApiResponse("Internal Server Error");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }


        }
    }
}
