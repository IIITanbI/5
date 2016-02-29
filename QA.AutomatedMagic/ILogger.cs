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
        void DEBUG(string message, Exception exception = null);
        void WARN(string message, Exception exception = null);
        void INFO(string message, Exception exception = null);
        void ERROR(string message, Exception exception = null);
    }
}
