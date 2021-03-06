﻿namespace QA.AutomatedMagic.MetaMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Reflection;
    using System.Xml.Linq;
    public class MetaTypeValueMember : MetaTypeMember
    {
        public IValueSourceResolver ValueSourceResolver { get; private set; }
        public IValueParser ValueParser { get; private set; }

        public MetaTypeValueMember(MetaType parentType, PropertyInfo propertyInfo, MetaTypeValueAttribute valueAttribute, List<MetaLocationAttribute> locationAttributes)
            : base(parentType, propertyInfo, valueAttribute, locationAttributes)
        {
            InitParser(valueAttribute);
        }

        public MetaTypeValueMember(MetaType parentType, FieldInfo fieldInfo, MetaTypeValueAttribute valueAttribute, List<MetaLocationAttribute> locationAttributes)
            : base(parentType, fieldInfo, valueAttribute, locationAttributes)
        {
            InitParser(valueAttribute);
        }

        public void InitParser(MetaTypeValueAttribute valueAttribute)
        {
            ValueParser = valueAttribute.ValueParserType != null
                ? (IValueParser)Activator.CreateInstance(valueAttribute.ValueParserType)
                : MetaType.ValueParsers.First(vp => vp.IsMatch(MemberType));
        }

        public override void InitSourceResolver(Type type)
        {
            ValueSourceResolver = type != null
                ? (IValueSourceResolver)Activator.CreateInstance(type)
                : ParentType.SourceResolver.GetValueSourceResolver();
        }

        public override object Parse(XElement source)
        {
            var resolvedSource = ValueSourceResolver.ResolveValue(source, this);
            if (resolvedSource == null && !IsRequired)
                return null;

            if (resolvedSource == null)
                throw new ParseException("Couldn't resolve value from source", source, this);

            return ValueParser.Parse(resolvedSource, MemberType);
        }

        public override List<string> GetPaths(object parentObj)
        {
            return new List<string> { Info.Name };
        }

        public override object ResolveValue(string path, object parentObj)
        {
            return GetValue(parentObj);
        }
    }
}
