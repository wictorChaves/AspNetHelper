using Azure;
using Azure.Storage.Queues.Models;
using Sender;

var queueManager = QueueManager.CreateInstace("fila-teste");

Console.WriteLine("Receive - Press any key to stop...");
Console.ReadKey();

var messageQuantity = await queueManager.GetLengthAsync();
Response<PeekedMessage[]> peekMessages = await queueManager.PeekMessages(messageQuantity);
Console.WriteLine($"Mensagens olhadas na fila:");
foreach (var peekMessage in peekMessages.Value)
{
    Console.WriteLine($" - '{peekMessage.MessageText}'");
}
Console.WriteLine($"Quantidade de mensagens na fila {await queueManager.GetLengthAsync()}");
Console.WriteLine($"Quantidade de mensagens entregues {peekMessages.Value.Select(x => x.DequeueCount).Sum()}");

messageQuantity = await queueManager.GetLengthAsync();
Response<QueueMessage[]> queueMessages = await queueManager.ReceiveMessages(messageQuantity);
Console.WriteLine($"Mensagens pegas na fila:");
foreach (var queueMessage in queueMessages.Value)
{
    Console.WriteLine($" - '{queueMessage.MessageText}'");
}
Console.WriteLine($"Quantidade de mensagens na fila {await queueManager.GetLengthAsync()}");
Console.WriteLine($"Quantidade de mensagens entregues {queueMessages.Value.Select(x => x.DequeueCount).Sum()}");