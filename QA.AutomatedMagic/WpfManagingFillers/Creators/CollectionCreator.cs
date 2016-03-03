namespace QA.AutomatedMagic.WpfManagingFillers.Creators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using MetaMagic;

    public class CollectionCreator
    {
        private MetaTypeCollectionMember _collectionMember;
        private Panel _headerPanel;
        private object _parentObj;

        public CollectionCreator(Panel headerPanel, MetaTypeCollectionMember collectionMember, object parentObj)
        {
            _headerPanel = headerPanel;
            _collectionMember = collectionMember;
            _parentObj = parentObj;
        }
    }
}
