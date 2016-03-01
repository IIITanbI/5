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
        public ILogger PARENT { get; private set; } = null;
        public string Name { get; private set; }
        private Dictionary<ILogger, LogLevel> _loggers = new Dictionary<ILogger, LogLevel>();
        public List<LogMessage> LogMessages { get; private set; } = new List<LogMessage>();

        public TestLogger(string name, string itemType)
        {
            Name = $"({itemType}) {name}";
        }
        public void AddLogger(ILogger logger, LogLevel level)
        {
            _loggers.Add(logger, level);
        }

        public void TRACE(string message, Exception exception = null)
        {
            LOG(LogLevel.TRACE, message, exception);
        }
        public void TRACE(string message, AttachedItemType itemType, string itemPath)
        {
            LOG(LogLevel.TRACE, message, itemType, itemPath);
        }
        public void DEBUG(string message, Exception exception = null)
        {
            LOG(LogLevel.DEBUG, message, exception);
        }
        public void DEBUG(string message, AttachedItemType itemType, string itemPath)
        {
            LOG(LogLevel.DEBUG, message, itemType, itemPath);
        }
        public void WARN(string message, Exception exception = null)
        {
            LOG(LogLevel.WARN, message, exception);
        }
        public void WARN(string message, AttachedItemType itemType, string itemPath)
        {
            LOG(LogLevel.WARN, message, itemType, itemPath);
        }
        public void INFO(string message, Exception exception = null)
        {
            LOG(LogLevel.INFO, message, exception);
        }
        public void INFO(string message, AttachedItemType itemType, string itemPath)
        {
            LOG(LogLevel.INFO, message, itemType, itemPath);
        }
        public void ERROR(string message, Exception exception = null)
        {
            LOG(LogLevel.ERROR, message, exception);
        }
        public void ERROR(string message, AttachedItemType itemType, string itemPath)
        {
            LOG(LogLevel.ERROR, message, itemType, itemPath);
        }

        public string GetFullName()
        {
            if (PARENT == null)
                return Name;

            return $"{PARENT.GetFullName()} $ {Name}";
        }

        public void LOG(LogLevel level, string message, Exception exception = null)
        {
            var logMessage = new LogMessage { DataStemp = DateTime.Now, Level = level, Message = message, Exception = exception };
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
        public void LOG(LogLevel level, string message, AttachedItemType itemType, string itemPath)
        {
            var logMessage = new LogMessage { DataStemp = DateTime.Now, Level = level, Message = message, ItemType = itemType, ItemPath = itemPath };
            LogMessages.Add(logMessage);

            foreach (var logger in _loggers)
            {
                if (logger.Value <= level)
                    switch (level)
                    {
                        case LogLevel.TRACE:
                            logger.Key.TRACE(message, itemType, itemPath);
                            break;
                        case LogLevel.DEBUG:
                            logger.Key.DEBUG(message, itemType, itemPath);
                            break;
                        case LogLevel.WARN:
                            logger.Key.WARN(message, itemType, itemPath);
                            break;
                        case LogLevel.INFO:
                            logger.Key.INFO(message, itemType, itemPath);
                            break;
                        case LogLevel.ERROR:
                            logger.Key.ERROR(message, itemType, itemPath);
                            break;
                        default:
                            break;
                    }
            }
        }

        public void SetParent(ILogger logger)
        {
            PARENT = logger;
        }

        public void SpamTo(ILogger logger)
        {
            foreach (var logMessage in LogMessages)
            {
                if (logMessage.ItemType != AttachedItemType.NONE)
                {
                    switch (logMessage.Level)
                    {
                        case LogLevel.TRACE:
                            logger.TRACE(logMessage.Message, logMessage.ItemType, logMessage.ItemPath);
                            break;
                        case LogLevel.DEBUG:
                            logger.DEBUG(logMessage.Message, logMessage.ItemType, logMessage.ItemPath);
                            break;
                        case LogLevel.WARN:
                            logger.WARN(logMessage.Message, logMessage.ItemType, logMessage.ItemPath);
                            break;
                        case LogLevel.INFO:
                            logger.INFO(logMessage.Message, logMessage.ItemType, logMessage.ItemPath);
                            break;
                        case LogLevel.ERROR:
                            logger.ERROR(logMessage.Message, logMessage.ItemType, logMessage.ItemPath);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (logMessage.Level)
                    {
                        case LogLevel.TRACE:
                            logger.TRACE(logMessage.Message, logMessage.Exception);
                            break;
                        case LogLevel.DEBUG:
                            logger.DEBUG(logMessage.Message, logMessage.Exception);
                            break;
                        case LogLevel.WARN:
                            logger.WARN(logMessage.Message, logMessage.Exception);
                            break;
                        case LogLevel.INFO:
                            logger.INFO(logMessage.Message, logMessage.Exception);
                            break;
                        case LogLevel.ERROR:
                            logger.ERROR(logMessage.Message, logMessage.Exception);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
