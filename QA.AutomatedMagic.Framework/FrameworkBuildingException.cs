﻿namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class FrameworkBuildingException : FrameworkException
    {
        public FrameworkBuildingException(TestItem item, string message, params string[] infos)
            : base(item, message, infos)
        { }

        public FrameworkBuildingException(TestItem item, string message, Exception innerException, params string[] infos)
            : base(item, message, innerException, infos)
        { }

        public FrameworkBuildingException(string message, Exception innerException, params string[] infos)
            : base(message, innerException, infos)
        { }
    }
}
