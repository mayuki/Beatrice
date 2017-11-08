using System;
using System.Linq;
using System.Threading.Tasks;

namespace Beatrice.Response
{
    public abstract class ActionResponse<T>
    {
        public string RequestId { get; set; }
        public T Payload { get; set; }
    }
}
