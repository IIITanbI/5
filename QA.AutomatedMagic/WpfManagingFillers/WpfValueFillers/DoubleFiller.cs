namespace QA.AutomatedMagic.WpfManagingFillers.WpfValueFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Xceed.Wpf.Toolkit;


    public class DoubleFiller : BaseWpfValueFiller
    {
        public override void FillCreateControls(StackPanel panel, Type type)
        {
            var doubleUpDown = new DoubleUpDown();
            panel.Children.Add(doubleUpDown);

            GetValue = () => doubleUpDown.Value.GetValueOrDefault();
        }

        public override void FillEditControls(StackPanel panel, object obj, Type type)
        {
            var doubleUpDown = new DoubleUpDown { Value = (double)obj };
            panel.Children.Add(doubleUpDown);

            GetValue = () => doubleUpDown.Value.GetValueOrDefault();
            SetValue = val => doubleUpDown.Value = (double)val;
        }

        public override bool IsMatch(Type type)
        {
            return type == typeof(double);
        }
    }
}
