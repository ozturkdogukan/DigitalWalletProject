using AutoMapper;
using Digital.Base.Extensions;
using Digital.Base.Response;
using Digital.Data.Model;
using Digital.Data.UnitOfWork;
using Digital.Operation.Base;
using Digital.Schema.Dto.Product;
using Digital.Schema.Dto.User;
using Digital.Schema.FluentValidation;

namespace Digital.Operation.Services
{
    public class UserService : BaseService<User, UserRequest, UserResponse>, IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        // Kullanıcının kayıt olabilmesini sağlar.
        public override ApiResponse Insert(UserRequest request)
        {
            var validator = new UserValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                string errorMessage = string.Join(Environment.NewLine, validationResult.Errors.Select(error => error.ErrorMessage));
                return new ApiResponse(errorMessage);
            }
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
                entity.Role = "Member";
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
        // Kullanıcının bakiye bilgisinin dönülmesini sağlar.
        public ApiResponse<UserResponse> GetUserPoint(int userId)
        {
            var user = unitOfWork.GetRepository<User>().GetById(userId);
            var userResponse = mapper.Map<UserResponse>(user);
            return new ApiResponse<UserResponse>(userResponse);
        }


        // Bütün kullanıcı bilgileri dönülür.
        public ApiResponse<List<AdminUserResponse>> GetAllUser()
        {
            var users = unitOfWork.GetRepository<User>().GetAll()?.ToList();
            var userResponse = mapper.Map<List<AdminUserResponse>>(users);
            return new ApiResponse<List<AdminUserResponse>>(userResponse);
        }

        // Kullanıcı sadece kendi user bilgilerini düzenleyebilir.
        public ApiResponse UpdateUser(int userId, UserRequest request)
        {
            var user = unitOfWork.GetRepository<User>().Any(x => x.Id.Equals(userId));
            if (!user)
            {
                return new ApiResponse("Record Not Found.");
            }

            var existingUser = unitOfWork.GetRepository<User>().GetById(userId);

            // Sadece güncellenmesi istenen alanları güncelle
            existingUser.Name = request.Name;
            existingUser.LastName = request.LastName;
            existingUser.Email = request.Email;
            existingUser.Password = Extension.CreateMD5(request.Password);

            existingUser.UpdatedAt = DateTime.UtcNow;

            unitOfWork.GetRepository<User>().Update(existingUser);

            if (unitOfWork.SaveChanges() > 0)
            {
                return new ApiResponse();
            }

            return new ApiResponse("Internal Server Error");
        }

        // Adminin kullanıcı bilgilerini düzenleyebilmesini sağlayan metod. Rol alanında değişiklik yapılamaz.
        public ApiResponse AdminUpdateUser(int userId, AdminUserRequest request)
        {
            var user = unitOfWork.GetRepository<User>().Any(x => x.Id.Equals(userId));
            if (!user)
            {
                return new ApiResponse("Record Not Found.");
            }
            var existingUser = unitOfWork.GetRepository<User>().GetById(userId);
            // Role alanını korumak için geçici bir değişken tanımla
            var tempRole = existingUser.Role;
            request.Password = Extension.CreateMD5(request.Password);
            // request nesnesini existingUser nesnesine eşle
            mapper.Map(request, existingUser);
            // Role alanını geçici değişkenle güncelle
            existingUser.Role = tempRole;
            existingUser.Id = userId;
            existingUser.UpdatedAt = DateTime.UtcNow;

            unitOfWork.GetRepository<User>().Update(existingUser);

            if (unitOfWork.SaveChanges() > 0)
            {
                return new ApiResponse();
            }
            return new ApiResponse("Internal Server Error");
        }

    }
}
