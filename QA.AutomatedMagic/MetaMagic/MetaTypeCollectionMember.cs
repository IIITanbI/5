namespace QA.AutomatedMagic.MetaMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class MetaTypeCollectionMember : MetaTypeMember
    {
        public ICollectionSourceResolver CollectionSourceResolver { get; private set; }
        public Lazy<MetaLocation> ChildrenLocation { get; private set; }

        public Lazy<MetaType> ChildrenMetaType { get; private set; } = null;
        public Type ChildrenType { get; private set; }
        public bool IsAssignableTypesAllowed { get; private set; }

        public ICollectionWrapper CollectionWrapper { get; private set; }
        public IValueParser ChildValueParser { get; private set; } = null;
        public IManagingCollectionFiller ManagingCollectionFiller { get; private set; }

        public MetaTypeCollectionMember(MetaType parentType, PropertyInfo propertyInfo, MetaTypeCollectionAttribute collectionAttribute, List<MetaLocationAttribute> locationAttributes)
            : base(parentType, propertyInfo, collectionAttribute, locationAttributes)
        {
            Init(collectionAttribute);
        }

        public MetaTypeCollectionMember(MetaType parentType, FieldInfo fieldInfo, MetaTypeCollectionAttribute collectionAttribute, List<MetaLocationAttribute> locationAttributes)
           : base(parentType, fieldInfo, collectionAttribute, locationAttributes)
        {
            Init(collectionAttribute);
        }

        public void Init(MetaTypeCollectionAttribute collectionAttribute)
        {
            ChildrenType = MemberType.IsArray
                ? MemberType.GetElementType()
                : MemberType.GetGenericArguments().Last();

            IsAssignableTypesAllowed = ChildrenType.IsInterface || ChildrenType.IsAbstract || collectionAttribute.IsAssignableTypesAllowed;


            if (typeof(BaseMetaObject).IsAssignableFrom(ChildrenType))
            {
                ChildrenMetaType = new Lazy<MetaType>(() => ReflectionManager.GetMetaType(ChildrenType));

                if (!IsAssignableTypesAllowed)
                {
                    ChildrenLocation = new Lazy<MetaLocation>(() =>
                    {
                        var location = new MetaLocation(ChildrenType.Name);
                        location.Add(collectionAttribute.PossibleChildrenNames);
                        location.Add(ChildrenMetaType.Value.Location.PossibleNames);
                        return location;
                    });
                }
                else
                {
                    ChildrenLocation = new Lazy<MetaLocation>(() =>
                    {
                        var assignableTypes = ChildrenMetaType.Value.AssignableTypes;

                        var location = new MetaLocation(false);
                        foreach (var type in assignableTypes)
                            location.Add(type.Location.PossibleNames);

                        return location;
                    });
                }
            }
            else
            {
                ChildrenLocation = new Lazy<MetaLocation>(() =>
                {
                    var location = new MetaLocation(ChildrenType.Name);
                    location.Add(collectionAttribute.PossibleChildrenNames);
                    return location;
                });

                ChildValueParser = collectionAttribute.ChildValueParserType != null
                    ? (IValueParser)Activator.CreateInstance(collectionAttribute.ChildValueParserType)
                    : MetaType.ValueParsers.First(vp => vp.IsMatch(ChildrenType));
            }

            CollectionWrapper = collectionAttribute.CollectionWrapperType != null
                ? (ICollectionWrapper)Activator.CreateInstance(collectionAttribute.CollectionWrapperType)
                : MetaType.CollectionWrappers.First(cw => cw.IsMatch(MemberType));

            ManagingCollectionFiller = collectionAttribute.CollectionManagingFillerType != null
                ? (IManagingCollectionFiller)Activator.CreateInstance(collectionAttribute.CollectionManagingFillerType)
                : ParentType.ManagingFiller.GetManagingCollectionFiller();
        }

        public override void InitSourceResolver(Type type)
        {
            CollectionSourceResolver = type != null
                ? (ICollectionSourceResolver)Activator.CreateInstance(type)
                : ParentType.SourceResolver.GetCollectionSourceReolver();
        }

        public override object Parse(object source, IContext context = null)
        {
            var childrenResolvedSources = CollectionSourceResolver.ResolveCollection(source, this);

            if (childrenResolvedSources == null && !IsRequired)
                return null;

            var childObjs = new List<object>();

            if (ChildrenMetaType == null)
            {
                var childValueResolver = ParentType.SourceResolver.GetValueSourceResolver();
                var location = new MetaLocation(true);

                foreach (var childResolvedSource in childrenResolvedSources)
                {
                    var valueToParse = childValueResolver.ResolveValue(childResolvedSource, location);
                    var childValue = ChildValueParser.Parse(valueToParse, ChildrenType);
                    childObjs.Add(childValue);
                }
            }
            else
            {
                if (!IsAssignableTypesAllowed)
                {
                    foreach (var childResolvedSource in childrenResolvedSources)
                    {
                        var childValue = ChildrenMetaType.Value.Parse(childResolvedSource, context);
                        childObjs.Add(childValue);
                    }
                }
                else
                {
                    foreach (var childResolvedSource in childrenResolvedSources)
                    {
                        var name = ParentType.SourceResolver.GetSourceNodeName(childResolvedSource);
                        var assignableType = ChildrenMetaType.Value.AssignableTypes.First(at => at.Location.PossibleNames.Contains(name));
                        var childValue = assignableType.Parse(childResolvedSource, context);
                        childObjs.Add(childValue);
                    }
                }
            }

            var createdCollection = CollectionWrapper.CreateNew(ChildrenType, null);

            foreach (var childObj in childObjs)
            {
                CollectionWrapper.Add(createdCollection, childObj, ChildrenType);
            }

            return createdCollection;
        }

        public override List<string> GetPaths(object parentObj)
        {
            var root = Info.Name;

            var collectionObj = GetValue(parentObj);
            if (collectionObj == null) return null;

            var children = CollectionWrapper.GetChildren(collectionObj);

            var childPaths = new List<string>();
            childPaths.Add(root);

            int counter = 1;
            if (ChildrenMetaType == null)
            {
                foreach (var child in children)
                {
                    childPaths.Add($"{root}[{counter++}]");
                }
            }
            else
            {
                foreach (var child in children)
                {
                    var cps = ChildrenMetaType.Value.GetPaths(child);
                    var p = $"{root}[{counter++}]";
                    foreach (var cp in cps)
                    {
                        childPaths.Add($"{p}.{cp}");
                    }
                }
            }

            return childPaths;
        }

        public override object ResolveValue(string path, object parentObj)
        {
            var collectionObj = GetValue(parentObj);

            var firstName = path;
            if (path.Contains('.'))
            {
                firstName = path.Substring(0, path.IndexOf('.'));
            }

            if (firstName == Info.Name) return collectionObj;

            var indexVal = firstName.Substring(firstName.IndexOf('[') + 1, firstName.IndexOf(']') - firstName.IndexOf('[') - 1);
            var index = int.Parse(indexVal);

            var children = CollectionWrapper.GetChildren(collectionObj);

            if (firstName == path)
                return children[index - 1];

            return ChildrenMetaType.Value.ResolvePath(path.Substring(path.IndexOf('.') + 1), children[index - 1]);
        }
    }
}
