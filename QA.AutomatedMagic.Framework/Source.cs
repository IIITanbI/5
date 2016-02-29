namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using MetaMagic;

    [MetaType("Source configuration")]
    public class Source : BaseMetaObject
    {
        [MetaTypeValue("Source type. Xml, External, Generic or ExternalGeneric", IsRequired = false)]
        [MetaLocation("sourceType", "itemSourceType")]
        public SourceType ItemSourceType { get; set; } = SourceType.Xml;

        [MetaTypeValue("Root element name")]
        [MetaLocation("root", "rootElement")]
        public string RootElementName { get; set; } = null;

        [MetaTypeValue("Root element name")]
        [MetaLocation(true)]
        public string Path { get; set; } = null;

        [MetaTypeValue("XElement used as template for generation")]
        [MetaLocation(true, "xmlTemplate", "generationTemplate")]
        public XElement XmlSource { get; set; }

        public enum SourceType
        {
            Xml, External, Generic, ExternalGeneric
        }
    }
}
