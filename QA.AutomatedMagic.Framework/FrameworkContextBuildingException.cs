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
        {
            
        }

        public FrameworkContextBuildingException(TestItem item, string message, Exception innerException, params string[] infos)
        {

        }

        public FrameworkContextBuildingException(string message, FrameworkContextBuildingException innerFcbEx, params string[] infos)
        {

        }
    }
}
