using Digital.Base.Notification;
using Digital.Base.Response;
using Digital.Operation.Services;
using Digital.Operation.Services.Notification;
using Digital.Schema.Dto.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Digital.WebApi.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService notificationService;

        public NotificationController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }


        // RabbitMq kuyruğuna mail ekler.
        [HttpPost("ProcuderMail")]
        public ApiResponse ProcuderMail(MailInfo mailRequest)
        {
            var response = notificationService.ProduceEmail(mailRequest);
            return response;
        }

        // Rabbitmq kuyruğundaki son maili alıp gönderir.
        [HttpPost("ConsumeMail")]
        public ApiResponse ConsumeMail()
        {
            var response = notificationService.ConsumeEmail();
            return response;
        }

    }
}
