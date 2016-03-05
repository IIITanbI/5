namespace QA.AutomatedMagic.TestInfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Logged message")]
    public class LogMessage : LogItem
    {
        public Exception Ex { get; set; }
    }
}
