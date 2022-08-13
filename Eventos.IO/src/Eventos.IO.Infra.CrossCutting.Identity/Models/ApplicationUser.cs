using Eventos.IO.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Eventos.IO.Infra.CrossCutting.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
    }
}
