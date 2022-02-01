using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var fruitString = File.ReadAllText("fruits.json");
var fruits = System.Text.Json.JsonSerializer.Deserialize<Fruit[]>(fruitString, new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
});


var config = new ProducerConfig
{
    BootstrapServers = Environment.GetEnvironmentVariable("BoostrapServer") ?? "localhost:29092"
};

app.MapGet("/process/{quantity}", (int quantity) =>
{
    using (var producer = new ProducerBuilder<Null, FruitMessage>(config).SetValueSerializer(new FruitMessage()).Build())
    {
        for (int i = 1; i <= quantity; i++)
        {
            var fruitMessage = new FruitMessage
              (
                  DateTime.Now,
                  fruits[Random.Shared.Next(fruits.Length)]
              );
            var result = producer.ProduceAsync(
                "fruits",
                new Message<Null, FruitMessage>
                { Value = fruitMessage });
            result.Wait();
            logger.Information(
                $"Generate : { quantity } fruit message!");
        }
    }

    return quantity;
})
.WithName("GetWeatherForecast");

app.MapGet("/health", () => { return ""; });

app.Run();

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


internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class Fruit
{
    public Fruit(int id, string name, int price, int quantity)
    {
        Id = id;
        Name = name;
        Price = price;
        Quantity = quantity;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
}

public class LowercaseContractResolver : DefaultContractResolver
{
    protected override string ResolvePropertyName(string propertyName)
    {
        return propertyName.ToLower();
    }
}