namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class FrameworkBuildingException : FrameworkException
    {
        public FrameworkBuildingException(TestItem item, string message, params string[] infos)
        {

        }

        public FrameworkBuildingException(TestItem item, string message, Exception innerException, params string[] infos)
        {

        }

        public FrameworkBuildingException(string message, FrameworkBuildingException innerFcbEx, params string[] infos)
        {

        }
    }
}
