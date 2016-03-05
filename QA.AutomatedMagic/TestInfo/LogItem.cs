namespace QA.AutomatedMagic.TestInfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Base type for LogMessage")]
    public abstract class LogItem : BaseMetaObject
    {
        [MetaTypeValue("Log exception")]
        public DateTime DataStemp { get; set; }

        [MetaTypeValue("Log level")]
        public LogLevel Level { get; set; }

        [MetaTypeValue("Log message")]
        public string Message { get; set; }
    }
}
