﻿namespace QA.AutomatedMagic.ValueParsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    public class ParsableTypesParser : IValueParser
    {
        public bool IsMatch(Type type)
        {
            return type.IsPrimitive || type.Name == "DateTime" || type.Name == "TimeSpan";
        }

        public object Parse(object source, Type type)
        {
            source = (source as XElement)?.Value ?? (source as XAttribute)?.Value ?? source;

            var m = type.GetMethod("TryParse", new Type[] { typeof(string), type.MakeByRefType() });
            object[] args = { source, null };
            var r = (bool)m.Invoke(null, args);

            if (!r)
                throw new ParseException(source, type);

            return args[1];
        }
    }
}
