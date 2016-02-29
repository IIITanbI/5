namespace QA.AutomatedMagic
{
    using MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IManagingCollectionFiller
    {
        object FillCreateControlls(object container, object parentObj, MetaTypeCollectionMember collectionMember);

        object FillEditControls(object container, object parentObj, MetaTypeCollectionMember collectionMember);

        void FillInfoControlls(object container, object parentObj, MetaTypeCollectionMember collectionMember);
    }
}
