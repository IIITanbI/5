namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Group of Test steps")]
    public class TestStepGroup : TestStep
    {
        [MetaTypeCollection("List of test steps", IsAssignableTypesAllowed = true)]
        public List<TestStep> TestSteps { get; set; }
    }
}
