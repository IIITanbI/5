namespace QA.AutomatedMagic.Framework.TestContextItems.Dynamic
{
    using MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [MetaType("Base type for all Dynamic nodes")]
    public abstract class BaseDynamicNode : BaseMetaObject
    {
        public abstract object GetValue(TestContext context);
    }
}
