namespace QA.AutomatedMagic.WpfManagingFillers.WpfValueFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;

    public class BoolFiller : BaseWpfValueFiller
    {
        public override void FillCreateControls(StackPanel panel, Type type)
        {
            var checkBox = new CheckBox { IsChecked = false };
            panel.Children.Add(checkBox);

            GetValue = () => checkBox.IsChecked.GetValueOrDefault();
        }

        public override void FillEditControls(StackPanel panel, object obj, Type type)
        {
            var checkBox = new CheckBox { IsChecked = (bool)obj };
            panel.Children.Add(checkBox);

            GetValue = () => checkBox.IsChecked.GetValueOrDefault();
            SetValue = val => checkBox.IsChecked = (bool)val;
        }

        public override bool IsMatch(Type type)
        {
            return type == typeof(bool);
        }
    }
}
