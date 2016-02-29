﻿namespace QA.AutomatedMagic.MetaMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class MetaTypeObjectMember : MetaTypeMember
    {
        public IObjectSourceResolver ObjectSourceResolver { get; private set; }

        public Lazy<MetaType> MemberMetaType { get; private set; }

        public bool IsAssignableTypesAllowed { get; private set; }

        public MetaTypeObjectMember(MetaType parentType, PropertyInfo propertyInfo, MetaTypeObjectAttribute objectAttribute, List<MetaLocationAttribute> locationAttributes)
            : base(parentType, propertyInfo, objectAttribute, locationAttributes)
        {
            IsAssignableTypesAllowed = MemberType.IsInterface || MemberType.IsAbstract || objectAttribute.IsAssignableTypesAllowed;
            MemberMetaType = new Lazy<MetaType>(() => ReflectionManager.GetMetaType(MemberType));
        }

        public MetaTypeObjectMember(MetaType parentType, FieldInfo fieldInfo, MetaTypeObjectAttribute objectAttribute, List<MetaLocationAttribute> locationAttributes)
            : base(parentType, fieldInfo, objectAttribute, locationAttributes)
        {
            IsAssignableTypesAllowed = MemberType.IsInterface || MemberType.IsAbstract || objectAttribute.IsAssignableTypesAllowed;
            MemberMetaType = new Lazy<MetaType>(() => ReflectionManager.GetMetaType(MemberType));
        }

        public override void InitSourceResolver(Type type)
        {
            ObjectSourceResolver = type != null
                ? (IObjectSourceResolver)Activator.CreateInstance(type)
                : ParentType.SourceResolver.GetObjectSourceResolver();
        }

        public override object Parse(object source)
        {
            var resolvedValue = ObjectSourceResolver.ResolveObject(source, this);

            if (resolvedValue == null && !IsRequired)
                return null;

            if (!IsAssignableTypesAllowed)
                return MemberMetaType.Value.Parse(resolvedValue);

            foreach (var assignableType in MemberMetaType.Value.AssignableTypes)
            {
                var resolvedValue2 = ObjectSourceResolver.ResolveObject(resolvedValue, assignableType.Location);
                if (resolvedValue2 != null)
                    return assignableType.Parse(resolvedValue2);
            }

            throw new ParseException();
        }
    }
}
