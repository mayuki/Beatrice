using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beatrice.Request;

namespace Beatrice.Device
{
    public interface IActionCommandSupport<T>
        where T : ActionCommand
    {
        Task InvokeAsync(DeviceFeatureInvocationContext ctx, T commandParams);
    }
}
