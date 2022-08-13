using Evento.IO.Site.Areas.Identity.Pages.Account.ViewModel;
using Eventos.IO.Domain.Core.Bus;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Interfaces;
using Eventos.IO.Domain.Organizadores.Commands;
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
            IUser user,
            UserManager<ApplicationUser> userManager,
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

        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid) return Response(model);

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var registroCommand = new RegistrarOrganizadorCommand(Guid.Parse(user.Id), model.Nome, model.CPF, user.Email);
                _bus.SendCommand(registroCommand);

                if (!OperacaoValida())
                {
                    await _userManager.DeleteAsync(user);
                    return Response(model);
                }

                _logger.LogInformation(1, "Usuário criado com sucesso!");
                return Ok(model);
            }

            return Response(model);
        }


    }
}
