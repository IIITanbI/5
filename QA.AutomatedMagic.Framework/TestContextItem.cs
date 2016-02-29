namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using MetaMagic;
    
    [MetaType("TestContext item config")]
    [MetaLocation("contextItem")]
    public class TestContextItem : Source
    {
        [MetaTypeValue("TypeName for test item")]
        [MetaLocation("typeName", "type")]
        public string TypeName { get; set; }

        [MetaTypeValue("TestItem config to desirialize")]
        [MetaLocation(true)]
        public XElement ItemConfig { get; set; }

        public List<TestContextItem> Build()
        {
            switch (ItemSourceType)
            {
                case SourceType.Xml:
                    
                    return new List<TestContextItem> { this };

                case SourceType.External:

                    var doc = XDocument.Load(Path);
                    var xml = doc.Element(RootElementName);
                    var testContextItem = MetaType.Parse<TestContextItem>(xml);

                    return testContextItem.Build();

                case SourceType.Generic:
                case SourceType.ExternalGeneric:

                    throw new NotImplementedException();

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
