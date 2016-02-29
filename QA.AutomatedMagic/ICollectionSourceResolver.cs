namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;


    public interface ICollectionSourceResolver
    {
        List<object> ResolveCollection(object source, MetaTypeCollectionMember collectionMember);
        object Serialize(object obj, MetaTypeCollectionMember collectionMember);
    }
}
