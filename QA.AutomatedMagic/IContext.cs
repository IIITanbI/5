namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    public interface IContext
    {
        object ResolveValue(string path);
        object ResolveValue(Type type, string name);

        bool Contains(Type type, string name);

        void Add(Type type, string name, object value);
        void Add(string path, object value);

        string ResolveBind(string stringWithBind);
    }
}
