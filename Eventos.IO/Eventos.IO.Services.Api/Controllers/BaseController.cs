using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventos.IO.Services.Api.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        private readonly IDomainNotificationHandler<DomainNotification> _notifications;
        protected Guid OrganizadorId { get; set; }

        protected BaseController(
            IDomainNotificationHandler<DomainNotification> notifications,
            IUser user
            )
        {
            _notifications = notifications;
            if (user.IsAuthenticated()) OrganizadorId = user.GetUserId();
        }

        protected new IActionResult Response(object retult = null)
        {
            if (OperacaoValida())
            {
                return Ok(new
                {
                    success = true,
                    data = retult
                });
            }
            return Ok(new
            {
                success = false,
                error = _notifications.GetNotifications().Select(n => n.Value)
            });
        }

        protected bool OperacaoValida()
        {
            return !_notifications.HasNotifications();
        }
    }
}
