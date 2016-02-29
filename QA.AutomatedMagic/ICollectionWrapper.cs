namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public interface ICollectionWrapper
    {
        bool IsMatch(Type type);
        object CreateNew(Type childType, object arg);
        void Add(object collection, object item, Type childType);
        void Remove(object collection, object item, Type childType);
        List<object> GetChildren(object collection);
        string GetCollectionType();
    }
}
