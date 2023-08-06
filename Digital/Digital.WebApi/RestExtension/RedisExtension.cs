using StackExchange.Redis;

namespace Digital.WebApi.RestExtension
{
    public static class RedisExtension
    {
        public static void AddRedisExtension(this IServiceCollection services, IConfiguration configuration)
        {
            var configOptions = new ConfigurationOptions();
            configOptions.EndPoints.Add(configuration["Redis:Host"], Convert.ToInt32(configuration["Redis:Port"]));
            configOptions.DefaultDatabase = Convert.ToInt32(configuration["Redis:DefaultDatabase"]);
            configOptions.AllowAdmin = true;

            services.AddStackExchangeRedisCache(option =>
            {
                option.ConfigurationOptions = configOptions;
                option.InstanceName = configuration["Redis:InstanceName"];
            });
        }
    }
}
