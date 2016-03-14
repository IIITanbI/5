namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using MetaMagic;

    public interface IObjectSourceResolver
    {
        XElement ResolveObject(XElement source, MetaTypeObjectMember objectMember);
        XElement ResolveObject(XElement source, MetaLocation location);
        XElement Serialize(object parentObj, MetaTypeObjectMember objectMember);
        XElement Serialize(object obj, MetaType metaType, string name, bool isAssignableTypesAllowed);
    }
}
