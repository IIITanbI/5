namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    public class TestContextValueInfo
    {
        public string ValueKey { get; set; }
        public MetaType ValueMetaType { get; set; }
        public Lazy<IMetaObject> ValueValue { get; set; }
    }
}
