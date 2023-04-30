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

namespace FunctionAppSenderAndReceiver
{
    public class Function
    {
        [FunctionName("Function1")]
        public void Receiver([QueueTrigger("fila-teste", Connection = "")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }

        [FunctionName("ListenerHttp")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Sender([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
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
