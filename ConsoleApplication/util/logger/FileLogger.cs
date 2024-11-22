using Microsoft.Extensions.Logging;

namespace ConsoleApplication.util.logger
{
    public class FileLogger : ILogger
    {
        private readonly string filePath;

        public FileLogger(string filePath)
        {
            this.filePath = filePath;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        /**
         * <summary>Function that saves logs to specified file.</summary>
         */
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));

            string message = formatter(state, exception);
            string logEntry = $"{DateTime.Now} [{logLevel}] {message}{Environment.NewLine}";

            if (exception != null)
            {
                logEntry += $"{exception}{Environment.NewLine}";
            }

            lock (filePath)
            {
                File.AppendAllText(filePath, logEntry);
            }
        }
    }
}
