using Microsoft.Extensions.Logging;

namespace ConsoleApplication.util.logger
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string logFilePath;
        public FileLoggerProvider(string logFilePath)
        {
            this.logFilePath = logFilePath;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(logFilePath);
        }

        public void Dispose()
        {

        }
    }
}
