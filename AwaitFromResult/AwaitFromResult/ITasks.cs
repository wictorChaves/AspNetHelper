using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwaitFromResult
{
    public interface ITasks
    {
        Task<string> Make(string description, int millisecondsDelay);
    }
}
