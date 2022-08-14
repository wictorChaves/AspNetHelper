using Azure;
using Azure.Storage.Queues.Models;
using Sender;

using (var queueManager = QueueManager.CreateInstace("fila-teste"))
{
    Console.WriteLine($"Fila criada");
    Console.WriteLine($"Quantidade de mensagens na fila {await queueManager.GetLengthAsync()}");

    var sendReceipt = await queueManager.InsertMessage("Hello, World");
    Console.WriteLine($"Mensagem adicionada");
    Console.WriteLine($"Quantidade de mensagens na fila {await queueManager.GetLengthAsync()}");

    sendReceipt = await queueManager.InsertMessage("Hello, World, Again");
    Console.WriteLine($"Mais uma mensagem adicionada");
    Console.WriteLine($"Quantidade de mensagens na fila {await queueManager.GetLengthAsync()}");

    var messageQuantity = await queueManager.GetLengthAsync();
    Response<PeekedMessage[]> peekMessages = await queueManager.PeekMessages(messageQuantity);
    Console.WriteLine($"Mensagens olhadas na fila:");
    foreach (var peekMessage in peekMessages.Value)
    {
        Console.WriteLine($" - '{peekMessage.Body}'");
    }
    Console.WriteLine($"Quantidade de mensagens na fila {await queueManager.GetLengthAsync()}");
    Console.WriteLine($"Quantidade de mensagens entregues {peekMessages.Value.Select(x => x.DequeueCount).Sum()}");

    Console.WriteLine($"Atualizando a primeira mensagem");
    QueueMessage uploadMessage = (await queueManager.ReceiveMessages()).Value[0];
    var updateReceipt = await queueManager.UpdateMessage(uploadMessage, "Não é mais um \"Hello, World\"");

    Console.WriteLine($"Atualizando a primeira mensagem");
    Task.Delay(10000).Wait();

    messageQuantity = await queueManager.GetLengthAsync();
    Response<QueueMessage[]> queueMessages = await queueManager.ReceiveMessages(messageQuantity);
    Console.WriteLine($"Mensagens pegas na fila:");
    foreach (var queueMessage in queueMessages.Value)
    {
        Console.WriteLine($" - '{queueMessage.Body}'");
    }
    Console.WriteLine($"Quantidade de mensagens na fila {await queueManager.GetLengthAsync()}");
    Console.WriteLine($"Quantidade de mensagens entregues {queueMessages.Value.Select(x => x.DequeueCount).Sum()}");

    Console.WriteLine($"Deletando todas as mensagem");
    Response response;
    foreach (var queueMessage in queueMessages.Value)
    {
        response = await queueManager.DequeueMessage(queueMessage);
    }
    Console.WriteLine($"Quantidade de mensagens na fila {await queueManager.GetLengthAsync()}");
}
