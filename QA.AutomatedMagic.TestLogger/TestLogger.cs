namespace QA.AutomatedMagic.TestLogger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class TestLogger : ILogger
    {
        private Lazy<dynamic> _log;
        private Dictionary<TestLogger, LogLevel> _parentLoggers = new Dictionary<TestLogger, LogLevel>();
        public List<LogMessage> Messages { get; set; } = new List<LogMessage>();

        public string Name { get; private set; }
        public string TestItemType { get; private set; }

        public string FullName { get { return GetFullName(); } }

        public TestLogger(string name, string testItemType)
        {
            TestItemType = testItemType;
            Name = $"({testItemType}) {name}";

            //_log = new Lazy<NLog.Logger>(() =>
            //{
            //    var log = NLog.LogManager.GetLogger(FullName);

            //    return log;
            //});
        }

        public void AddParent(TestLogger log, LogLevel level = LogLevel.ERROR)
        {
            _parentLoggers.Add(log, level);

        }

        public string GetFullName()
        {
            if (_parentLoggers.Count != 0)
                return $"{Name}";
            return $"{_parentLoggers.First().Key.GetFullName()}${Name}";
        }

        public void TRACE(string message, Exception exception = null)
        {
            _log?.Value.Trace(exception, message);
            LOG(LogLevel.TRACE, message, exception);
        }

        public void DEBUG(string message, Exception exception = null)
        {
            _log?.Value.Debug(exception, message);
            LOG(LogLevel.DEBUG, message, exception);
        }

        public void WARN(string message, Exception exception = null)
        {
            _log?.Value.Warn(exception, message);
            LOG(LogLevel.WARN, message, exception);
        }

        public void INFO(string message, Exception exception = null)
        {
            _log?.Value.Info(exception, message);
            LOG(LogLevel.INFO, message, exception);
        }

        public void ERROR(string message, Exception exception = null)
        {
            _log?.Value.Error(exception, message);
            LOG(LogLevel.ERROR, message, exception);
        }

        protected void LOG(LogLevel level, string message, Exception exception = null)
        {
            Messages.Add(new LogMessage { Time = DateTime.UtcNow, Level = level, Message = message, Ex = exception });
            foreach (var log in _parentLoggers)
            {
                if (log.Value <= level)
                {
                    switch (level)
                    {
                        case LogLevel.TRACE:
                            log.Key.TRACE(message, exception);
                            break;
                        case LogLevel.DEBUG:
                            log.Key.DEBUG(message, exception);
                            break;
                        case LogLevel.WARN:
                            log.Key.WARN(message, exception);
                            break;
                        case LogLevel.INFO:
                            log.Key.INFO(message, exception);
                            break;
                        case LogLevel.ERROR:
                            log.Key.ERROR(message, exception);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void SpamToLog(TestLogger log)
        {
            foreach (var message in Messages)
            {
                switch (message.Level)
                {
                    case LogLevel.TRACE:
                        log.TRACE(message.Message, message.Ex);
                        break;
                    case LogLevel.DEBUG:
                        log.DEBUG(message.Message, message.Ex);
                        break;
                    case LogLevel.WARN:
                        log.WARN(message.Message, message.Ex);
                        break;
                    case LogLevel.INFO:
                        log.INFO(message.Message, message.Ex);
                        break;
                    case LogLevel.ERROR:
                        log.ERROR(message.Message, message.Ex);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
