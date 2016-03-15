namespace QA.AutomatedMagic.Framework.TestContextItems.Dynamic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Context value node")]
    public class ContextValueNode : BaseDynamicNode
    {
        [MetaTypeValue("Value to resolve from context")]
        public string Value { get; set; }

        public override object GetValue(TestContext context)
        {
            return context.ResolveValue(Value);
        }
    }
}
