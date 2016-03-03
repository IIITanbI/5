namespace QA.AutomatedMagic.WpfManagingFillers.WpfValueFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Xceed.Wpf.Toolkit;

    public class DateTimeFiller : BaseWpfValueFiller
    {
        public override void FillCreateControls(StackPanel panel, Type type)
        {
            var dateTimeUpDown = new DateTimeUpDown();
            panel.Children.Add(dateTimeUpDown);

            GetValue = () => dateTimeUpDown.Value.GetValueOrDefault();
        }

        public override void FillEditControls(StackPanel panel, object obj, Type type)
        {
            var dateTimeUpDown = new DateTimeUpDown { Value = (DateTime)obj };
            panel.Children.Add(dateTimeUpDown);

            GetValue = () => dateTimeUpDown.Value.GetValueOrDefault();
            SetValue = val => dateTimeUpDown.Value = (DateTime)val;
        }

        public override bool IsMatch(Type type)
        {
            return type == typeof(DateTime);
        }
    }
}
