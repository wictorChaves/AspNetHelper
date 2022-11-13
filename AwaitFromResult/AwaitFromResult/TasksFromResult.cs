using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwaitFromResult
{
    public class TasksFromResult : ITasks
    {
        public Task<string> Make(string description, int millisecondsDelay)
        {
            Console.WriteLine("Fazendo " + description + " ...");
            Task.Delay(millisecondsDelay).Wait();
            Console.WriteLine(description + " ...Pronto");
            return Task.FromResult("Pronto");
        }
    }
}
