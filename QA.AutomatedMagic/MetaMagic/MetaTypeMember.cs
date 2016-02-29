﻿namespace QA.AutomatedMagic.MetaMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class MetaTypeMember
    {
        public MetaType ParentType { get; private set; }
        public MetaInfo Info { get; private set; }
        public MetaLocation Location { get; private set; }
        public Type MemberType { get; private set; }
        public bool IsRequired { get; private set; } = true;

        private Func<object, object> Getter { get; set; }
        private Action<object, object> Setter { get; set; }

        private MetaTypeMember(MetaType parentType, string name, MetaTypeMemberAttribute memberAttribute, List<MetaLocationAttribute> locationAttributes)
        {
            ParentType = parentType;
            InitSourceResolver(memberAttribute.SourceResolverType);

            IsRequired = memberAttribute.IsRequired;

            Info = new MetaInfo { Name = name, Description = memberAttribute.Description };
            Location = new MetaLocation(locationAttributes);
        }

        public MetaTypeMember(MetaType parentType, PropertyInfo propertyInfo, MetaTypeMemberAttribute memberAttribute, List<MetaLocationAttribute> locationAttributes)
            : this(parentType, propertyInfo.Name, memberAttribute, locationAttributes)
        {
            Getter = new Func<object, object>(obj => propertyInfo.GetValue(obj));
            Setter = new Action<object, object>((obj, value) => propertyInfo.SetValue(obj, value));
            MemberType = propertyInfo.PropertyType;
        }

        public MetaTypeMember(MetaType parentType, FieldInfo fieldInfo, MetaTypeMemberAttribute memberAttribute, List<MetaLocationAttribute> locationAttributes)
            : this(parentType, fieldInfo.Name, memberAttribute, locationAttributes)
        {
            Getter = new Func<object, object>(obj => fieldInfo.GetValue(obj));
            Setter = new Action<object, object>((obj, value) => fieldInfo.SetValue(obj, value));
            MemberType = fieldInfo.FieldType;
        }

        public object GetValue(object obj)
        {
            return Getter(obj);
        }

        public void SetValue(object obj, object value)
        {
            Setter(obj, value);
        }

        public abstract object Parse(object source);
        public abstract void InitSourceResolver(Type type);

        public override string ToString()
        {
            return Info.ToString();
        }
    }
}