namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    public interface IManagingObjectFiller
    {
        void FillInfoControls(object container, object obj, MetaType metaType, string name, bool isAssignableTypesAllowed);
        void FillEditControls(object container, object obj, MetaType metaType, string name, bool isAssignableTypesAllowed);
        object FillCreateControls(object container, MetaType metaType, string name, bool isAssignableTypesAllowed);
        
        void FillInfoControls(object container, object parentObj, MetaTypeObjectMember objectMember);
        void FillEditControls(object container, object parentObj, MetaTypeObjectMember objectMember);
        object FillCreateControls(object container, MetaTypeObjectMember objectMember);
    }
}
