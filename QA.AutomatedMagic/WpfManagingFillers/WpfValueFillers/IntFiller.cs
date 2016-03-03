namespace QA.AutomatedMagic.WpfManagingFillers.WpfValueFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Xceed.Wpf.Toolkit;

    public class IntFiller : BaseWpfValueFiller
    {
        public override void FillCreateControls(StackPanel panel, Type type)
        {
            var intUpDown = new IntegerUpDown { Value = 0 };
            panel.Children.Add(intUpDown);

            GetValue = () => intUpDown.Value.GetValueOrDefault();
        }


        public override void FillEditControls(StackPanel panel, object obj, Type type)
        {
            var intUpDown = new IntegerUpDown { Value = (int)obj };
            panel.Children.Add(intUpDown);

            GetValue = () => intUpDown.Value.GetValueOrDefault();
            SetValue = val => intUpDown.Value = (int)val;
        }

        public override bool IsMatch(Type type)
        {
            return type == typeof(int);
        }
    }
}
