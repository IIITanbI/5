namespace QA.AutomatedMagic.TestInfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Logged file")]
    public class LogFile : LogItem
    {
        [MetaTypeValue("Type of attached data")]
        public LoggedFileType FileType { get; set; }

        [MetaTypeValue("Path to file with stored data")]
        public string FilePath { get; set; }
    }
}
