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
        [MetaTypeValue("Log exception")]
        public DateTime DataStemp { get; set; }

        [MetaTypeValue("Log level")]
        public LogLevel Level { get; set; }

        [MetaTypeValue("Log message")]
        public string Message { get; set; }

        [MetaTypeValue("Attached item type", IsRequired = false)]
        public AttachedItemType ItemType { get; set; } = AttachedItemType.NONE;

        [MetaTypeValue("Path to attached item", IsRequired = false)]
        public string ItemPath { get; set; } = null;

        public Exception Exception { get; set; }
    }
}
