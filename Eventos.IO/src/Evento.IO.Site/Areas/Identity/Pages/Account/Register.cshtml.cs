using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Evento.IO.Site.Areas.Identity.Pages.Account.ViewModel;
using Evento.IO.Site.Areas.Identity.Pages.Account.PageModels;
using Eventos.IO.Application.Interfaces;
using Eventos.IO.Application.ViewModels;
using Eventos.IO.Domain.Core.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Evento.IO.Site.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : BasePageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IOrganizadorAppService _organizadorAppService;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IOrganizadorAppService organizadorAppService,
            IDomainNotificationHandler<DomainNotification> notification) : base(notification)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _organizadorAppService = organizadorAppService;
        }

        [BindProperty]
        public RegisterViewModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    _addClaims(user);

                    if (_criaOrganizador(user, Input))
                        return actionFailed();

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return actionFailed();
        }

        private void _addClaims(IdentityUser user)
        {
            _userManager.AddClaimAsync(user, new Claim("Eventos", "Ler"));
            _userManager.AddClaimAsync(user, new Claim("Eventos", "Gravar"));
        }

        private bool _criaOrganizador(IdentityUser user, RegisterViewModel registerViewModel)
        {
            var organizador = new OrganizadorViewModel
            {
                Id = Guid.Parse(user.Id),
                Email = user.Email,
                Nome = registerViewModel.Nome,
                CPF = registerViewModel.CPF
            };

            _organizadorAppService.Registrar(organizador);

            if (OperacaoValida())
                return true;

            _userManager.DeleteAsync(user);
            return false;
        }

        private PageResult actionFailed()
        {
            return Page();
        }
    }
}