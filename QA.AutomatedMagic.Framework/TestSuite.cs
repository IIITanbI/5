namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Test suite")]
    public class TestSuite : TestCase
    {
        [MetaTypeCollection("List of child TestCases and TestSuites", "test", "testCase", "suite", "testSuite", IsAssignableTypesAllowed = true)]
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
                    ItemStatus = TestItemStatus.Unknown;
                    ExecuteSteps(TestStepOrder.Post);
                    ItemStatus = TestItemStatus.Failed;
                    Log.WARN($"Try #{_tryNumber} of {TryCount} completed with error. Try again");
                    continue;
                }

                Log.DEBUG("Start executing children");
                Log.DEBUG($"Children count: {Children.Count}");

                if (!IsParallelismEnabled)
                {
                    Log.DEBUG($"Execute children in sequence mode");
                    foreach (var child in Children)
                    {
                        child.Execute();
                    }
                }
                else
                {
                    Log.DEBUG($"Execute children in parallel mode");
                    Log.DEBUG("Thread number: " + (ThreadNumber == 0 ? "Auto" : ThreadNumber.ToString()));

                    var parallelOptions = new ParallelOptions();
                    if (ThreadNumber != 0)
                        parallelOptions.MaxDegreeOfParallelism = ThreadNumber;

                    Parallel.ForEach(Children, parallelOptions, child => child.Execute());
                }
                Log.DEBUG("Children execution was completed");

                if (Children.Any(c => c.ItemStatus == TestItemStatus.Failed))
                    ItemStatus = TestItemStatus.Failed;

                ItemStatus = TestItemStatus.Unknown;
                ExecuteSteps(TestStepOrder.Post);
                ItemStatus = TestItemStatus.Failed;
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
                    ItemStatus = TestItemStatus.Unknown;
                    ExecuteSteps(TestStepOrder.Post);
                    ItemStatus = TestItemStatus.Failed;
                    Log.ERROR($"Try #{_tryNumber} of {TryCount} completed with error.");
                    Log.ERROR($"Execution of item: {this} completed with status: {ItemStatus}");
                    Parent?.Log.ERROR($"Execution of item: {this} completed with status: {ItemStatus}");
                    return;
                }

                Log.DEBUG("Start executing children");
                Log.DEBUG($"Children count: {Children.Count}");

                if (!IsParallelismEnabled)
                {
                    Log.DEBUG($"Execute children in sequence mode");
                    foreach (var child in Children)
                    {
                        child.Execute();
                    }
                }
                else
                {
                    Log.DEBUG($"Execute children in parallel mode");
                    Log.DEBUG("Thread number: " + (ThreadNumber == 0 ? "Auto" : ThreadNumber.ToString()));

                    var parallelOptions = new ParallelOptions();
                    if (ThreadNumber != 0)
                        parallelOptions.MaxDegreeOfParallelism = ThreadNumber;

                    Parallel.ForEach(Children, parallelOptions, child => child.Execute());
                }
                Log.DEBUG("Children execution was completed");

                if (Children.Any(c => c.ItemStatus == TestItemStatus.Failed))
                    ItemStatus = TestItemStatus.Failed;

                ItemStatus = TestItemStatus.Unknown;
                ExecuteSteps(TestStepOrder.Post);
                ItemStatus = TestItemStatus.Failed;
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
    }
}
