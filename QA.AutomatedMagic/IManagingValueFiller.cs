namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    public interface IManagingValueFiller
    {
        void FillInfoControls(object container, object parentObj, MetaTypeValueMember valueMember);
        void FillEditControls(object container, object parentObj, MetaTypeValueMember valueMember);
        object FillCreateControls(object container, MetaTypeValueMember valueMember);
        
        void FillInfoControls(object container, object obj, string name);
        Func<object> FillEditControls(object container, object obj, string name);
        object FillCreateControls(object container, Type type, string name);
    }
}
