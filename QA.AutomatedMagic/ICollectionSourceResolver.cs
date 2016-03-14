namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using MetaMagic;


    public interface ICollectionSourceResolver
    {
        List<XElement> ResolveCollection(XElement source, MetaTypeCollectionMember collectionMember);
        XElement Serialize(object obj, MetaTypeCollectionMember collectionMember);
    }
}
