namespace QA.AutomatedMagic
{
    using MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IValueSourceResolver
    {
        object ResolveValue(object source, MetaTypeValueMember valueMember);
        object ResolveValue(object source, MetaLocation valueMember);
        object Serialize(object parentObj, MetaTypeValueMember valueMember);
        object Serialize(object obj, string name);
    }
}
