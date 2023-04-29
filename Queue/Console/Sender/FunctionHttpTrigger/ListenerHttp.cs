using Azure;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Sender;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FunctionHttpTrigger
{
    public class ListenerHttp
    {
        private readonly ILogger<ListenerHttp> _logger;

        public ListenerHttp(ILogger<ListenerHttp> log)
        {
            _logger = log;
        }

        [FunctionName("ListenerHttp")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            return await SendMessageAndReturnOkResult(requestBody);
        }

        private async Task<IActionResult> SendMessageAndReturnOkResult(string requestBody)
        {
            var sendReceipt = await SendMessage(requestBody);
            return new OkObjectResult($"Mensagem com o ID {sendReceipt.Value.MessageId} enviada!");
        }

        private async Task<Response<SendReceipt>> SendMessage(string responseMessage)
        {
            var queueManager = QueueManager.CreateInstace("fila-teste");
            return await queueManager.InsertMessage(responseMessage);
        }
    }
}

