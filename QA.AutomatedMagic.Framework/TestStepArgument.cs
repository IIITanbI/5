namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Test step argument")]
    public class TestStepArgument
    {
        [MetaTypeValue("Test step argument type", IsRequired = false)]
        public TestStepArgumentType Type { get; set; } = TestStepArgumentType.Context;

        [MetaTypeValue("Test step argument value")]
        public string Value { get; set; }
    }
}
