namespace QA.AutomatedMagic.Framework.TestContextItems.Dynamic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Dynamic node")]
    public class DynamicNode : BaseDynamicNode
    {
        [MetaTypeObject("Inner any node inherited from BaseDynamicNode", IsAssignableTypesAllowed = true)]
        public BaseDynamicNode Node { get; set; }

        public override object GetValue(TestContext context)
        {
            return Node.GetValue(context);
        }
    }
}
