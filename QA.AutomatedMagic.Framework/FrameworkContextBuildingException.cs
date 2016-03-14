namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FrameworkContextBuildingException : FrameworkException
    {

        public FrameworkContextBuildingException(TestItem item, string message, params string[] infos)
            : base(item, message, infos)
        { }

        public FrameworkContextBuildingException(TestItem item, string message, Exception innerException, params string[] infos)
            : base(item, message, innerException, infos)
        { }
    }
}
