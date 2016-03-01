namespace QA.AutomatedMagic.XmlSourceResolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using MetaMagic;

    public class XmlValueSourceResolver : IValueSourceResolver
    {
        public object ResolveValue(object source, MetaTypeValueMember valueMember)
        {
            var xmlConfig = source as XElement;

            var possibleNames = valueMember.Location.PossibleNames;
            var el = XmlHelper.GetElementByNames(xmlConfig, possibleNames);

            if (el != null)
            {
                var fn = el.FirstNode;
                if (fn != null && fn.NodeType == System.Xml.XmlNodeType.CDATA)
                    return (el.FirstNode as XCData).Value;
                else
                    return el.Value;
            }

            var att = XmlHelper.GetAttributeByNames(xmlConfig, possibleNames);
            if (att != null)
                return att.Value;

            if (valueMember.Location.CouldBeValue)
            {
                var fn = xmlConfig.FirstNode;
                if (fn != null && fn.NodeType == System.Xml.XmlNodeType.CDATA)
                    return (xmlConfig.FirstNode as XCData).Value;
                else
                    return xmlConfig.Value;
            }

            return null;
        }

        public object ResolveValue(object source, MetaLocation location)
        {
            var xmlConfig = source as XElement;

            var possibleNames = location.PossibleNames;
            var el = XmlHelper.GetElementByNames(xmlConfig, possibleNames);

            if (el != null)
            {
                var fn = el.FirstNode;
                if (fn != null && fn.NodeType == System.Xml.XmlNodeType.CDATA)
                    return (el.FirstNode as XCData).Value;
                else
                    return el.Value;
            }

            var att = XmlHelper.GetAttributeByNames(xmlConfig, possibleNames);
            if (att != null)
                return att.Value;

            if (location.CouldBeValue)
            {
                var fn = xmlConfig.FirstNode;
                if (fn != null && fn.NodeType == System.Xml.XmlNodeType.CDATA)
                    return (xmlConfig.FirstNode as XCData).Value;
                else
                    return xmlConfig.Value;
            }

            return null;
        }

        public object Serialize(object parentObj, MetaTypeValueMember valueMember)
        {
            var value = valueMember.GetValue(parentObj);
            if (value == null) return null;
            var el = new XElement(valueMember.Info.Name, new XCData(value.ToString()));
            return el;
        }

        public object Serialize(object obj, string name)
        {
            var el = new XElement(name, new XCData(obj.ToString()));
            return el;
        }
    }
}
