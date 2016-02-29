namespace QA.AutomatedMagic.XmlSourceResolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public static class XmlHelper
    {
        public static XElement GetElementByNames(XElement parentElement, List<string> possibleNames)
        {
            foreach (var name in possibleNames)
            {
                var el = parentElement.Element(name);
                if (el != null)
                    return el;
            }
            return null;
        }

        public static XAttribute GetAttributeByNames(XElement parentElement, List<string> possibleNames)
        {
            foreach (var name in possibleNames)
            {
                var att = parentElement.Attribute(name);
                if (att != null)
                    return att;
            }
            return null;
        }

        public static List<XElement> GetElementsByNames(XElement parentElement, List<string> possibleNames)
        {
            var list = new List<XElement>();

            foreach (var element in parentElement.Elements())
            {
                if(possibleNames.Contains(element.Name.LocalName))
                    list.Add(element);
            }

            return list;
        }
    }
}
