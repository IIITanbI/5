namespace QA.AutomatedMagic.MetaMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Reflection;
    using XmlSourceResolver;
    using ValueParsers;
    using CollectionWrappers;

    public class MetaType
    {
        public static Type DefaultSourceResolverType { get; set; } = typeof(BaseMetaObjectXmlSourceResolver);
        public static Type DefaultManagingFillerType { get; set; } = typeof(BaseMetaObjectWpfFiller);
        public static List<IValueParser> ValueParsers { get; set; } = new List<IValueParser>
        {
            new StringParser(),
            new ParsableTypesParser(),
            new EnumParser(),
            new XElementParser()
        };
        public static List<ICollectionWrapper> CollectionWrappers { get; set; } = new List<ICollectionWrapper>
        {
            new ListWrapper(),
            new LinkedListWrapper(),
            new DictionaryWrapper(),
            new ArrayWrapper()
        };

        public static object Parse(Type type, object source)
        {
            var metaType = ReflectionManager.GetMetaType(type);
            return metaType.Parse(source);
        }
        public static T Parse<T>(object source)
        {
            return (T)Parse(typeof(T), source);
        }

        public static object SerializeObject(object obj)
        {
            var metaType = ReflectionManager.GetMetaType(obj.GetType());
            return metaType.SourceResolver.GetObjectSourceResolver().Serialize(obj, metaType, metaType.Info.Name, false);
        }

        public Type TargetType { get; private set; }
        public MetaInfo Info { get; private set; }
        public MetaLocation Location { get; private set; }
        public List<MetaTypeMember> Members { get; private set; }
        public List<MetaType> AssignableTypes { get; private set; }
        public MetaTypeMember Key { get; private set; }

        public ISourceResolver SourceResolver { get; private set; }
        public IManagingFiller ManagingFiller { get; private set; }

        private string _keyName;

        public MetaType(Type type)
        {
            TargetType = type;

            var metaTypeAttribute = type.GetCustomAttribute<MetaTypeAttribute>();
            Info = new MetaInfo { Name = type.Name, Description = metaTypeAttribute.Description };
            _keyName = metaTypeAttribute.KeyName;

            SourceResolver = (ISourceResolver)Activator.CreateInstance(metaTypeAttribute.SourceResolverType ?? DefaultSourceResolverType);
            ManagingFiller = (IManagingFiller)Activator.CreateInstance(metaTypeAttribute.ManagingFillerType ?? DefaultManagingFillerType);

            var locationAttributes = type.GetCustomAttributes<MetaLocationAttribute>().ToList();
            locationAttributes.Add(new MetaLocationAttribute(TargetType.Name));
            Location = new MetaLocation(locationAttributes);

            AssignableTypes = new List<MetaType>();
            if (!type.IsAbstract)
                AssignableTypes.Add(this);

            Members = new List<MetaTypeMember>();

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var memberAtt = property.GetCustomAttribute<MetaTypeMemberAttribute>(true);
                if (memberAtt != null)
                {
                    var locationAtts = property.GetCustomAttributes<MetaLocationAttribute>().ToList();
                    locationAtts.Add(new MetaLocationAttribute(property.Name));
                    var collectionAtt = memberAtt as MetaTypeCollectionAttribute;
                    if (collectionAtt != null)
                    {
                        var newMember = new MetaTypeCollectionMember(this, property, collectionAtt, locationAtts);
                        AddMember(newMember);
                        continue;
                    }
                    var objectAtt = memberAtt as MetaTypeObjectAttribute;
                    if (objectAtt != null)
                    {
                        var newMember = new MetaTypeObjectMember(this, property, objectAtt, locationAtts);
                        AddMember(newMember);
                        continue;
                    }
                    var valueAtt = memberAtt as MetaTypeValueAttribute;
                    if (valueAtt != null)
                    {
                        var newMember = new MetaTypeValueMember(this, property, valueAtt, locationAtts);
                        AddMember(newMember);
                        continue;
                    }
                }
            }

            var fields = type.GetFields();
            foreach (var field in fields)
            {
                var memberAtt = field.GetCustomAttribute<MetaTypeMemberAttribute>(true);
                if (memberAtt != null)
                {
                    var locationAtts = field.GetCustomAttributes<MetaLocationAttribute>().ToList();
                    locationAtts.Add(new MetaLocationAttribute(field.Name));
                    var collectionAtt = memberAtt as MetaTypeCollectionAttribute;
                    if (collectionAtt != null)
                    {
                        var newMember = new MetaTypeCollectionMember(this, field, collectionAtt, locationAtts);
                        AddMember(newMember);
                        continue;
                    }
                    var objectAtt = memberAtt as MetaTypeObjectAttribute;
                    if (objectAtt != null)
                    {
                        var newMember = new MetaTypeObjectMember(this, field, objectAtt, locationAtts);
                        AddMember(newMember);
                        continue;
                    }
                    var valueAtt = memberAtt as MetaTypeValueAttribute;
                    if (valueAtt != null)
                    {
                        var newMember = new MetaTypeValueMember(this, field, valueAtt, locationAtts);
                        AddMember(newMember);
                        continue;
                    }
                }
            }
        }
        private void AddMember(MetaTypeMember member)
        {
            if (Key == null && _keyName != null && member.Info.Name == _keyName)
                Key = member;

            Members.Add(member);
        }

        public object Parse(object source)
        {
            var createdObject = Activator.CreateInstance(TargetType);

            foreach (var member in Members)
            {
                var memberValue = member.Parse(source);
                if (memberValue != null)
                    member.SetValue(createdObject, memberValue);
            }

            return createdObject;
        }

        public override string ToString()
        {
            return Info.ToString();
        }
    }
}
