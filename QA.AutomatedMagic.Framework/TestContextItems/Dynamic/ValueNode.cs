namespace QA.AutomatedMagic.Framework.TestContextItems.Dynamic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Value node")]
    public class ValueNode : BaseDynamicNode
    {
        [MetaTypeValue("Node value")]
        [MetaLocation(true)]
        public string Value { get; set; }

        public override object GetValue(TestContext context)
        {
            return Value;
        }
    }
}
