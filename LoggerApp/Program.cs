using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, config) =>
{
    config.Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName()
        .Enrich.WithAssemblyName()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
        {
            IndexFormat = $"logs-{DateTime.UtcNow:yyyy-dd}",
            AutoRegisterTemplate = true,
            NumberOfShards = 2,
            NumberOfReplicas = 1,
        })
        .ReadFrom.Configuration(context.Configuration);
});


var app = builder.Build();

app.MapGet("/", () =>
{
    return "Hello Farshad Goodarzi";
});

app.Run();