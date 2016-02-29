namespace QA.AutomatedMagic.ValueParsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class StringParser : IValueParser
    {
        public bool IsMatch(Type type)
        {
            return type.Name == "String";
        }

        public object Parse(object source, Type type)
        {
            return source;
        }
    }
}
