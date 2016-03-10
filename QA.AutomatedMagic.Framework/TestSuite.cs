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

        public TestSuite()
        {
            ItemType = TestItemType.Suite;
        }

        public override void Build()
        {
            base.Build();

            Log.INFO($"Start building children for item: {this}");
            Log.INFO($"Children count: {Children.Count}");

            foreach (var child in Children)
            {
                child.Parent = this;
                child.Build();
            }

            Log.INFO($"Children were successfully built for item: {this}");
            Log.INFO($"Build was successfully completed for item: {this}");
        }

        public override void Execute()
        {
            base.Execute();
        }
    }
}
