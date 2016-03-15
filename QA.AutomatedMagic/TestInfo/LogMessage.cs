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
        [MetaTypeValue("Exception string", IsRequired = false)]
        public string ExceptionString { get; set; } = null;

        private Exception _ex;
        public Exception Ex
        {
            get
            {
                return _ex;
            }
            set
            {
                _ex = value;
                ExceptionString = value?.ToString();
            }
        }
    }
}
