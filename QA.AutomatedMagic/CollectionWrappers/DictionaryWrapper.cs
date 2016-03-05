namespace QA.AutomatedMagic.CollectionWrappers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class DictionaryWrapper : ICollectionWrapper
    {
        private Dictionary<Type, MethodInfo> _addMethods = new Dictionary<Type, MethodInfo>();
        private Dictionary<Type, MethodInfo> _removeMethods = new Dictionary<Type, MethodInfo>();

        public void Add(object collection, object item, Type childType)
        {
            var childMeta = AutomatedMagicManager.GetMetaType(item.GetType());

            MethodInfo add = null;
            if (!_addMethods.ContainsKey(childType))
            {
                add = collection.GetType().GetMethod("Add", new[] { childMeta.Key.MemberType, childType });
                _addMethods.Add(childType, add);
            }
            else
            {
                add = _addMethods[item.GetType()];
            }
            add.Invoke(collection, new[] { childMeta.Key.GetValue(item), item });
        }

        public object CreateNew(Type childType, object arg)
        {
            var childMeta = AutomatedMagicManager.GetMetaType(childType);
            var dictionaryType = typeof(Dictionary<,>);
            var constructedListType = dictionaryType.MakeGenericType(childMeta.Key.MemberType, childType);

            var instance = Activator.CreateInstance(constructedListType);
            return instance;
        }

        public List<object> GetChildren(object collection)
        {
            var childObjs = new List<object>();

            foreach (var child in ((IDictionary)collection).Values)
                childObjs.Add(child);

            return childObjs;
        }

        public bool IsMatch(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        public void Remove(object collection, object item, Type childType)
        {
            var childMeta = AutomatedMagicManager.GetMetaType(item.GetType());

            MethodInfo remove = null;
            if (!_removeMethods.ContainsKey(childType))
            {
                remove = collection.GetType().GetMethod("Remove", new[] { childMeta.Key.MemberType });
                _removeMethods.Add(childType, remove);
            }
            else
            {
                remove = _removeMethods[childType];
            }
            remove.Invoke(collection, new[] { childMeta.Key.GetValue(item) });
        }

        public string GetCollectionType()
        {
            return "Dictionary";
        }
    }
}
