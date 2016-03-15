namespace QA.AutomatedMagic.ValueParsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public class EnumParser : IValueParser
    {
        public bool IsMatch(Type type)
        {
            return type.IsEnum;
        }

        public object Parse(XObject source, Type type)
        {
            var str = (source as XElement)?.Value 
                ?? (source as XAttribute)?.Value 
                ?? (source as XCData)?.Value
                ?? (source as XText)?.Value
                ?? source.ToString();

            try
            {
                var enumVal = Enum.Parse(type, str);
                if (!Enum.IsDefined(type, enumVal))
                    throw new ParseException(str, type);
                return enumVal;
            }
            catch
            {
                throw new ParseException(str, type);
            }
        }
    }
}
