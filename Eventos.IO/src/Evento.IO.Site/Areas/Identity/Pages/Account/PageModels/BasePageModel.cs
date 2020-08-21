using Eventos.IO.Domain.Core.Notifications;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evento.IO.Site.Areas.Identity.Pages.Account.PageModels
{
    public class BasePageModel : PageModel
    {
        private readonly IDomainNotificationHandler<DomainNotification> _notification;

        public BasePageModel(IDomainNotificationHandler<DomainNotification> notification)
        {
            _notification = notification;
        }

        protected bool OperacaoValida()
        {
            return (!_notification.HasNotifications());
        }
    }
}
