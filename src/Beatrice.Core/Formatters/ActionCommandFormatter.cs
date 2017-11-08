using Beatrice.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utf8Json;
using Utf8Json.Internal;

namespace Beatrice.Formatters
{
    public class ActionCommandFormatter : IJsonFormatter<ActionCommand>
    {
        private static readonly AutomataDictionary _automata = new AutomataDictionary();

        private delegate ActionCommand ActionCommandDeserializeDelegate(ref ArraySegment<byte> arraySegment, IJsonFormatterResolver formatterResolver);
        private Dictionary<string, ActionCommandDeserializeDelegate> _deserializeByAction;

        static ActionCommandFormatter()
        {
            _automata.Add("command", 0);
            _automata.Add("params", 1);
        }

        public ActionCommandFormatter()
        {
            // TODO: AutomataDictionary
            _deserializeByAction = ActionCommand.ByCommand
                .ToDictionary(k => k.Key, v =>
                {
                    var methodDeserialize = typeof(ActionCommandFormatter)
                        .GetMethod("_Deserialize", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
                    var methodDeserializeOfT = methodDeserialize.MakeGenericMethod(v.Value);

                    return (ActionCommandDeserializeDelegate)methodDeserializeOfT.CreateDelegate(typeof(ActionCommandDeserializeDelegate));
                });
        }

        private static ActionCommand _Deserialize<T>(ref ArraySegment<byte> arraySegment, IJsonFormatterResolver formatterResolver)
            where T:  ActionCommand, new()
        {
            if (arraySegment.Array == null)
            {
                return new T();
            }

            var childReader = new JsonReader(arraySegment.Array, arraySegment.Offset);
            return formatterResolver.GetFormatter<T>().Deserialize(ref childReader, formatterResolver);
        }


        public ActionCommand Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull()) return null;

            var typeName = String.Empty;
            ArraySegment<byte> valueSegment = default(ArraySegment<byte>);

            var count = 0;
            while (reader.ReadIsInObject(ref count))
            {
                var propName = reader.ReadPropertyNameSegmentRaw();
                var i = -1;
                _automata.TryGetValue(propName, out i);
                switch (i)
                {
                    case 0:
                        typeName = reader.ReadString();
                        break;
                    case 1:
                        valueSegment = reader.ReadNextBlockSegment();
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }
            }

            if (typeName == String.Empty) return null;

            ActionCommand value;
            if (_deserializeByAction.TryGetValue(typeName, out var d))
            {
                value = d(ref valueSegment, formatterResolver);
            }
            else
            {
                var childReader = new JsonReader(valueSegment.Array, valueSegment.Offset);
                value = formatterResolver.GetFormatter<ActionCommand.Generic>().Deserialize(ref childReader, formatterResolver);
            }

            value.Command = typeName;

            return value;
        }

        public void Serialize(ref JsonWriter writer, ActionCommand value, IJsonFormatterResolver formatterResolver)
        {
            throw new NotImplementedException();
        }
    }

}
