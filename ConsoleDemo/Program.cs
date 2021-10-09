// See https://aka.ms/new-console-template for more information


using ASPCoreAzureSentinelLoggerProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

Console.WriteLine("Hello, World!");
IConfiguration config = new ConfigurationBuilder().AddJsonFile("AppSettings.json", optional: true).AddUserSecrets<Program>().Build();
AzureSentinelLoggerOptions azureSentinelLoggerOptions= new AzureSentinelLoggerOptions();
config.GetSection("AzureSentinelLoggerOptions").Bind(azureSentinelLoggerOptions);

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddProvider(new AzureSentinelLoggerProvider(azureSentinelLoggerOptions));


});
ILogger logger = loggerFactory.CreateLogger<Program>();

logger.LogCritical("Error in the System Critical");
logger.LogError("Error in the System Error");
logger.LogInformation("Error in the System Information");
logger.LogWarning("Error in the System Warning");
