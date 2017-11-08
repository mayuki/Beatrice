using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Beatrice.Formatters
{
    public class BeatriceCompositeResolver : IJsonFormatterResolver
    {
        public static IJsonFormatterResolver Instance = new BeatriceCompositeResolver();

        static readonly IJsonFormatter[] formatters = new IJsonFormatter[]
        {
            new IntentRequestFormatter(),
            new ActionCommandFormatter()
        };

        static readonly IJsonFormatterResolver[] resolvers = new[]
        {
            StandardResolver.ExcludeNullCamelCase
        };

        BeatriceCompositeResolver()
        {
        }

        public IJsonFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly IJsonFormatter<T> formatter;

            static FormatterCache()
            {
                foreach (var item in formatters)
                {
                    foreach (var implInterface in item.GetType().GetTypeInfo().ImplementedInterfaces)
                    {
                        var ti = implInterface.GetTypeInfo();
                        if (ti.IsGenericType && ti.GenericTypeArguments[0] == typeof(T))
                        {
                            formatter = (IJsonFormatter<T>)item;
                            return;
                        }
                    }
                }

                foreach (var item in resolvers)
                {
                    var f = item.GetFormatter<T>();
                    if (f != null)
                    {
                        formatter = f;
                        return;
                    }
                }
            }
        }
    }
}
