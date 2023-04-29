using Azure;
using Azure.Storage.Queues.Models;
using Sender;

using (var queueManager = QueueManager.CreateInstace("fila-teste"))
{
    Console.WriteLine("Press any key to stop...");
    Console.ReadKey();

    Console.WriteLine($"Fila criada");
    Console.WriteLine($"Quantidade de mensagens na fila {await queueManager.GetLengthAsync()}");

    var sendReceipt = await queueManager.InsertMessage("Hello, World");
    Console.WriteLine($"Mensagem adicionada");
    Console.WriteLine($"Quantidade de mensagens na fila {await queueManager.GetLengthAsync()}");

    sendReceipt = await queueManager.InsertMessage("Hello, World, Again");
    Console.WriteLine($"Mais uma mensagem adicionada");
    Console.WriteLine($"Quantidade de mensagens na fila {await queueManager.GetLengthAsync()}");

    Console.WriteLine("Press any key to stop...");
    Console.ReadKey();
}
