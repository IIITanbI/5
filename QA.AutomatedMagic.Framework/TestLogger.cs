namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TestInfo;

    public class TestLogger : ILogger
    {
        public string Name { get; }
        public List<LogItem> LogMessages { get; private set; } = new List<LogItem>();
        private object _lock = new object();

        private Dictionary<ILogger, LogLevel> _loggers = new Dictionary<ILogger, LogLevel>();

        public TestLogger(string name)
        {
            Name = name;
        }
        public void AddLogger(ILogger logger, LogLevel level)
        {
            _loggers.Add(logger, level);
        }

        #region Simple actions
        public void TRACE(string message, Exception exception = null)
        {
            LOG(LogLevel.TRACE, message, exception);
        }
        public void TRACE(string message, LoggedFileType fileType, string filePath)
        {
            LOG(LogLevel.TRACE, message, fileType, filePath);
        }

        public void DEBUG(string message, Exception exception = null)
        {
            LOG(LogLevel.DEBUG, message, exception);
        }
        public void DEBUG(string message, LoggedFileType fileType, string filePath)
        {
            LOG(LogLevel.DEBUG, message, fileType, filePath);
        }

        public void WARN(string message, Exception exception = null)
        {
            LOG(LogLevel.WARN, message, exception);
        }
        public void WARN(string message, LoggedFileType fileType, string filePath)
        {
            LOG(LogLevel.WARN, message, fileType, filePath);
        }

        public void INFO(string message, Exception exception = null)
        {
            LOG(LogLevel.INFO, message, exception);
        }
        public void INFO(string message, LoggedFileType fileType, string filePath)
        {
            LOG(LogLevel.INFO, message, fileType, filePath);
        }

        public void ERROR(string message, Exception exception = null)
        {
            LOG(LogLevel.ERROR, message, exception);
        }
        public void ERROR(string message, LoggedFileType fileType, string filePath)
        {
            LOG(LogLevel.ERROR, message, fileType, filePath);
        }
        #endregion

        public void LOG(LogLevel level, string message, Exception exception = null)
        {
            lock (_lock)
            {
                if (exception == null)
                    Console.WriteLine($"{Name}\t{level}\t{message}");
                else
                    Console.WriteLine($"{Name}\t{level}\t{message}\nException:\n{exception}");

                var logMessage = new LogMessage { DataStemp = DateTime.Now, Level = level, Message = message, Ex = exception };
                LogMessages.Add(logMessage);

                foreach (var logger in _loggers)
                {
                    if (logger.Value <= level)
                        switch (level)
                        {
                            case LogLevel.TRACE:
                                logger.Key.TRACE(message, exception);
                                break;
                            case LogLevel.DEBUG:
                                logger.Key.DEBUG(message, exception);
                                break;
                            case LogLevel.WARN:
                                logger.Key.WARN(message, exception);
                                break;
                            case LogLevel.INFO:
                                logger.Key.INFO(message, exception);
                                break;
                            case LogLevel.ERROR:
                                logger.Key.ERROR(message, exception);
                                break;
                            default:
                                break;
                        }
                }
            }
        }
        public void LOG(LogLevel level, string message, LoggedFileType fileType, string filePath)
        {
            lock (_lock)
            {
                var logMessage = new LogFile { DataStemp = DateTime.Now, Level = level, FileType = fileType, FilePath = filePath };
                LogMessages.Add(logMessage);

                foreach (var logger in _loggers)
                {
                    if (logger.Value <= level)
                        switch (level)
                        {
                            case LogLevel.TRACE:
                                logger.Key.TRACE(message, fileType, filePath);
                                break;
                            case LogLevel.DEBUG:
                                logger.Key.DEBUG(message, fileType, filePath);
                                break;
                            case LogLevel.WARN:
                                logger.Key.WARN(message, fileType, filePath);
                                break;
                            case LogLevel.INFO:
                                logger.Key.INFO(message, fileType, filePath);
                                break;
                            case LogLevel.ERROR:
                                logger.Key.ERROR(message, fileType, filePath);
                                break;
                            default:
                                break;
                        }
                }
            }
        }
    }
}
