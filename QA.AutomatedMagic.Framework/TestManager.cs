namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    public static class TestManager
    {
        public static TestLogger Log { get; set; } = new TestLogger("TestManager");
    }
}
