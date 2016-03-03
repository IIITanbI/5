namespace QA.AutomatedMagic.WpfManagingFillers.WpfValueFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Xceed.Wpf.Toolkit;

    public class ByteFiller : BaseWpfValueFiller
    {
        public override void FillCreateControls(StackPanel panel, Type type)
        {
            var byteDownUp = new ByteUpDown();
            panel.Children.Add(byteDownUp);

            GetValue = () => byteDownUp.Value.GetValueOrDefault();
        }

        public override void FillEditControls(StackPanel panel, object obj, Type type)
        {
            var byteDownUp = new ByteUpDown { Value = (byte)obj };
            panel.Children.Add(byteDownUp);

            GetValue = () => byteDownUp.Value.GetValueOrDefault();
            SetValue = val => byteDownUp.Value = (byte)val;
        }

        public override bool IsMatch(Type type)
        {
            return type == typeof(byte);
        }
    }
}
