using Digital.Base.Jwt;
using Digital.Base.Logger;
using Digital.Data.Context;
using Digital.Data.UnitOfWork;
using Digital.WebApi.Middleware;
using Digital.WebApi.RestExtension;
using FluentValidation.AspNetCore;
using Serilog;
using System.Text.Json.Serialization;

namespace Digital.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static JwtConfig JwtConfig { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation(x => x.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies())).AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            JwtConfig = Configuration.GetSection("JwtConfig").Get<JwtConfig>();
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));
            services.AddCustomSwaggerExtension();
            services.AddDbContextExtension(Configuration);
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddMapperExtension();
            services.AddServiceExtension();
            services.AddJwtExtension();
            services.AddRedisExtension(Configuration);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            // Auto Migration

            IServiceScopeFactory serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
            DigitalEfDbContext dbContext = serviceScope.ServiceProvider.GetService<DigitalEfDbContext>();
            dbContext.Database.EnsureCreated();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DefaultModelsExpandDepth(-1);
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Digital Company");
                c.DocumentTitle = "Digital Company";
            });

            Action<RequestProfilerModel> requestResponseHandler = requestProfilerModel =>
            {
                Log.Information("-------------Request-Begin------------");
                Log.Information(requestProfilerModel.Request);
                Log.Information(Environment.NewLine);
                Log.Information(requestProfilerModel.Response);
                Log.Information("-------------Request-End------------");
            };
            app.UseMiddleware<RequestLoggingMiddleware>(requestResponseHandler);

            app.UseHttpsRedirection();

            // add auth 
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
