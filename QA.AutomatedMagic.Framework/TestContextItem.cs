namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Base class for Test context item")]
    public abstract class TestContextItem : BaseMetaObject
    {
        [MetaTypeValue("Name of context item")]
        public string Name { get; set; }

        public abstract List<TestContextValueInfo> Build(TestContext context);
    }
}
