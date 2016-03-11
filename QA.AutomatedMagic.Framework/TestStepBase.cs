namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Base class for test steps")]
    public abstract class TestStepBase : TestItem
    {
        [MetaTypeValue("Order of test step", IsRequired = false)]
        public TestStepOrder Order { get; set; } = TestStepOrder.Case;

        [MetaTypeValue("Is test step skipped on fail", IsRequired = false)]
        public bool IsSkippedOnFail { get; set; } = false;

        public override TestItemType ItemType { get; protected set; }

        public TestStepBase()
        {
            ItemType = TestItemType.Step;
        }
        public void AddStepResult(string stepName, object result)
        {
            TestItem curItem = this;
            while (curItem.ItemType == TestItemType.Step)
                curItem = curItem.Parent;

            curItem.Context.AddStepResult(stepName, result);
        }
    }
}
