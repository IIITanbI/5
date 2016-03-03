namespace QA.AutomatedMagic.WpfManagingFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;

    public abstract class BaseWpfValueFiller : IWpfValueFiller
    {
        public Action<object> SetValue { get; protected set; }
        public Func<object> GetValue { get; protected set; }

        public abstract void FillCreateControls(StackPanel panel, Type type);

        public abstract void FillEditControls(StackPanel panel, object obj, Type type);

        public void FillInfoControls(StackPanel panel, object obj, Type type)
        {
            var label = new Label { Content = (obj ?? "Value is not specified or null").ToString() };
            panel.Children.Add(label);
        }

        public abstract bool IsMatch(Type type);

        public BaseWpfValueFiller GetInstance()
        {
            return (BaseWpfValueFiller)Activator.CreateInstance(GetType());
        }
    }
}
