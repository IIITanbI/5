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
        public LinkedList<TestStep> TestSteps { get; set; } = new LinkedList<TestStep>();

        public override TestItemType ItemType { get; protected set; }

        public TestCase()
        {
            ItemType = TestItemType.Test;
        }

        public override void Build()
        {
            Log.INFO($"Start building item: {this}");
            base.Build();


            Log.INFO($"Sort steps for item: {this}");
            var steps = TestSteps.ToList();
            steps.Sort((s1, s2) => s1.Order - s2.Order);
            TestSteps.Clear();
            steps.ForEach(s => TestSteps.AddLast(s));

            if (Parent != null)
            {
                Log.INFO($"Merge steps with parent steps for item: {this}");
                MergeSteps();
            }

            Log.INFO($"Start building TestSteps for item: {this}");
            Log.INFO($"TestSteps count: {TestSteps.Count}");
            foreach (var testStep in TestSteps)
            {
                testStep.Parent = this;
                testStep.Build();
            }
            Log.INFO($"TestSteps were successfully built for item: {this}");

            if (ItemType == TestItemType.Test)
                Log.INFO($"Build was successfully completed for item: {this}");
        }

        public void MergeSteps()
        {
            foreach (var parentStep in ((TestCase)Parent).TestSteps)
            {
                switch (parentStep.Order)
                {
                    case TestStepOrder.PrePre:
                        {
                            var step = (TestStep)MetaType.CopyObject(parentStep);
                            TestSteps.AddFirst(step);
                            break;
                        }
                    case TestStepOrder.PrePost:
                    case TestStepOrder.CasePost:
                        {
                            var step = (TestStep)MetaType.CopyObject(parentStep);
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
                            var step = (TestStep)MetaType.CopyObject(parentStep);
                            var first = TestSteps.FirstOrDefault(s => s.Order >= parentStep.Order);

                            if (first == null)
                                TestSteps.AddLast(step);
                            else
                                TestSteps.AddBefore(TestSteps.Find(first), step);

                            break;
                        }
                    case TestStepOrder.PostPost:
                        {
                            var step = (TestStep)MetaType.CopyObject(parentStep);
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
            throw new NotImplementedException();
        }
    }
}
