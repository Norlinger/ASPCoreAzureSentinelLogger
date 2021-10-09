using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ASPCoreAzureSentinelLoggerProvider
{
    public class AzureSentinelLogger : ILogger
    {
        protected AzureSentinelLoggerOptions Options { get; set; } = new AzureSentinelLoggerOptions();
        public AzureSentinelLogger(AzureSentinelLoggerOptions options)
        {
            this.Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == Options.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //if (!IsEnabled(logLevel))
            //    return;

            string dateTime = DateTime.UtcNow.ToString("R");
            var log = new LogEntity

            {
                EventId = eventId.ToString(),
                LogLevel = logLevel.ToString(),
                Message = formatter(state, exception),
            };
    string jsonString = log.GetAsJson();
    string signature = ConstructSignature(dateTime, jsonString.Length, Options);

    var task = Task.Run(async () => await SendDataToSentinelAsync(signature, dateTime, jsonString, Options));
    task.Wait();
        }

private static  async Task SendDataToSentinelAsync(string signature, string dateTime, string jsonString, AzureSentinelLoggerOptions options)
{
    try
    {
        string url = options.GetFullUrl();

        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Log-Type", options.LogName);
        client.DefaultRequestHeaders.Add("Authorization", signature);
        client.DefaultRequestHeaders.Add("x-ms-date", dateTime);
        client.DefaultRequestHeaders.Add("time-generated-field", "");

        System.Net.Http.HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8);
        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await client.PostAsync(new Uri(url), httpContent);
        Console.WriteLine("Return Result: " + response.ReasonPhrase);
    }
    catch (Exception ex)
    {
        Console.WriteLine("API Post Exception: " + ex.Message);
    }
}

private static string ConstructSignature(string dateTime, int length, AzureSentinelLoggerOptions options)
{

    string message = "POST\n" + length + "\napplication/json\n" + "x-ms-date:" + dateTime + "\n/api/logs";
    var encoding = new System.Text.ASCIIEncoding();
    byte[] keyByte = Convert.FromBase64String(options.WorkSpaceKey);
    byte[] messageBytes = encoding.GetBytes(message);
    using (var hmacsha256 = new HMACSHA256(keyByte))
    {
        byte[] hash = hmacsha256.ComputeHash(messageBytes);
        return "SharedKey " + options.WorkSpaceId + ":" + Convert.ToBase64String(hash);
    }
}
    }

    public static class AzureSentinelLoggerExtensions
{
    public static ILoggerFactory AddAzureSentinelLogger(this ILoggerFactory loggerFactory, AzureSentinelLoggerOptions options)
    {
        loggerFactory.AddProvider(new AzureSentinelLoggerProvider(options));
        return loggerFactory;
    }
}
}
