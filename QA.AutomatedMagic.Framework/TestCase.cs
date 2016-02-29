namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;
    
    [MetaType("TestProject config")]
    [MetaLocation("test")]
    public class TestCase : TestItem
    {
        public TestCase()
        {
            ItemType = TestItemType.Test;
        }
    }
}
