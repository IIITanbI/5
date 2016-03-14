namespace QA.AutomatedMagic
{
    using MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public interface IValueSourceResolver
    {
        XObject ResolveValue(XObject source, MetaTypeValueMember valueMember);
        XObject ResolveValue(XObject source, MetaLocation valueMember);
        XElement Serialize(object parentObj, MetaTypeValueMember valueMember);
        XElement Serialize(object obj, string name);
    }
}
