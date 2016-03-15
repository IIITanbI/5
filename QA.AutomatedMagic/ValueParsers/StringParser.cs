namespace QA.AutomatedMagic.ValueParsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    public class StringParser : IValueParser
    {
        public bool IsMatch(Type type)
        {
            return type.Name == "String";
        }

        public object Parse(XObject source, Type type)
        {
            var str = (source as XElement)?.Value
                ?? (source as XAttribute)?.Value
                ?? (source as XCData)?.Value
                ?? (source as XText)?.Value
                ?? source.ToString();

            return str;
        }
    }
}
