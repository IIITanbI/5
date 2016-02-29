namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    public interface IObjectSourceResolver
    {
        object ResolveObject(object source, MetaTypeObjectMember objectMember);
        object ResolveObject(object source, MetaLocation location);
        object Serialize(object parentObj, MetaTypeObjectMember objectMember);
        object Serialize(object obj, MetaType metaType, string name, bool isAssignableTypesAllowed);
    }
}
