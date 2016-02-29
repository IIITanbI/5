namespace QA.AutomatedMagic.TestInfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("LogMessage config")]
    public class LogMessage : BaseMetaObject
    {
        [MetaTypeValue("Log level")]
        public LogLevel Level { get; set; }

        [MetaTypeValue("Log message")]
        public string Message { get; set; }

        public Exception Exception { get; set; }

        [MetaTypeValue("Log exception")]
        public DateTime DataStemp { get; set; }
    }
}
