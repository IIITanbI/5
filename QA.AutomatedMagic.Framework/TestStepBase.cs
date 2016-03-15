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

        public virtual TestInfo.Step GetTestInfo()
        {
            var si = new TestInfo.Step();

            si.Name = Info.Name;
            si.Description = Info.Description;
            si.Status = ItemStatus;
            si.Messages.AddRange(Log.LogMessages);
            si.Duration = SWatch.Elapsed;

            return si;
        }
    }
}
