using Digital.Base.Notification;
using Digital.Base.Response;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Operation.Services.Notification
{
    public interface INotificationService
    {
        ApiResponse ProduceEmail(MailInfo request);
        ApiResponse ConsumeEmail();
    }
}
