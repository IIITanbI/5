namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Base class for MetaObject with name", keyName: "UniqueName")]
    public class BaseNamedMetaObject : BaseMetaObject
    {
        [MetaTypeValue("Unique name for object")]
        public string UniqueName { get; set; }
    }
}
