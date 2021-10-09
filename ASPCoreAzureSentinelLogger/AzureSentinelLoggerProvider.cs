using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPCoreAzureSentinelLoggerProvider
{
    public class AzureSentinelLoggerProvider : ILoggerProvider
    {
        public AzureSentinelLoggerProvider(AzureSentinelLoggerOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        protected AzureSentinelLoggerOptions Options { get; set; } = new AzureSentinelLoggerOptions();
        private readonly ConcurrentDictionary<string, AzureSentinelLogger> _loggers = new ConcurrentDictionary<string, AzureSentinelLogger>();

        public ILogger CreateLogger(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                categoryName = Guid.NewGuid().ToString();
            }
            return _loggers.GetOrAdd(categoryName, name => new AzureSentinelLogger(Options));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
