namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ILogger
    {
        void TRACE(string message, Exception exception = null);
        void TRACE(string message, AttachedItemType itemType, string itemPath);
        void DEBUG(string message, Exception exception = null);
        void DEBUG(string message, AttachedItemType itemType, string itemPath);
        void WARN(string message, Exception exception = null);
        void WARN(string message, AttachedItemType itemType, string itemPath);
        void INFO(string message, Exception exception = null);
        void INFO(string message, AttachedItemType itemType, string itemPath);
        void ERROR(string message, Exception exception = null);
        void ERROR(string message, AttachedItemType itemType, string itemPath);
        void LOG(LogLevel level, string message, Exception exception = null);
        void LOG(LogLevel level, string message, AttachedItemType itemType, string itemPath);
        void SetParent(ILogger logger);
        void AddLogger(ILogger logger, LogLevel level);
        string GetFullName();
        ILogger PARENT { get; }
        string Name { get; }
    }
}
