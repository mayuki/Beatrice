using Beatrice.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utf8Json;
using Utf8Json.Internal;

namespace Beatrice.Formatters
{
    public class IntentRequestFormatter : IJsonFormatter<IntentRequest>
    {
        private static readonly AutomataDictionary _automataRequest = new AutomataDictionary();
        private static readonly AutomataDictionary _automataIntent = new AutomataDictionary();

        static IntentRequestFormatter()
        {
            _automataRequest.Add("intent", 0);
            _automataRequest.Add("payload", 1);

            _automataIntent.Add("action.devices.EXECUTE", 0);
            _automataIntent.Add("action.devices.QUERY", 1);
            _automataIntent.Add("action.devices.SYNC", 2);
        }

        public IntentRequest Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull()) return null;

            ArraySegment<byte> intentName = default(ArraySegment<byte>);
            ArraySegment<byte> valueSegment = default(ArraySegment<byte>);

            // Read intent and payload
            var count = 0;
            while (reader.ReadIsInObject(ref count))
            {
                var propName = reader.ReadPropertyNameSegmentRaw();
                var i = -1;
                _automataRequest.TryGetValue(propName, out i);
                switch (i)
                {
                    case 0: // intent
                        intentName = reader.ReadStringSegmentRaw();
                        break;
                    case 1: // payload
                        valueSegment = reader.ReadNextBlockSegment();
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }
            }

            if (intentName.Array == null) return null;

            IPayload payload = null;
            var i2 = -1;
            _automataIntent.TryGetValue(intentName, out i2);
            switch (i2)
            {
                case 0: // action.devices.EXECUTE
                    {
                        var childReader = new JsonReader(valueSegment.Array, valueSegment.Offset);
                        payload = formatterResolver.GetFormatter<ExecutePayload>().Deserialize(ref childReader, formatterResolver);
                    }
                    break;
                case 1: // action.devices.QUERY
                    {
                        var childReader = new JsonReader(valueSegment.Array, valueSegment.Offset);
                        payload = formatterResolver.GetFormatter<StatesPayload>().Deserialize(ref childReader, formatterResolver);
                    }
                    break;
                case 2: // action.devices.SYNC
                    payload = new SyncPayload();
                    break;
            }

            return new IntentRequest
            {
                Intent = Encoding.UTF8.GetString(intentName.Array, intentName.Offset, intentName.Count),
                Payload = payload,
            };
        }

        public void Serialize(ref JsonWriter writer, IntentRequest value, IJsonFormatterResolver formatterResolver)
        {
            throw new NotImplementedException();
        }
    }

}
