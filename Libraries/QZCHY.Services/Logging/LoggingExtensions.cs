using System;
using QZCHY.Core.Domain.AccountUsers;
using QZCHY.Core.Domain.Logging;

namespace QZCHY.Services.Logging
{
    public static class LoggingExtensions
    {
        public static void Debug(this ILogger logger, string message, Exception exception = null, AccountUser property = null)
        {
            FilteredLog(logger, LogLevel.Debug, message, exception, property);
        }
        public static void Information(this ILogger logger, string message, Exception exception = null, AccountUser property = null)
        {
            FilteredLog(logger, LogLevel.Information, message, exception, property);
        }
        public static void Warning(this ILogger logger, string message, Exception exception = null, AccountUser property = null)
        {
            FilteredLog(logger, LogLevel.Warning, message, exception, property);
        }
        public static void Error(this ILogger logger, string message, Exception exception = null, AccountUser property = null)
        {
            FilteredLog(logger, LogLevel.Error, message, exception, property);
        }
        public static void Fatal(this ILogger logger, string message, Exception exception = null, AccountUser property = null)
        {
            FilteredLog(logger, LogLevel.Fatal, message, exception, property);
        }

        private static void FilteredLog(ILogger logger, LogLevel level, string message, Exception exception = null, AccountUser property = null)
        {
            //don't log thread abort exception
            if (exception is System.Threading.ThreadAbortException)
                return;

            if (logger.IsEnabled(level))
            {
                string fullMessage = exception == null ? string.Empty : exception.ToString();
                logger.InsertLog(level, message, fullMessage, property);
            }
        }
    }
}
