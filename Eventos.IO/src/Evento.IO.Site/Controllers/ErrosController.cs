using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eventos.IO.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Evento.IO.Site.Controllers
{
    public class ErrosController : Controller
    {

        private readonly IUser _user;

        public ErrosController(IUser user)
        {
            _user = user;
        }

        [Route("/erro-de-aplicacao")]
        [Route("/erro-de-aplicacao/{id}")]
        public ActionResult Erros(string id)
        {
            switch (id)
            {
                case "404":
                    return View("NotFound");
                case "403":
                case "401":
                    if (!_user.IsAuthenticated()) return RedirectToAction("Login", "Account");
                    return View("AccessDenied");
            }

            return View("Error");
        }
    }
}