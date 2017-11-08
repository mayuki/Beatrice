using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Beatrice.Request;

namespace Beatrice.Device
{
    public static class DeviceFeatureHelper
    {
        private static ConcurrentDictionary<Type, Func<object, DeviceFeatureInvocationContext, ActionCommand, Task>> _invokerCache
            = new ConcurrentDictionary<Type, Func<object, DeviceFeatureInvocationContext, ActionCommand, Task>>();

        public static Task InvokeAsync(DeviceFeature deviceFeature, DeviceFeatureInvocationContext ctx, ActionCommand actionCommand)
        {
            var actionCommandType = actionCommand.GetType();

            var invoker = _invokerCache.GetOrAdd(actionCommandType, _ =>
            {
                var actionCommandSupportType = typeof(IActionCommandSupport<>).MakeGenericType(actionCommandType);

                var method = actionCommandSupportType.GetMethod("InvokeAsync");
                var thisArg = Expression.Parameter(typeof(object));
                var arg0 = Expression.Parameter(typeof(DeviceFeatureInvocationContext));
                var arg1 = Expression.Parameter(typeof(ActionCommand));

                // (thisArg, arg0, arg1) => (thisArg is IActionCommandSupport<T>)
                //     ? ((IActionCommandSupport<T>)thisArg).InvokeAsync(arg0, (<ActionCommand>)arg1)
                //     : Task.CompletedTask;
                var call = Expression.Condition(Expression.TypeIs(thisArg, actionCommandSupportType),
                    Expression.Call(Expression.Convert(thisArg, actionCommandSupportType), method, arg0, Expression.Convert(arg1, actionCommandType)),
                    Expression.Property(null, typeof(Task).GetProperty("CompletedTask")));
                var lambda = Expression.Lambda(call, thisArg, arg0, arg1);

                return (Func<object, DeviceFeatureInvocationContext, ActionCommand, Task>)lambda.Compile();
            });

            return invoker(deviceFeature.Instance, ctx, actionCommand);
        }

        public static DeviceFeature Create(string typeName, IConfigurationSection options)
        {
            var t = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetType(typeName)).FirstOrDefault(x => x != null);
            if (t == null)
            {
                throw new ArgumentException(String.Format("DeviceFeature '{0}' is not supported.", typeName), nameof(typeName));
            }

            var parameters = t.GetConstructors().First().GetParameters();
            if (parameters.Length == 1)
            {
                return new DeviceFeature(Activator.CreateInstance(t, new[] { options.Get(parameters[0].ParameterType) }));
            }
            else if (parameters.Length == 0)
            {
                return new DeviceFeature(Activator.CreateInstance(t));
            }
            else
            {
                throw new Exception("Parameters of device constructor must be one Options or none.");
            }
        }
    }

}
