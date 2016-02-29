namespace QA.AutomatedMagic.ValueParsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public class XElementParser : IValueParser
    {
        public bool IsMatch(Type type)
        {
            return typeof(XElement).IsAssignableFrom(type);
        }

        public object Parse(object source, Type type)
        {
            return source;
        }
    }
}
