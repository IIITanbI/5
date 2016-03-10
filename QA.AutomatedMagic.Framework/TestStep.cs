namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Test step")]
    public class TestStep : TestItem
    {
        [MetaTypeValue("Order of test step")]
        public TestStepOrder Order { get; set; } = TestStepOrder.Case;

        [MetaTypeValue("Is test step skipped on fail")]
        public bool IsSkippedOnFail { get; set; } = false;

        [MetaTypeValue("Manager")]
        public string Manager { get; set; }

        [MetaTypeValue("Command")]
        public string Command { get; set; }

        [MetaTypeCollection("List of argument for test step", "argument", "arg", IsRequired = false)]
        public List<TestStepArgument> Parameters { get; set; } = new List<TestStepArgument>();

        public override TestItemType ItemType { get; protected set; }

        public TestStep()
        {
            ItemType = TestItemType.Step;
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }

        public override void Build()
        {
            Log.INFO($"Start building item: {this}");
            base.Build();

            if (ItemType == TestItemType.Step)
                Log.INFO($"Build was successfully completed for item: {this}");
        }
    }
}
