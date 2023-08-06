using Digital.Operation.Services;
using Digital.Operation.Services.Admin;
using Digital.Operation.Services.Notification;
using Digital.Operation.Services.Redis;
using Digital.Operation.Services.Token;

namespace Digital.WebApi.RestExtension
{
    public static class ServiceExtension
    {
        public static void AddServiceExtension(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICouponService, CouponService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddSingleton<IRedisService, RedisService>();
            services.AddSingleton<INotificationService, NotificationService>();
        }
    }

}
