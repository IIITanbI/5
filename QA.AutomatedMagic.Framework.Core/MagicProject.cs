namespace QA.AutomatedMagic.Framework.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class MagicProject
    {
        public string Path { get; set; }
        public string Name { get; set; }

        public List<MagicReference> References { get; set; }

        public void Parse()
        {
            var projectXml = XDocument.Parse(Path);
            var referenceEls = projectXml.XPathSelectElements("//*[name()='ItemGroup']/*[name()='Reference']/*[name()='HintPath']/..").ToList();
            for (int i = 0; i < referenceEls.Count; i++)
            {
                var includeAtt = referenceEls[i].Attributes().First();
                var includeValue = includeAtt.Value;
                if (includeValue.Contains(','))
                {
                    includeValue = includeValue.Split(',')[0];
                    includeAtt.Value = includeValue;
                }
            }
        }
    }
}
