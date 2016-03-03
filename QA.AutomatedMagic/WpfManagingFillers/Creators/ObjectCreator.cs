namespace QA.AutomatedMagic.WpfManagingFillers.Creators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using MetaMagic;


    public class ObjectCreator
    {
        private MetaTypeObjectMember _objectMember;
        private Panel _headerPanel;
        private object _parentObj;

        public ObjectCreator(Panel headerPanel, MetaTypeObjectMember objectMember, object parentObj)
        {
            _headerPanel = headerPanel;
            _objectMember = objectMember;
            _parentObj = parentObj;
        }
    }
}
