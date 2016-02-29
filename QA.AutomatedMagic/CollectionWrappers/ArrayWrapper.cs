namespace QA.AutomatedMagic.CollectionWrappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ArrayWrapper : ICollectionWrapper
    {
        public void Add(object collection, object item, Type childType)
        {
            throw new NotImplementedException();
        }

        public object CreateNew(Type childType, object arg)
        {
            throw new NotImplementedException();
        }

        public List<object> GetChildren(object collection)
        {
            throw new NotImplementedException();
        }

        public string GetCollectionType()
        {
            return "Array";
        }

        public bool IsMatch(Type type)
        {
            return type.IsArray;
        }

        public void Remove(object collection, object item, Type childType)
        {
            throw new NotImplementedException();
        }
    }
}
