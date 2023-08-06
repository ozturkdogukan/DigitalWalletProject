using Digital.Base.Notification;
using Digital.Base.Response;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Digital.Operation.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration configuration;
        public NotificationService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        // Rabbitmq kuyruğundaki son maili alır okur ve SendMail metoduna iletir.
        public ApiResponse ConsumeEmail()
        {
            try
            {
                var host = configuration["RabbitMQ:Host"].ToString();
                var port = configuration["RabbitMQ:Port"];
                var factory = new ConnectionFactory() { HostName = host, Port = Convert.ToInt32(port) };

                using (IConnection connection = factory.CreateConnection())
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(QueueName.MailQueue, true, false, false, null);

                    BasicGetResult result = channel.BasicGet(QueueName.MailQueue, autoAck: true);

                    if (result != null)
                    {
                        var body = result.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        MailInfo mail = JsonConvert.DeserializeObject<MailInfo>(message);
                        if (mail != null)
                        {
                            bool status = SendMail(mail);
                            if (!status)
                                Log.Error($"Email send failed for {mail.EmailTo}");
                        }
                    }
                    else
                    {
                        Log.Error("No messages available in the queue.");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("NotificationService", ex);
                return new ApiResponse(ex.Message);
            }

            return new ApiResponse();
        }
        // Rabbitmq kuyruğuna yeni bir mail ekler.
        public ApiResponse ProduceEmail(MailInfo request)
        {
            if (request == null)
                return new ApiResponse("Invalid mail request");
            if (string.IsNullOrWhiteSpace(request.EmailTo))
                return new ApiResponse("Invalid mail request");
            if (string.IsNullOrWhiteSpace(request.Subject))
                return new ApiResponse("Invalid mail request");
            if (string.IsNullOrWhiteSpace(request.Body))
                return new ApiResponse("Invalid mail request");


            try
            {
                var host = configuration["RabbitMQ:Host"].ToString();
                var port = configuration["RabbitMQ:Port"];
                var factory = new ConnectionFactory() { HostName = host, Port = Convert.ToInt32(port) };

                using (IConnection connection = factory.CreateConnection())
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(QueueName.MailQueue, true, false, false, null);

                    string body = JsonConvert.SerializeObject(request);
                    byte[] arr = Encoding.UTF8.GetBytes(body);

                    channel.BasicPublish(exchange: "", routingKey: QueueName.MailQueue, basicProperties: null, body: arr);
                }
            }
            catch (Exception ex)
            {
                Log.Error("NotificationService", ex);
                return new ApiResponse(ex.Message);
            }

            return new ApiResponse();
        }

        // Mail göndermek için kullanılır.
        private bool SendMail(MailInfo mail)
        {
            try
            {
                var host = configuration["Smtp:Host"].ToString();
                var from = configuration["Smtp:From"].ToString();
                var username = configuration["Smtp:Username"].ToString();
                var password = configuration["Smtp:Password"].ToString();

                SmtpClient client = new SmtpClient()
                {
                    Host = host,
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true,
                    Port = 587
                };

                client.Send(from, mail.EmailTo, mail.Subject, mail.Body);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("NotificationService", ex);
                return false;
            }

        }
    }

}
