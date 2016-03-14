namespace QA.AutomatedMagic.XmlSourceResolver
{
    using MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public class XmlObjectSourceResolver : IObjectSourceResolver
    {
        public XElement ResolveObject(XElement source, MetaTypeObjectMember objectMember)
        {
            var xmlConfig = source as XElement;

            var objectConfig = XmlHelper.GetElementByNames(xmlConfig, objectMember.Location.PossibleNames);

            if (objectConfig == null && objectMember.Location.CouldBeValue)
                return xmlConfig;

            return objectConfig;
        }

        public XElement ResolveObject(XElement source, MetaLocation location)
        {
            var xmlConfig = source as XElement;

            var objectConfig = XmlHelper.GetElementByNames(xmlConfig, location.PossibleNames);

            if (objectConfig == null && location.CouldBeValue)
                return xmlConfig;

            return objectConfig;
        }

        public XElement Serialize(object parentObj, MetaTypeObjectMember objectMember)
        {
            var memberValue = objectMember.GetValue(parentObj);
            if (memberValue == null) return null;
            if (!objectMember.IsAssignableTypesAllowed)
                return Serialize(memberValue, objectMember.MemberMetaType.Value, objectMember.Info.Name, false);

            var rootEl = new XElement(objectMember.Info.Name);
            var childEl = Serialize(memberValue, objectMember.MemberMetaType.Value, null, true);
            rootEl.Add(childEl);

            return rootEl;
        }

        public XElement Serialize(object obj, MetaType metaType, string name, bool isAssignableTypesAllowed)
        {
            if (isAssignableTypesAllowed)
            {
                var type = obj.GetType();
                metaType = AutomatedMagicManager.GetMetaType(type);
            }

            var rootEl = new XElement(name ?? metaType.Info.Name);

            foreach (var metaTypeMember in metaType.Members)
            {
                var valueMember = metaTypeMember as MetaTypeValueMember;
                if (valueMember != null)
                {
                    rootEl.Add(valueMember.ValueSourceResolver.Serialize(obj, valueMember));
                    continue;
                }

                var collectionMember = metaTypeMember as MetaTypeCollectionMember;
                if (collectionMember != null)
                {
                    rootEl.Add(collectionMember.CollectionSourceResolver.Serialize(obj, collectionMember));
                    continue;
                }

                var objectMember1 = metaTypeMember as MetaTypeObjectMember;
                if (objectMember1 != null)
                {
                    rootEl.Add(objectMember1.ObjectSourceResolver.Serialize(obj, objectMember1));
                    continue;
                }
            }

            return rootEl;
        }
    }
}
