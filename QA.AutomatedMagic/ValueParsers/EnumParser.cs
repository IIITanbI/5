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

        public object Parse(object source, Type type)
        {
            source = (source as XElement)?.Value ?? (source as XAttribute)?.Value ?? source;

            try
            {
                var enumVal = Enum.Parse(type, source.ToString());
                if (!Enum.IsDefined(type, enumVal))
                    throw new ParseException(source, type);
                return enumVal;
            }
            catch
            {
                throw new ParseException(source, type);
            }
        }
    }
}
