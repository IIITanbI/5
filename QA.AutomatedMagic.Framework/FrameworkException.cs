namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class FrameworkException : Exception
    {
        public FrameworkException(string message, Exception innerException = null)
            : base(message, innerException)
        {

        }

    }
}
