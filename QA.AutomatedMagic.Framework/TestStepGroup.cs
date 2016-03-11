namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Group of Test steps")]
    public class TestStepGroup : TestStepBase
    {
        [MetaTypeCollection("List of test steps", IsAssignableTypesAllowed = true)]
        public List<TestStep> TestSteps { get; set; }

        public override void Execute()
        {
            ItemStatus = TestItemStatus.Unknown;
            Log.INFO($"Start executing {this}");
            Parent.Log.INFO($"Start executing {this}");

            #region Execute Child TestSteps with tries
            for (; _tryNumber < TryCount; _tryNumber++)
            {
                ItemStatus = TestItemStatus.Unknown;

                Log.DEBUG($"Start try #{_tryNumber} of {TryCount}");
                foreach (var step in TestSteps)
                {
                    if (ItemStatus == TestItemStatus.Failed)
                    {
                        step.ItemStatus = TestItemStatus.Skipped;
                        continue;
                    }

                    step.Execute();

                    if (step.ItemStatus == TestItemStatus.Failed)
                    {
                        ItemStatus = TestItemStatus.Failed;
                    }
                }

                if (ItemStatus == TestItemStatus.Failed)
                {
                    Log.WARN($"Try #{_tryNumber} of {TryCount} completed with error. Try again");
                }

                if (ItemStatus == TestItemStatus.Passed)
                    break;
            }

            if (ItemStatus != TestItemStatus.Passed)
            {
                ItemStatus = TestItemStatus.Unknown;

                Log.DEBUG($"Start try #{_tryNumber} of {TryCount}");
                foreach (var step in TestSteps)
                {
                    if (ItemStatus == TestItemStatus.Failed)
                    {
                        step.ItemStatus = TestItemStatus.Skipped;
                        continue;
                    }

                    step.Execute();

                    if (step.ItemStatus == TestItemStatus.Failed)
                    {
                        ItemStatus = TestItemStatus.Failed;
                    }
                }

                if (ItemStatus == TestItemStatus.Failed)
                {
                    if (IsSkippedOnFail)
                    {
                        ItemStatus = TestItemStatus.Skipped;

                        Log.WARN($"Try #{_tryNumber} of {TryCount} completed with error");
                        Log.WARN($"Execution of {this} completed with status: {ItemStatus}");
                        Parent.Log.WARN($"Execution of {this} completed with status: {ItemStatus}");
                        return;
                    }

                    Log.ERROR($"Try #{_tryNumber} of {TryCount} completed with error");
                    Log.ERROR($"Execution of {this} completed with status: {ItemStatus}");
                    Parent.Log.ERROR($"Execution of {this} completed with status: {ItemStatus}");
                    return;
                }
            }
            #endregion

            ItemStatus = TestItemStatus.Passed;

            Log.DEBUG($"Try #{_tryNumber} of {TryCount} was successfully completed");
            Log.INFO($"Execution of {this} completed with status: {ItemStatus}");
            Parent.Log.INFO($"Execution of {this} completed with status: {ItemStatus}");
        }

        public override void Build()
        {
            if (!(Order == TestStepOrder.Pre || Order == TestStepOrder.Case || Order == TestStepOrder.Post)) return;
            TestManager.Log.INFO($"Start building item: {this}");
            base.Build();


            TestManager.Log.INFO($"Start building child TestSteps for item: {this}");
            TestManager.Log.INFO($"Child TestSteps count: {TestSteps.Count}");
            foreach (var testStep in TestSteps)
            {
                testStep.Parent = this;
                testStep.Info.Name = $"{Info.Name}.{testStep.Info.Name}";
                testStep.Build();
            }
            TestManager.Log.INFO($"Child TestSteps were successfully built for item: {this}");
            TestManager.Log.INFO($"Build was successfully completed for item: {this}");
        }
    }
}
