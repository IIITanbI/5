namespace QA.AutomatedMagic.Framework.TestContextItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Context object item")]
    public class ContextObjectItem : TestContextItem
    {
        [MetaTypeCollection("List of context objects. Each must have Key", "object", IsAssignableTypesAllowed = true)]
        public List<IMetaObject> Objects { get; set; }

        public override List<IMetaObject> Build(TestContext context)
        {
            return Objects;
        }
    }
}
