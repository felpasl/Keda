using Avro.Util;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Consumer.AzureFunction
{

    public class FruitMessage : ISerializer<FruitMessage>
    {
        public DateTime? now { get; set; }
        public Fruit? fruit { get; set; }

        public FruitMessage(DateTime now, Fruit fruit)
        {
            this.now = now;
            this.fruit = fruit;
        }
        public FruitMessage()
        {
        }
        public byte[] Serialize(FruitMessage data, SerializationContext context)
        {
            using (var ms = new MemoryStream())
            {
                var settings = new JsonSerializerSettings();
                settings.ContractResolver = new LowercaseContractResolver();
                var json = JsonConvert.SerializeObject(data, Formatting.Indented, settings);
                var writer = new StreamWriter(ms);

                writer.Write(json);
                writer.Flush();
                ms.Position = 0;

                return ms.ToArray();
            }
        }
    }
    public class LowercaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}
