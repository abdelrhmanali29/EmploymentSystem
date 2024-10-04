using Microsoft.Extensions.Logging;

namespace EmploymentSystem.Utilities
{
    public class LoggingHelper<T>
    {
        private readonly ILogger<T> _logger;

        public LoggingHelper(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogError(string message, Exception ex)
        {
            _logger.LogError(ex, message);
        }
    }
}
