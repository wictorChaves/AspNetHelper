﻿using Elmah;
using Elmah.Io.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eventos.IO.Infra.CrossCutting.AspNetFilters
{
    public class GlobalActionLogger : IActionFilter
    {
        private readonly ILogger<GlobalActionLogger> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public GlobalActionLogger(ILogger<GlobalActionLogger> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                var date = new
                {
                    Version = "v1.0",
                    User = context.HttpContext.User.Identity.Name,
                    IP = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Hostname = context.HttpContext.Request.Host.ToString(),
                    AreaAccessed = context.HttpContext.Request.GetDisplayUrl(),
                    Action = context.ActionDescriptor.DisplayName,
                    TimeStamp = DateTime.Now
                };
                var eventId = 1;
                _logger.LogInformation(eventId, date.ToString(), "Log de Auditoria");
            }

            if (_hostingEnvironment.IsProduction())
            {
                var message = new CreateMessage()
                {
                    Version = "v1.0",
                    Application = "Eventos.IO",
                    Source = "GlobalActionLoggerFilter",
                    User = context.HttpContext.User.Identity.Name,
                    Hostname = context.HttpContext.Request.Host.Host,
                    Url = context.HttpContext.Request.GetDisplayUrl(),
                    DateTime = DateTime.Now,
                    Method = context.HttpContext.Request.Method,
                    StatusCode = context.HttpContext.Response.StatusCode,
                    Cookies = context.HttpContext.Request?.Cookies?.Keys.Select(k => new Item(k, context.HttpContext.Request.Cookies[k])).ToList(),
                    Form = Form(context.HttpContext),
                    ServerVariables = context.HttpContext.Request?.Headers?.Keys.Select(k => new Item(k, context.HttpContext.Request.Headers[k])).ToList(),
                    QueryString = context.HttpContext.Request?.Query?.Keys.Select(k => new Item(k, context.HttpContext.Request.Query[k])).ToList(),
                    Data = context.Exception?.ToDataList(),
                    Detail = JsonConvert.SerializeObject(new { DadoExtra = "Dados a mais", DadoInfo = "Pode serum Json" })
                };

                var client = ElmahioAPI.Create("04f3bea824e54f0bb09b8c73d8967186");
                client.Messages.Create(new Guid("bd857b62-d3eb-4d72-8155-6a9d92622a11").ToString(), message);
            }
        }

        private static List<Item> Form(HttpContext httpContext)
        {
            try
            {
                return httpContext.Request?.Form?.Keys.Select(k => new Item(k, httpContext.Request.Form[k])).ToList();
            }
            catch (InvalidOperationException)
            {
                // Request not a form POST or similar
            }

            return null;
        }

        public void OnActionExecuting(ActionExecutingContext context) { }
    }
}
