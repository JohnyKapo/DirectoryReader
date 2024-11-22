using Microsoft.Extensions.Logging;

namespace ConsoleApplication.util.logger
{
    /**
     * <summary>
     * Class <b>Logger</b> is a custom logger instance of this project.<br></br>
     * It saves logs to a file located in binary representation of this project bin/Debug.
     * </summary>
     */
    public class Logger
    {
        private ILogger<Program> logger;
        private static Logger instance = null;

        string logFilePath = "console-log.txt";

        public static Logger GetInstance()
        {
            if (instance == null)
            {
                instance = new Logger();
            }
            return instance;
        }

        private Logger()
        {

            ILoggerFactory loggerFactory = LoggerFactory.Create(builder => 
            {
                builder.AddProvider(new FileLoggerProvider(logFilePath));
                builder.SetMinimumLevel(LogLevel.Debug);
            });

            logger = loggerFactory.CreateLogger<Program>();
        }

        public void LogInformation(string message)
        {
            logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            logger.LogWarning(message);
        }

        public void LogError(string message, Exception ex = null)
        {
            logger.LogError(ex, message);
        }

        public void LogDebug(string message)
        {
            logger.LogDebug(message);
        }


    }
}
