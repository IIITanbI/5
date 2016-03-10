namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Test project")]
    public class TestProject : TestSuite
    {
        public TestProject()
        {
            ItemType = TestItemType.Project;
        }

        public override void Build()
        {
            base.Build();
        }
    }
}
