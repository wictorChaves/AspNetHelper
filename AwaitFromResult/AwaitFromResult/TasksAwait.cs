using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwaitFromResult
{
    public class TasksAwait : ITasks
    {
        public async Task<string> Make(string description, int millisecondsDelay)
        {
            Console.WriteLine("Fazendo " + description + " ...");
            await Task.Delay(millisecondsDelay);
            Console.WriteLine(description + " ...Pronto");
            return "Pronto";
        }
    }
}
