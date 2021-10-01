using Eventos.IO.Domain.Core.Bus;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Interfaces;
using Eventos.IO.Infra.CrossCutting.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventos.IO.Services.Api.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _singInManager;
        private readonly ILogger _logger;
        private readonly IBus _bus;

        public AccountController(
            IDomainNotificationHandler<DomainNotification> notifications,
            IUser user, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> singInManager,
            IBus bus,
            ILoggerFactory loggerFactory
            ) : base(notifications, user)
        {
            _userManager = userManager;
            _singInManager = singInManager;
            _bus = bus;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }
    }
}
