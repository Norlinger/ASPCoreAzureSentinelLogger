using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPCoreAzureSentinelLoggerProvider
{
    /// <summary>
    /// 
    /// </summary>
    public class AzureSentinelLoggerOptions
    {
        public AzureSentinelLoggerOptions()
        {
        }

        public string WorkSpaceId { get; set; } = String.Empty;
        public string WorkSpaceKey { get; set; } = String.Empty;
        public string EndPoint { get; set; } = ".ods.opinsights.azure.com/api/logs?api-version=2016-04-01";
        public string LogName { get; set; } = System.AppDomain.CurrentDomain.FriendlyName;

        public LogLevel LogLevel { get; set; } = LogLevel.Warning;

        /// <summary>
        /// Construct the URL as String where the Log Entry should be send to where TenantId is your Id and Endpoint is the Azure URL Standard: .ods.opinsights.azure.com/api/logs?api-version=2016-04-01
        /// </summary>
        /// <returns>The URL https://(TeantId)(EndPoint)</returns>
        public string GetFullUrl()
        {
            return $"https://{WorkSpaceId}{EndPoint}";

        }
    }
}
