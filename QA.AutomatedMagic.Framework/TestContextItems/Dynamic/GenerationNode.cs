namespace QA.AutomatedMagic.Framework.TestContextItems.Dynamic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;
    using ValueGeneration;

    [MetaType("Generation node")]
    public class GenerationNode : BaseDynamicNode
    {
        [MetaTypeObject("Generator implements BaseValueGenerator", IsAssignableTypesAllowed = true)]
        public BaseValueGenerator Generator { get; set; }

        public override object GetValue(TestContext context)
        {
            return Generator.GenerateValue();
        }
    }
}
