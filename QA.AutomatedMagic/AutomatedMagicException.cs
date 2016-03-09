namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AutomatedMagicException : Exception
    {
        public AutomatedMagicException(string message, Exception innerException = null)
            : base(message, innerException)
        {

        }
    }
}
