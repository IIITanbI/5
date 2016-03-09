namespace QA.AutomatedMagic.MetaMagic
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
            MemberMetaType = new Lazy<MetaType>(() => AutomatedMagicManager.GetMetaType(MemberType));
        }

        public MetaTypeObjectMember(MetaType parentType, FieldInfo fieldInfo, MetaTypeObjectAttribute objectAttribute, List<MetaLocationAttribute> locationAttributes)
            : base(parentType, fieldInfo, objectAttribute, locationAttributes)
        {
            IsAssignableTypesAllowed = MemberType.IsInterface || MemberType.IsAbstract || objectAttribute.IsAssignableTypesAllowed;
            MemberMetaType = new Lazy<MetaType>(() => AutomatedMagicManager.GetMetaType(MemberType));
        }

        public override void InitSourceResolver(Type type)
        {
            ObjectSourceResolver = type != null
                ? (IObjectSourceResolver)Activator.CreateInstance(type)
                : ParentType.SourceResolver.GetObjectSourceResolver();
        }

        public override object Parse(object source, IContext context = null)
        {
            var resolvedValue = ObjectSourceResolver.ResolveObject(source, this);

            if (resolvedValue == null && !IsRequired)
                return null;

            if (resolvedValue == null)
                throw new ParseException(source, this);

            if (!IsAssignableTypesAllowed)
                return MemberMetaType.Value.Parse(resolvedValue, context);

            foreach (var assignableType in MemberMetaType.Value.AssignableTypes)
            {
                var resolvedValue2 = ObjectSourceResolver.ResolveObject(resolvedValue, assignableType.Location);
                if (resolvedValue2 != null)
                    return assignableType.Parse(resolvedValue2, context);
            }

            throw new ParseException($"Couldn't find assignable type" ,source, this);
        }

        public override List<string> GetPaths(object parentObj)
        {
            var value = GetValue(parentObj);
            if (value == null) return null;

            var root = Info.Name;

            var childPaths = new List<string>();
            childPaths.Add(root);

            var cps = MemberMetaType.Value.GetPaths(value);

            foreach (var cp in cps)
            {
                childPaths.Add($"{root}.{cp}");
            }

            return childPaths;
        }

        public override object ResolveValue(string path, object parentObj)
        {
            var obj = GetValue(parentObj);

            var firstName = path;
            if (path.Contains('.'))
            {
                firstName = path.Substring(0, path.IndexOf('.'));
            }

            if (firstName == path) return obj;
            
            path = path.Substring(path.IndexOf('.') + 1);

            return MemberMetaType.Value.ResolvePath(path, obj);
        }
    }
}
