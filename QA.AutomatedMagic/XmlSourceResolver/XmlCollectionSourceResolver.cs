namespace QA.AutomatedMagic.XmlSourceResolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using MetaMagic;

    public class XmlCollectionSourceResolver : ICollectionSourceResolver
    {
        public List<object> ResolveCollection(object source, MetaTypeCollectionMember collectionMember)
        {
            var xmlConfig = source as XElement;

            var root = XmlHelper.GetElementByNames(xmlConfig, collectionMember.Location.PossibleNames);

            if (root == null) return null;

            var rootElement = root as XElement;

            var childrenEls = XmlHelper.GetElementsByNames(rootElement, collectionMember.ChildrenLocation.Value.PossibleNames);

            return childrenEls.Cast<object>().ToList();
        }

        public object Serialize(object obj, MetaTypeCollectionMember collectionMember)
        {
            var collectionObj = collectionMember.GetValue(obj);
            var collectionChildren = collectionMember.CollectionWrapper.GetChildren(collectionObj);

            var collectionEl = new XElement(collectionMember.Info.Name);

            if (collectionMember.ChildrenMetaType != null)
            {
                var resolver = collectionMember.ChildrenMetaType.Value.SourceResolver.GetObjectSourceResolver();
                foreach (var child in collectionChildren)
                {
                    collectionEl.Add(resolver.Serialize(child, collectionMember.ChildrenMetaType.Value, null, collectionMember.IsAssignableTypesAllowed));
                }
            }
            else
            {
                var valueSourceResolver = collectionMember.ParentType.SourceResolver.GetValueSourceResolver();
                var name = collectionMember.ChildrenLocation.Value.PossibleNames.First();
                foreach (var child in collectionChildren)
                {
                    collectionEl.Add(valueSourceResolver.Serialize(child, name));
                }
            }

            return collectionEl;
        }
    }
}
