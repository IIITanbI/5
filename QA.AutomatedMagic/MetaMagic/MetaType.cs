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
        #region Static
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

        public static object Parse(Type type, object source, IContext context = null, bool isAssignableTypesAllowed = false)
        {
            var metaType = AutomatedMagicManager.GetMetaType(type);
            return metaType.Parse(source, context, isAssignableTypesAllowed);
        }
        public static T Parse<T>(object source, IContext context = null, bool isAssignableTypesAllowed = false)
        {
            return (T)Parse(typeof(T), source, context, isAssignableTypesAllowed);
        }


        public static object SerializeObject(object obj)
        {
            var metaType = AutomatedMagicManager.GetMetaType(obj.GetType());
            return metaType.SourceResolver.GetObjectSourceResolver().Serialize(obj, metaType, metaType.Info.Name, false);
        }
        public static object CopyObject(object obj)
        {
            var metaType = AutomatedMagicManager.GetMetaType(obj.GetType());
            return metaType.Copy(obj);
        }
        public static T CopyObjectWithCast<T>(T obj)
        {
            var metaType = AutomatedMagicManager.GetMetaType(typeof(T));
            return (T)metaType.Copy(obj);
        }
        #endregion

        public Type TargetType { get; private set; }
        public MetaInfo Info { get; private set; }
        public MetaLocation Location { get; private set; }
        public List<MetaTypeMember> Members { get; private set; }
        public List<MetaType> AssignableTypes { get; private set; }
        public MetaTypeMember Key { get; private set; }

        public ISourceResolver SourceResolver { get; private set; }
        public IManagingFiller ManagingFiller { get; private set; }

        private string _keyName = null;
        private Type _sourceResolverType = null;
        private Type _managingFillerType = null;

        public MetaType(Type type)
        {
            TargetType = type;

            var metaTypeAttribute = type.GetCustomAttribute<MetaTypeAttribute>();
            Info = new MetaInfo { Name = type.Name, Description = metaTypeAttribute.Description };
            _keyName = metaTypeAttribute.KeyName;

            var curBase = type.BaseType;
            while (curBase != null && curBase != typeof(object))
            {
                var baseMetaTypeAttribute = curBase.GetCustomAttribute<MetaTypeAttribute>();
                if (baseMetaTypeAttribute == null) break;

                if (_keyName == null && baseMetaTypeAttribute.KeyName != null)
                    _keyName = baseMetaTypeAttribute.KeyName;

                if (_sourceResolverType == null && baseMetaTypeAttribute.SourceResolverType != null)
                    _sourceResolverType = baseMetaTypeAttribute.SourceResolverType;

                if (_managingFillerType == null && baseMetaTypeAttribute.ManagingFillerType != null)
                    _managingFillerType = baseMetaTypeAttribute.ManagingFillerType;

                curBase = curBase.BaseType;
            }

            SourceResolver = (ISourceResolver)Activator.CreateInstance(_sourceResolverType ?? DefaultSourceResolverType);
            ManagingFiller = (IManagingFiller)Activator.CreateInstance(_managingFillerType ?? DefaultManagingFillerType);

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

        public object Parse(object source, IContext context = null, bool isAssignableTypesAllowed = false)
        {
            object key = null;
            if (context != null && Key != null)
            {
                key = Key.Parse(source);
                if (key != null)
                {
                    if (context.Contains(TargetType, key.ToString()))
                    {
                        return context.ResolveValue(TargetType, key.ToString());
                    }
                }
            }

            if (isAssignableTypesAllowed)
            {
                var name = SourceResolver.GetSourceNodeName(source);
                var assignableType = AssignableTypes.FirstOrDefault(at => at.Info.Name == name);

                if (assignableType == null)
                    throw new ParseException($"Couldn't find assignable type with name: {name} for MetaType {Info}", TargetType);

                return assignableType.Parse(source, context, false);
            }

            var createdObject = Activator.CreateInstance(TargetType);

            var parsedMembersDict = new Dictionary<MetaTypeMember, bool>();
            Members.ForEach(m => parsedMembersDict.Add(m, false));

            while (parsedMembersDict.Any(m => !m.Value))
            {
                var memberToParse = parsedMembersDict.First(m => !m.Value).Key;
                if (memberToParse == Key && key != null)
                {
                    memberToParse.SetValue(createdObject, key);
                    parsedMembersDict[memberToParse] = true;
                    continue;
                }

                ParseMember(source, context, createdObject, parsedMembersDict, memberToParse);
            }

            return createdObject;
        }

        private void ParseMember(object source, IContext context, object createdObject, Dictionary<MetaTypeMember, bool> parsedMembersDict, MetaTypeMember memberToParse)
        {
            if (memberToParse.Constraint != null)
            {
                foreach (var constrEntry in memberToParse.Constraint.Constraints.Values)
                {
                    foreach (var memberConstr in constrEntry)
                    {
                        if (!parsedMembersDict[memberConstr.Member.Value])
                            ParseMember(source, context, createdObject, parsedMembersDict, memberConstr.Member.Value);
                    }

                    if (!constrEntry.All(ce =>
                            {
                                if (ce.IsPositive)
                                    return ce.Values.Contains(ce.Member.Value.GetValue(createdObject));
                                else
                                    return !ce.Values.Contains(ce.Member.Value.GetValue(createdObject));
                            })
                        )
                    {
                        parsedMembersDict[memberToParse] = true;
                        return;
                    }
                }
            }

            var memberValue = memberToParse.Parse(source, context);
            if (memberValue != null)
                memberToParse.SetValue(createdObject, memberValue);
            parsedMembersDict[memberToParse] = true;
        }

        public object Copy(object obj)
        {
            return Parse(SourceResolver.GetObjectSourceResolver().Serialize(obj, this, Info.Name, false));
        }

        public List<string> GetPaths(object obj)
        {
            var childPaths = new List<string>();

            foreach (var member in Members)
            {
                var cps = member.GetPaths(obj);
                if (cps != null)
                {
                    foreach (var cp in cps)
                    {
                        childPaths.Add(cp);
                    }
                }
            }

            return childPaths;
        }

        public object ResolvePath(string path, object parentObj)
        {
            var memberName = path;
            if (path.Contains('.'))
            {
                memberName = path.Substring(0, path.IndexOf('.'));
            }
            if (memberName.Contains('['))
            {
                memberName = memberName.Substring(0, path.IndexOf('['));
            }

            var member = Members.First(m => m.Info.Name == memberName);

            return member.ResolveValue(path, parentObj);
        }

        public override string ToString()
        {
            return Info.ToString();
        }
    }
}
