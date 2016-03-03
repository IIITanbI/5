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
        Func<object> FillCreateControls(object container, Type type, string name);
        void FillInfoControls(object container, object parentObj, MetaTypeValueMember valueMember);
        void FillEditControls(object container, object parentObj, MetaTypeValueMember valueMember);

        void FillInfoControls(object container, object obj, string name);
        Func<object> FillEditControls(object container, object obj, string name);
    }
}
