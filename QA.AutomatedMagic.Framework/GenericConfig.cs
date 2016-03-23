namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;
    using System.Xml.Linq;

    [MetaType("Generic Config")]
    public class GenericConfig : BaseMetaObject
    {
        [MetaTypeValue("Specify enumerable item that will be used as generation source")]
        public string ForeachItemIn { get; set; }

        [MetaTypeValue("Template for child TestItem")]
        public XElement ChildTemplate { get; set; }
    }
}
