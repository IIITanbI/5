namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;
    using TestInfo;

    [MetaType("Test suite")]
    public class TestSuite : TestCase
    {
        [MetaTypeCollection("List of child TestCases and TestSuites", IsAssignableTypesAllowed = true)]
        [MetaLocation("Tests", "Suites", "TestCases", "TestSuites")]
        public List<TestCase> Children { get; set; }

        [MetaTypeValue("Is parallelism enabled?", IsRequired = false)]
        public bool IsParallelismEnabled { get; set; } = false;

        [MetaTypeValue("Number of threads for parallel execution. 0 - Auto", IsRequired = false)]
        public int ThreadNumber { get; set; } = 0;

        public TestSuite()
        {
            ItemType = TestItemType.Suite;
        }

        public override void MetaInit()
        {
            base.MetaInit();
            StepMarks.Remove("TestCase");
            StepMarks.Add("TestSuite");
        }

        public override void Build()
        {
            base.Build();

            TestManager.Log.INFO($"Start building children for item: {this}");
            TestManager.Log.INFO($"Children count: {Children.Count}");

            foreach (var child in Children)
            {
                child.Parent = this;
                child.Build();
            }

            TestManager.Log.INFO($"Children were successfully built for item: {this}");
            TestManager.Log.INFO($"Build was successfully completed for item: {this}");
        }

        public override void ClearSteps()
        {
            base.ClearSteps();

            foreach (var child in Children)
            {
                child.ClearSteps();
            }
        }

        public override void BuildSteps()
        {
            base.BuildSteps();

            TestManager.Log.INFO($"Start building steps for children in: {this}");
            foreach (var child in Children)
            {
                child.BuildSteps();
            }
            TestManager.Log.INFO($"Building steps for children in: {this} successfully completed");
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

                if (ItemStatus == TestItemStatus.Passed)
                {
                    Log.DEBUG("Start executing children");
                    Log.DEBUG($"Children count: {Children.Count}");

                    if (!IsParallelismEnabled)
                    {
                        Log.DEBUG($"Execute children in sequence mode");
                        bool skip = false;
                        foreach (var child in Children)
                        {
                            if (skip)
                            {
                                child.Skip();
                                continue;
                            }

                            ExecuteChild(child);

                            if (child.ItemStatus == TestItemStatus.Failed)
                                if (child.FailParentOnFail)
                                {
                                    skip = true;
                                }
                        }
                    }
                    else
                    {
                        Log.DEBUG($"Execute children in parallel mode");
                        Log.DEBUG("Thread number: " + (ThreadNumber == 0 ? "Auto" : ThreadNumber.ToString()));

                        var parallelOptions = new ParallelOptions();
                        if (ThreadNumber != 0)
                            parallelOptions.MaxDegreeOfParallelism = ThreadNumber;

                        Parallel.ForEach(Children, parallelOptions, child => ExecuteChild(child));
                    }
                    Log.DEBUG("Children execution was completed");

                    if (Children.Any(c => c.ItemStatus == TestItemStatus.Failed))
                        ItemStatus = TestItemStatus.Failed;
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

                if (ItemStatus == TestItemStatus.Passed)
                {
                    Log.DEBUG("Start executing children");
                    Log.DEBUG($"Children count: {Children.Count}");

                    if (!IsParallelismEnabled)
                    {
                        Log.DEBUG($"Execute children in sequence mode");
                        bool skip = false;
                        foreach (var child in Children)
                        {
                            if (skip)
                            {
                                child.Skip();
                                continue;
                            }

                            ExecuteChild(child);

                            if (child.ItemStatus == TestItemStatus.Failed)
                                if (child.FailParentOnFail)
                                {
                                    skip = true;
                                }
                        }
                    }
                    else
                    {
                        Log.DEBUG($"Execute children in parallel mode");
                        Log.DEBUG("Thread number: " + (ThreadNumber == 0 ? "Auto" : ThreadNumber.ToString()));

                        var parallelOptions = new ParallelOptions();
                        if (ThreadNumber != 0)
                            parallelOptions.MaxDegreeOfParallelism = ThreadNumber;

                        Parallel.ForEach(Children, parallelOptions, child => ExecuteChild(child));
                    }
                    Log.DEBUG("Children execution was completed");

                    if (Children.Any(c => c.ItemStatus == TestItemStatus.Failed))
                        ItemStatus = TestItemStatus.Failed;
                }
                else
                {
                    Children.ForEach(c => c.ItemStatus = TestItemStatus.Skipped);
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


        private void ExecuteChild(TestCase child)
        {
            child.SWatch.Start();
            child.Execute();
            child.SWatch.Stop();
        }

        public override void Skip()
        {
            base.Skip();

            foreach (var child in Children)
            {
                child.Skip();
            }
        }

        public override TestInfo.TestItem GetTestInfo()
        {
            var ti = base.GetTestInfo();

            foreach (var child in Children)
            {
                ti.Childs.Add(child.GetTestInfo());
            }

            return ti;
        }
    }
}
