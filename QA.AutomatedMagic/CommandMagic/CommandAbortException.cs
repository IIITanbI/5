namespace QA.AutomatedMagic.CommandMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class CommandAbortException : Exception
    {
        public CommandAbortException(string reason, Exception innerException = null)
            : base(reason, innerException)
        {

        }
    }
}
