namespace QA.AutomatedMagic.CollectionWrappers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class LinkedListWrapper : ICollectionWrapper
    {
        private Dictionary<Type, MethodInfo> _addMethods = new Dictionary<Type, MethodInfo>();
        private Dictionary<Type, MethodInfo> _removeMethods = new Dictionary<Type, MethodInfo>();

        public void Add(object collection, object item, Type childType)
        {
            MethodInfo add = null;
            if (!_addMethods.ContainsKey(childType))
            {
                add = collection.GetType().GetMethod("AddLast", new[] { childType });
                _addMethods.Add(childType, add);
            }
            else
            {
                add = _addMethods[childType];
            }
            add.Invoke(collection, new[] { item });
        }

        public object CreateNew(Type childType, object arg)
        {
            var listType = typeof(LinkedList<>);
            var constructedListType = listType.MakeGenericType(childType);

            var instance = Activator.CreateInstance(constructedListType);
            return instance;
        }

        public List<object> GetChildren(object collection)
        {
            var childObjs = new List<object>();

            foreach (var child in (IEnumerable)collection)
                childObjs.Add(child);

            return childObjs;
        }

        public bool IsMatch(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(LinkedList<>);
        }

        public void Remove(object collection, object item, Type childType)
        {
            MethodInfo remove = null;
            if (!_removeMethods.ContainsKey(childType))
            {
                remove = collection.GetType().GetMethod("Remove", new[] { childType });
                _removeMethods.Add(childType, remove);
            }
            else
            {
                remove = _removeMethods[childType];
            }
            remove.Invoke(collection, new[] { item });
        }

        public string GetCollectionType()
        {
            return "LinkedList";
        }
    }
}
