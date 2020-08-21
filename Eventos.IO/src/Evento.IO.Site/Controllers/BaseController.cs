using System;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Evento.IO.Site.Controllers
{
    public class BaseController : Controller
    {
        private readonly IDomainNotificationHandler<DomainNotification> _notification;
        private readonly IUser _user;

        public Guid OrganizadorId { get; set; }

        public BaseController(IDomainNotificationHandler<DomainNotification> notification, IUser user)
        {
            _notification = notification;
            _user = user;

            if (_user.IsAuthenticated())
                OrganizadorId = _user.GetUserId();
        }

        protected bool OperacaoValida()
        {
            return (!_notification.HasNotifications());
        }
    }
}