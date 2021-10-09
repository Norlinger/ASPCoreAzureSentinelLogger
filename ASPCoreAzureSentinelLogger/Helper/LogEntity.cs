using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ASPCoreAzureSentinelLoggerProvider
{

    /// <summary>
    /// The Fileds written to the Sentinel Log
    /// </summary>
    public class LogEntity
    {
        public LogEntity()
        {
        }

        public string LogLevel { get; set; } = String.Empty;
        public string EventId { get; set; } = String.Empty;
        public string Message { get; set; } = String.Empty;


        /// <summary>
        /// Convert the Logentry to Json
        /// </summary>
        /// <returns>json string</returns>
        public string GetAsJson()
        {
            string jsonString = JsonSerializer.Serialize(this);

            return jsonString;
        }
    }
}
