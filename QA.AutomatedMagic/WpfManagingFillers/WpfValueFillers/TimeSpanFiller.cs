namespace QA.AutomatedMagic.WpfManagingFillers.WpfValueFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Xceed.Wpf.Toolkit;


    public class TimeSpanFiller : BaseWpfValueFiller
    {
        public override void FillCreateControls(StackPanel panel, Type type)
        {
            var timSpanUpDown = new TimeSpanUpDown { Value = TimeSpan.FromDays(0) };
            panel.Children.Add(timSpanUpDown);

            GetValue = () => timSpanUpDown.Value.GetValueOrDefault();
        }

        public override void FillEditControls(StackPanel panel, object obj, Type type)
        {
            var timSpanUpDown = new TimeSpanUpDown { Value = (TimeSpan)obj };
            panel.Children.Add(timSpanUpDown);

            GetValue = () => timSpanUpDown.Value.GetValueOrDefault();
            SetValue = val => timSpanUpDown.Value = (TimeSpan)val;
        }

        public override bool IsMatch(Type type)
        {
            return type == typeof(TimeSpan);
        }
    }
}
