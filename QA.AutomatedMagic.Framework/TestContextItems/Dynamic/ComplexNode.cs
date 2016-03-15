namespace QA.AutomatedMagic.Framework.TestContextItems.Dynamic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Complex node")]
    public class ComplexNode : BaseDynamicNode
    {
        [MetaTypeCollection("List of nodes inherited from BaseDynamicNode")]
        public List<BaseDynamicNode> Nodes { get; set; }

        public override object GetValue(TestContext context)
        {
            var sb = new StringBuilder();

            foreach (var node in Nodes)
            {
                sb.Append(node.GetValue(context).ToString());
            }

            return sb.ToString();
        }
    }
}
