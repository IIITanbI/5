namespace QA.AutomatedMagic.WpfManagingFillers.WpfValueFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;

    public class StringFiller : BaseWpfValueFiller
    {
        public override bool IsMatch(Type type)
        {
            return type == typeof(string);
        }

        public override void FillCreateControls(StackPanel panel, Type type)
        {
            var textBlock = new TextBox { Text = "" };
            panel.Children.Add(textBlock);

            GetValue = () => textBlock.Text;
        }

        public override void FillEditControls(StackPanel panel, object obj, Type type)
        {
            var textBlock = new TextBox { Text = obj?.ToString() };
            panel.Children.Add(textBlock);

            GetValue = () => textBlock.Text;
            SetValue = val => textBlock.Text = val.ToString();
        }
    }
}
