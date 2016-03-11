namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Test case")]
    public class TestCase : TestItem
    {
        [MetaTypeCollection("List of test steps", IsAssignableTypesAllowed = true, IsRequired = false)]
        [MetaLocation("steps")]
        public LinkedList<TestStepBase> TestSteps { get; set; } = new LinkedList<TestStepBase>();

        [MetaTypeCollection("List of tags for test item", "tag", IsRequired = false)]
        public List<string> Tags { get; set; } = new List<string> { "All" };

        public override TestItemType ItemType { get; protected set; }

        public TestCase()
        {
            ItemType = TestItemType.Test;
        }

        public override void Build()
        {
            TestManager.Log.INFO($"Start building item: {this}");
            base.Build();


            TestManager.Log.INFO($"Sort steps for item: {this}");
            var steps = TestSteps.ToList();
            steps.Sort((s1, s2) => s1.Order - s2.Order);
            TestSteps.Clear();
            steps.ForEach(s => TestSteps.AddLast(s));

            if (Parent != null)
            {
                TestManager.Log.INFO($"Merge steps with parent steps for item: {this}");
                MergeSteps();
            }

            TestManager.Log.INFO($"Start building TestSteps for item: {this}");
            TestManager.Log.INFO($"TestSteps count: {TestSteps.Count}");
            foreach (var testStep in TestSteps)
            {
                testStep.Parent = this;
                testStep.Build();
            }
            TestManager.Log.INFO($"TestSteps were successfully built for item: {this}");

            if (ItemType == TestItemType.Test)
                TestManager.Log.INFO($"Build was successfully completed for item: {this}");
        }

        public void MergeSteps()
        {
            foreach (var parentStep in ((TestCase)Parent).TestSteps)
            {
                switch (parentStep.Order)
                {
                    case TestStepOrder.PrePre:
                        {
                            var step = (TestStepBase)MetaType.CopyObject(parentStep);
                            step.Order = TestStepOrder.Pre;
                            TestSteps.AddFirst(step);
                            break;
                        }
                    case TestStepOrder.PrePost:
                    case TestStepOrder.CasePost:
                        {
                            var step = (TestStepBase)MetaType.CopyObject(parentStep);
                            if (step.Order == TestStepOrder.PrePost)
                                step.Order = TestStepOrder.Pre;
                            if (step.Order == TestStepOrder.CasePost && ItemType == TestItemType.Test)
                                step.Order = TestStepOrder.Case;

                            var first = TestSteps.FirstOrDefault(s => s.Order > parentStep.Order);

                            if (first == null)
                                TestSteps.AddLast(step);
                            else
                                TestSteps.AddBefore(TestSteps.Find(first), step);

                            break;
                        }
                    case TestStepOrder.CasePre:
                    case TestStepOrder.PostPre:
                        {
                            var step = (TestStepBase)MetaType.CopyObject(parentStep);
                            if (step.Order == TestStepOrder.PostPre)
                                step.Order = TestStepOrder.Post;
                            if (step.Order == TestStepOrder.CasePre && ItemType == TestItemType.Test)
                                step.Order = TestStepOrder.Case;

                            var first = TestSteps.FirstOrDefault(s => s.Order >= parentStep.Order);

                            if (first == null)
                                TestSteps.AddLast(step);
                            else
                                TestSteps.AddBefore(TestSteps.Find(first), step);

                            break;
                        }
                    case TestStepOrder.PostPost:
                        {
                            var step = (TestStepBase)MetaType.CopyObject(parentStep);
                            step.Order = TestStepOrder.Post;
                            TestSteps.AddLast(step);
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        public override void Execute()
        {
            Log.INFO($"Start executing {this}");
            Parent?.Log.INFO($"Start executing {this}");
            ItemStatus = TestItemStatus.Unknown;

            for (; _tryNumber < TryCount; _tryNumber++)
            {
                ItemStatus = TestItemStatus.Unknown;

                Log.DEBUG($"Start try #{_tryNumber} of {TryCount}");

                ExecuteSteps(TestStepOrder.Pre);
                if (ItemStatus == TestItemStatus.Failed)
                {
                    ExecuteSteps(TestStepOrder.Case);
                    ItemStatus = TestItemStatus.Unknown;
                    ExecuteSteps(TestStepOrder.Post);
                    ItemStatus = TestItemStatus.Failed;
                    Log.WARN($"Try #{_tryNumber} of {TryCount} completed with error. Try again");
                    continue;
                }

                ExecuteSteps(TestStepOrder.Case);
                if (ItemStatus == TestItemStatus.Failed)
                {
                    ItemStatus = TestItemStatus.Unknown;
                    ExecuteSteps(TestStepOrder.Post);
                    ItemStatus = TestItemStatus.Failed;
                    Log.WARN($"Try #{_tryNumber} of {TryCount} completed with error. Try again");
                    continue;
                }

                ExecuteSteps(TestStepOrder.Post);
                if (ItemStatus == TestItemStatus.Failed)
                {
                    Log.WARN($"Try #{_tryNumber} of {TryCount} completed with error. Try again");
                    continue;
                }

                if (ItemStatus == TestItemStatus.Passed)
                    break;
            }


            if (ItemStatus != TestItemStatus.Passed)
            {
                ItemStatus = TestItemStatus.Unknown;

                Log.DEBUG($"Start try #{_tryNumber} of {TryCount}");

                ExecuteSteps(TestStepOrder.Pre);
                if (ItemStatus == TestItemStatus.Failed)
                {
                    ExecuteSteps(TestStepOrder.Case);
                    ItemStatus = TestItemStatus.Unknown;
                    ExecuteSteps(TestStepOrder.Post);
                    ItemStatus = TestItemStatus.Failed;

                    Log.ERROR($"Try #{_tryNumber} of {TryCount} completed with error.");
                    Log.ERROR($"Execution of item: {this} completed with status: {ItemStatus}");
                    Parent?.Log.ERROR($"Execution of item: {this} completed with status: {ItemStatus}");
                    return;
                }

                ExecuteSteps(TestStepOrder.Case);
                if (ItemStatus == TestItemStatus.Failed)
                {
                    ItemStatus = TestItemStatus.Unknown;
                    ExecuteSteps(TestStepOrder.Post);
                    ItemStatus = TestItemStatus.Failed;

                    Log.ERROR($"Try #{_tryNumber} of {TryCount} completed with error.");
                    Log.ERROR($"Execution of item: {this} completed with status: {ItemStatus}");
                    Parent?.Log.ERROR($"Execution of item: {this} completed with status: {ItemStatus}");
                    return;
                }

                ExecuteSteps(TestStepOrder.Post);

                if (ItemStatus == TestItemStatus.Failed)
                {
                    Log.ERROR($"Try #{_tryNumber} of {TryCount} completed with error.");
                    Log.ERROR($"Execution of item: {this} completed with status: {ItemStatus}");
                    Parent?.Log.ERROR($"Execution of item: {this} completed with status: {ItemStatus}");
                    return;
                }
            }

            Log.DEBUG($"Try #{_tryNumber} of {TryCount} was successfully completed");
            Log.INFO($"Execution of item: {this} completed with status: {ItemStatus}");
            Parent?.Log.INFO($"Execution of item: {this} completed with status: {ItemStatus}");
        }

        public void ExecuteSteps(TestStepOrder order)
        {
            var stepsToExecute = TestSteps.Where(s => s.Order == order && s.IsEnabled).ToList();
            if (stepsToExecute.Count > 0)
            {
                Log.DEBUG($"Number of {order} steps: {stepsToExecute.Count}");
                Log.DEBUG($"Start executing {order} steps");

                foreach (var step in stepsToExecute)
                {
                    if (ItemStatus == TestItemStatus.Failed)
                    {
                        step.ItemStatus = TestItemStatus.Skipped;
                        continue;
                    }

                    step.Execute();
                    ItemStatus = step.ItemStatus;
                }
                Log.DEBUG($"Executing {order} steps was completed");
            }
        }

        public virtual TestInfo.TestItem GetTestInfo()
        {
            var ti = new TestInfo.TestItem();

            ti.Name = Info.Name;
            ti.Description = Info.Description;
            ti.Status = ItemStatus;
            ti.LogMessages.AddRange(Log.LogMessages);
            ti.Type = ItemType;

            foreach (var step in TestSteps)
            {
                ti.Steps.Add(step.GetTestInfo());
            }

            return ti;
        }
    }
}
