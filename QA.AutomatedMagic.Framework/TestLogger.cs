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
        public string Name { get; private set; }
        public ILogger Parent { get; private set; }
        public List<LogItem> LogMessages { get; private set; } = new List<LogItem>();

        private Dictionary<ILogger, LogLevel> _loggers = new Dictionary<ILogger, LogLevel>();

        public TestLogger(string name, string itemType, ILogger parent = null)
        {
            Parent = parent;
            Name = $"({itemType}) {name}";
        }

        public void SetParent(ILogger logger)
        {
            Parent = logger;
        }

        public void AddLogger(ILogger logger, LogLevel level)
        {
            _loggers.Add(logger, level);
        }

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

        public string GetFullName()
        {
            if (Parent == null)
                return Name;

            return $"{Parent.GetFullName()} $ {Name}";
        }

        public void LOG(LogLevel level, string message, Exception exception = null)
        {
            Console.WriteLine($"{GetFullName()}\t{level}\t{message}");

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
        public void LOG(LogLevel level, string message, LoggedFileType fileType, string filePath)
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

        public void SpamTo(ILogger logger)
        {
            foreach (var logMessage in LogMessages)
            {
                var loggedFile = logMessage as LogFile;
                if (loggedFile != null)
                {
                    switch (loggedFile.Level)
                    {
                        case LogLevel.TRACE:
                            logger.TRACE(loggedFile.Message, loggedFile.FileType, loggedFile.FilePath);
                            break;
                        case LogLevel.DEBUG:
                            logger.DEBUG(loggedFile.Message, loggedFile.FileType, loggedFile.FilePath);
                            break;
                        case LogLevel.WARN:
                            logger.WARN(loggedFile.Message, loggedFile.FileType, loggedFile.FilePath);
                            break;
                        case LogLevel.INFO:
                            logger.INFO(loggedFile.Message, loggedFile.FileType, loggedFile.FilePath);
                            break;
                        case LogLevel.ERROR:
                            logger.ERROR(loggedFile.Message, loggedFile.FileType, loggedFile.FilePath);
                            break;
                        default:
                            break;
                    }
                    continue;
                }

                var loggedMessage = logMessage as LogMessage;
                if (loggedMessage != null)
                {
                    switch (loggedMessage.Level)
                    {
                        case LogLevel.TRACE:
                            logger.TRACE(loggedMessage.Message, loggedMessage.Ex);
                            break;
                        case LogLevel.DEBUG:
                            logger.DEBUG(loggedMessage.Message, loggedMessage.Ex);
                            break;
                        case LogLevel.WARN:
                            logger.WARN(loggedMessage.Message, loggedMessage.Ex);
                            break;
                        case LogLevel.INFO:
                            logger.INFO(loggedMessage.Message, loggedMessage.Ex);
                            break;
                        case LogLevel.ERROR:
                            logger.ERROR(loggedMessage.Message, loggedMessage.Ex);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
