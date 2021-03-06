﻿namespace QA.AutomatedMagic.Framework
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

        [MetaTypeValue("Is parent failed if this item has failed?", IsRequired = false)]
        public bool FailParentOnFail { get; set; } = false;

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

            if (ItemType == TestItemType.Test)
                TestManager.Log.INFO($"Build was successfully completed for item: {this}");
        }

        public void MergeSteps()
        {
            foreach (var parentStep in ((TestCase)Parent).TestSteps)
            {
                switch (parentStep.Order)
                {
                    case TestStepOrder.Pre:
                    case TestStepOrder.Case:
                    case TestStepOrder.Post:
                        continue;
                }

                switch (ItemType)
                {
                    case TestItemType.Project:
                    case TestItemType.Suite:
                        break;
                    case TestItemType.Test:

                        switch (parentStep.Order)
                        {
                            case TestStepOrder.PrePre:
                            case TestStepOrder.Pre:
                            case TestStepOrder.PrePost:
                            case TestStepOrder.PostPre:
                            case TestStepOrder.Post:
                            case TestStepOrder.PostPost:
                                continue;
                        }

                        break;
                }

                switch (parentStep.Order)
                {
                    case TestStepOrder.PrePre:
                    case TestStepOrder.PrePost:
                    case TestStepOrder.CasePre:
                    case TestStepOrder.CasePost:
                        {
                            var step = (TestStepBase)MetaType.CopyObject(parentStep);

                            var first = TestSteps.FirstOrDefault(s => s.Order > parentStep.Order);

                            if (first == null)
                                TestSteps.AddLast(step);
                            else
                                TestSteps.AddBefore(TestSteps.Find(first), step);

                            break;
                        }
                    case TestStepOrder.PostPre:
                        {
                            var step = (TestStepBase)MetaType.CopyObject(parentStep);

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
                            TestSteps.AddLast(step);
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        public virtual void ClearSteps()
        {
            var stepsToRemove = new List<TestStepBase>();
            foreach (var testStep in TestSteps)
            {
                if (testStep.StepMarks.Count > 0)
                {
                    if (!StepMarks.Any(sm => testStep.StepMarks.Contains(sm)))
                    {
                        stepsToRemove.Add(testStep);
                        continue;
                    }
                    else
                    {
                        switch (ItemType)
                        {
                            case TestItemType.Project:
                            case TestItemType.Suite:

                                switch (testStep.Order)
                                {
                                    case TestStepOrder.PrePre:
                                    case TestStepOrder.PrePost:
                                        testStep.Order = TestStepOrder.Pre;
                                        break;
                                    case TestStepOrder.PostPre:
                                    case TestStepOrder.PostPost:
                                        testStep.Order = TestStepOrder.Post;
                                        break;
                                    default:
                                        break;
                                }
                                continue;

                            case TestItemType.Test:
                                switch (testStep.Order)
                                {
                                    case TestStepOrder.PrePre:
                                    case TestStepOrder.PrePost:
                                        testStep.Order = TestStepOrder.Pre;
                                        break;
                                    case TestStepOrder.CasePre:
                                    case TestStepOrder.CasePost:
                                        testStep.Order = TestStepOrder.Case;
                                        break;
                                    case TestStepOrder.PostPre:
                                    case TestStepOrder.PostPost:
                                        testStep.Order = TestStepOrder.Post;
                                        break;
                                    default:
                                        break;
                                }
                                continue;
                        }
                    }
                }

                switch (ItemType)
                {
                    case TestItemType.Project:
                    case TestItemType.Suite:

                        var suite = (TestSuite)this;
                        var hasSuite = suite.Children.Any(c => c.ItemType != TestItemType.Test);

                        if (!hasSuite)
                            switch (testStep.Order)
                            {
                                case TestStepOrder.PrePre:
                                case TestStepOrder.PrePost:
                                    testStep.Order = TestStepOrder.Pre;
                                    break;
                                case TestStepOrder.PostPre:
                                case TestStepOrder.PostPost:
                                    testStep.Order = TestStepOrder.Post;
                                    break;
                                default:
                                    break;
                            }
                        else
                            switch (testStep.Order)
                            {
                                case TestStepOrder.PrePre:
                                case TestStepOrder.PrePost:
                                case TestStepOrder.CasePre:
                                case TestStepOrder.CasePost:
                                case TestStepOrder.PostPre:
                                case TestStepOrder.PostPost:
                                    stepsToRemove.Add(testStep);
                                    break;
                            }

                        break;
                    case TestItemType.Test:

                        switch (testStep.Order)
                        {
                            case TestStepOrder.PrePre:
                            case TestStepOrder.PrePost:
                                testStep.Order = TestStepOrder.Pre;
                                break;
                            case TestStepOrder.CasePre:
                            case TestStepOrder.CasePost:
                                testStep.Order = TestStepOrder.Case;
                                break;
                            case TestStepOrder.PostPre:
                            case TestStepOrder.PostPost:
                                testStep.Order = TestStepOrder.Post;
                                break;
                            default:
                                break;
                        }

                        break;
                }
            }

            foreach (var step in stepsToRemove)
            {
                TestSteps.Remove(step);
            }
        }

        public virtual void BuildSteps()
        {
            TestManager.Log.INFO($"Start building TestSteps for item: {this}");
            TestManager.Log.INFO($"TestSteps count: {TestSteps.Count}");
            foreach (var testStep in TestSteps)
            {
                testStep.Parent = this;
                testStep.Build();
            }
            TestManager.Log.INFO($"TestSteps were successfully built for item: {this}");
        }

        public override void Execute()
        {
            if (!IsEnabled) return;
            SWatch.Start();
            Log.INFO($"Start executing {this}");
            Parent?.Log.INFO($"Start executing {this}");
            ItemStatus = TestItemStatus.Unknown;

            for (; _tryNumber < TryCount; _tryNumber++)
            {
                ItemStatus = TestItemStatus.Unknown;

                Log.DEBUG($"Start try #{_tryNumber} of {TryCount}");

                ExecuteSteps(TestStepOrder.Pre);
                ExecuteSteps(TestStepOrder.Case);
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
                ExecuteSteps(TestStepOrder.Case);
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

        public override void Skip()
        {
            base.Skip();

            foreach (var testStep in TestSteps)
            {
                testStep.Skip();
            }
        }

        public void ExecuteSteps(TestStepOrder order)
        {
            var stepsToExecute = TestSteps.Where(s => s.Order == order && s.IsEnabled).ToList();
            if (stepsToExecute.Count > 0)
            {
                Log.DEBUG($"Number of {order} steps: {stepsToExecute.Count}");
                Log.DEBUG($"Start executing {order} steps");

                if (order == TestStepOrder.Post)
                {
                    var isFailed = ItemStatus == TestItemStatus.Failed;

                    foreach (var step in stepsToExecute)
                    {
                        step.SWatch.Start();
                        step.Execute();
                        step.SWatch.Stop();

                        if (step.ItemStatus == TestItemStatus.Failed)
                            isFailed = true;
                    }

                    if (isFailed)
                        ItemStatus = TestItemStatus.Failed;
                    else
                        ItemStatus = TestItemStatus.Passed;
                }
                else
                {
                    foreach (var step in stepsToExecute)
                    {
                        if (ItemStatus == TestItemStatus.Failed)
                        {
                            step.ItemStatus = TestItemStatus.Skipped;
                            continue;
                        }

                        step.SWatch.Start();
                        step.Execute();
                        step.SWatch.Stop();

                        ItemStatus = step.ItemStatus;
                    }
                }
                Log.DEBUG($"Executing {order} steps was completed");
            }
            else
            {
                if (ItemStatus == TestItemStatus.Failed)
                    return;

                ItemStatus = TestItemStatus.Passed;
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
            ti.Duration = SWatch.Elapsed;

            foreach (var step in TestSteps)
            {
                ti.Steps.Add(step.GetTestInfo());
            }

            return ti;
        }
    }
}
