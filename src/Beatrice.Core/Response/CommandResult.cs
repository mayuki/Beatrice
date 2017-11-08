using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beatrice.Response
{

    public class CommandResult
    {
        public string[] Ids { get; set; }
        public string Status { get; set; }
        public string ErrorCode { get; set; }
        public Dictionary<string, object> States { get; set; }
    }

}
