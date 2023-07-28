using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Kafka;
using Microsoft.Extensions.Logging;

namespace Fruit.Consumer.AzureFunction
{
    public class ConsumeFruit
    {
        // KafkaTrigger sample 
        // Consume the message from "topic" on the LocalBroker.
        // Add `BrokerList` and `KafkaPassword` to the local.settings.json
        // For EventHubs
        // "BrokerList": "{EVENT_HUBS_NAMESPACE}.servicebus.windows.net:9093"
        // "KafkaPassword":"{EVENT_HUBS_CONNECTION_STRING}
        [FunctionName("Function1")]
        public void Run(
            [KafkaTrigger("$Brokers",
                          "fruits",
                          ConsumerGroup = "consumer-azurefunction-processing")] KafkaEventData<FruitMessage> message,
            [Kafka("$Brokers",
                          "fruits-alert")] out KafkaEventData<FruitMessage> outputEvents,
            ILogger log)
        {
            log.LogInformation(">> Processing >> " + message.Value.fruit.name + " " + message.Value.fruit.quantity);

            if (message.Value.fruit.quantity < 50)
            {
                log.LogInformation(">> Buy >>" + message.Value.fruit.name + " " + message.Value.fruit.quantity);
                message.Value.fruit.quantity += 200;
                outputEvents = message;
            }

            outputEvents = null;
            log.LogInformation($"C# Kafka trigger function processed a message: {message.Value.fruit.id}");
        }
    }
}
