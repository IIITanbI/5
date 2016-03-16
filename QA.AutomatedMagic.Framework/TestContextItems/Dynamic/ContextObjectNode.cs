namespace QA.AutomatedMagic.Framework.TestContextItems.Dynamic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Context object node")]
    public class ContextObjectNode : BaseDynamicNode
    {
        [MetaTypeValue("Path to object in context")]
        public string Path { get; set; }

        public override object GetValue(TestContext context)
        {
            return MetaType.SerializeObject(context.ResolveValue(Path));
        }
    }
}
