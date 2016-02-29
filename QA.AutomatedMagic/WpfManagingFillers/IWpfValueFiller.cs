namespace QA.AutomatedMagic.WpfManagingFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;

    public interface IWpfValueFiller
    {
        Action<object> SetValue { get; }
        Func<object> GetValue { get; }
        bool IsMatch(Type type);
        void FillInfoControls(StackPanel panel, object obj, Type type);
        void FillEditControls(StackPanel panel, object obj, Type type);
        void FillCreateControls(StackPanel panel, Type type);
    }
}
