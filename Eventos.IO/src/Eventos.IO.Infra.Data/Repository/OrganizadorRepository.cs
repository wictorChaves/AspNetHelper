using Eventos.IO.Domain.Organizadores;
using Eventos.IO.Domain.Organizadores.Repository;
using Eventos.IO.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Eventos.IO.Infra.Data.Repository
{
    public class OrganizadorRepository : Repository<Organizador>, IOrganizadorRepository
    {
        public OrganizadorRepository(EventosContext context) : base(context)
        {
        }
    }
}
