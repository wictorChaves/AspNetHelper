using AutoMapper;
using Eventos.IO.Application.AutoMapper;
using Eventos.IO.Application.Interfaces;
using Eventos.IO.Application.Services;
using Eventos.IO.Domain.Core.Bus;
using Eventos.IO.Domain.Core.Events;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Eventos.Commands;
using Eventos.IO.Domain.Eventos.Events;
using Eventos.IO.Domain.Eventos.Repository;
using Eventos.IO.Domain.Interfaces;
using Eventos.IO.Domain.Organizadores.Commands;
using Eventos.IO.Domain.Organizadores.Events;
using Eventos.IO.Domain.Organizadores.Repository;
using Eventos.IO.Infra.CrossCutting.AspNetFilters;
using Eventos.IO.Infra.CrossCutting.Bus;
using Eventos.IO.Infra.CrossCutting.Identity.Models;
using Eventos.IO.Infra.Data.Context;
using Eventos.IO.Infra.Data.Repository;
using Eventos.IO.Infra.Data.UoW;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventos.IO.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            _aspNet(services);
            _application(services);
            _domainCommands(services);
            _domainEventos(services);
            _infraData(services);
            _infraBus(services);
            _infraFilter(services);
            _identity(services);
        }

        private static void _aspNet(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        private static void _application(IServiceCollection services)
        {
            services.AddSingleton<IConfigurationProvider>(AutoMapperConfiguration.RegisterMappings());
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));
            services.AddScoped<IEventoAppService, EventoAppService>();
            services.AddScoped<IOrganizadorAppService, OrganizadorAppService>();
        }

        private static void _domainCommands(IServiceCollection services)
        {
            services.AddScoped<IHandler<RegistrarEventoCommand>, EventoCommandHandler>();
            services.AddScoped<IHandler<AtualizarEventoCommand>, EventoCommandHandler>();
            services.AddScoped<IHandler<ExcluirEventoCommand>, EventoCommandHandler>();
            services.AddScoped<IHandler<AtualizarEnderecoEventoCommand>, EventoCommandHandler>();
            services.AddScoped<IHandler<IncluirEnderecoEventoCommand>, EventoCommandHandler>();
            services.AddScoped<IHandler<RegistrarOrganizadorCommand>, OrganizadorCommandHandler>();
        }

        private static void _domainEventos(IServiceCollection services)
        {
            services.AddScoped<IDomainNotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<IHandler<EventoRegistradoEvent>, EventoEventHandler>();
            services.AddScoped<IHandler<EventoAtualizadoEvent>, EventoEventHandler>();
            services.AddScoped<IHandler<EventoExcluidoEvent>, EventoEventHandler>();
            services.AddScoped<IHandler<EnderecoEventoAtualizadoEvent>, EventoEventHandler>();
            services.AddScoped<IHandler<EnderecoEventoAdicionadoEvent>, EventoEventHandler>();
            services.AddScoped<IHandler<OrganizadorRegistradoEvent>, OrganizadorEventHandler>();
        }

        private static void _infraData(IServiceCollection services)
        {
            services.AddScoped<IEventoRepository, EventoRepository>();
            services.AddScoped<IOrganizadorRepository, OrganizadorRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<EventosContext>();
        }

        private static void _infraBus(IServiceCollection services)
        {
            services.AddScoped<IBus, InMemoryBus>();
        }

        private static void _infraFilter(IServiceCollection services)
        {
            services.AddScoped<ILogger<GlobalExceptionHandlingFilter>, Logger<GlobalExceptionHandlingFilter>>();
            services.AddScoped<GlobalExceptionHandlingFilter>();
        }

        private static void _identity(IServiceCollection services)
        {
            services.AddScoped<IUser, AspNetUser>();
        }
    }
}
