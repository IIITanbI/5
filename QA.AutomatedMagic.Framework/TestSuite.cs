namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using MetaMagic;

    [MetaType("Test suite config")]
    [MetaLocation("Suite")]
    public class TestSuite : TestItem
    {
        [MetaTypeCollection("Child test items", IsAssignableTypesAllowed = true)]
        [MetaLocation("suites", "tests", "testSuites", "cases", "testCases")]
        public List<TestItem> TestItems { get; set; } = new List<TestItem>();

        [MetaTypeValue("Is parallel execution allowed for children", IsRequired = false)]
        [MetaLocation("parallel", "isParallel", "parallelAllowed")]
        public bool IsParallelExecutionAllowed { get; set; } = false;

        [MetaTypeValue("Level of parallelism", IsRequired = false)]
        [MetaLocation("levelOfparallelism")]
        public int ParallelismLevel { get; set; } = -1;

        public TestSuite()
        {
            ItemType = TestItemType.Suite;
        }

        public override List<TestItem> Build()
        {
            var builtSuites = base.Build();

            foreach (var builtSuite in builtSuites)
            {
                var builtChildren = new List<TestItem>();

                foreach (var testItem in ((TestSuite)builtSuite).TestItems)
                {
                    builtChildren.AddRange(testItem.Build());
                }

                ((TestSuite)builtSuite).TestItems.Clear();
                ((TestSuite)builtSuite).TestItems = builtChildren;
            }

            return builtSuites;
        }

        public override void SetParent(TestSuite parent)
        {
            base.SetParent(parent);

            foreach (var testItem in TestItems)
            {
                testItem.SetParent(this);
            }
        }

        public override TestItem GetState()
        {
            var testSuite = (TestSuite)base.GetState();
            foreach (var child in TestItems)
            {
                testSuite.TestItems.Add(child.GetState());
            }
            return testSuite;
        }

        public override void ExecuteStageCase()
        {
            base.ExecuteStageCase();

            if (!IsParallelExecutionAllowed)
            {
                foreach (var testItem in TestItems)
                {
                    testItem.Execute();
                }
            }
            else
            {
                if (ParallelismLevel != -1 && ParallelismLevel > 1)
                    Parallel.ForEach(TestItems, new ParallelOptions { MaxDegreeOfParallelism = ParallelismLevel }, ti => ti.Execute());
                else
                    Parallel.ForEach(TestItems, ti => ti.Execute());
            }

            if (TestItems.Any(ti => ti.Status == TestItemStatus.Failed))
                Status = TestItemStatus.Failed;
        }

        public override void MarkAsFailedOrSkipped(TestItemStatus status = TestItemStatus.Failed)
        {
            base.MarkAsFailedOrSkipped(status);

            foreach (var testItem in TestItems)
            {
                testItem.MarkAsFailedOrSkipped(TestItemStatus.Skipped);
            }
        }

        public override TestInfo.TestItem GetReportItem()
        {
            var reportItem = base.GetReportItem();

            foreach (var testItem in TestItems)
            {
                reportItem.Childs.Add(testItem.GetReportItem());
            }

            return reportItem;
        }
    }
}
