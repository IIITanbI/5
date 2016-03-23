namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class FrameworkGeneratingException : FrameworkBuildingException
    {
        public FrameworkGeneratingException(TestItem item, string message, params string[] infos)
            : base(item, message, infos)
        { }

        public FrameworkGeneratingException(TestItem item, string message, Exception innerException, params string[] infos)
            : base(item, message, innerException, infos)
        { }
    }
}
