namespace QA.AutomatedMagic.Framework.TestContextItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using MetaMagic;

    [MetaType("TestContext item config")]
    public class BuildedContextItem : BaseTestContextItem
    {
        [MetaTypeObject("Source for context item")]
        [MetaLocation("source")]
        public Source ContextItemSource { get; set; }

        [MetaTypeValue("TypeName for test item")]
        [MetaLocation("typeName", "type")]
        public string TypeName { get; set; }

        [MetaTypeValue("TestItem config to desirialize")]
        [MetaLocation(true)]
        public XElement ItemConfig { get; set; }

        public void Build()
        {
            switch (ContextItemSource.ItemSourceType)
            {
                case Source.SourceType.Xml:
                case Source.SourceType.External:

                    var doc = XDocument.Load(ContextItemSource.Path);
                    var xml = doc.Element(ContextItemSource.RootElementName);
                    var testContextItem = MetaType.Parse<BuildedContextItem>(xml);

                    break;

                case Source.SourceType.Generic:
                case Source.SourceType.ExternalGeneric:

                    throw new NotImplementedException();

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
