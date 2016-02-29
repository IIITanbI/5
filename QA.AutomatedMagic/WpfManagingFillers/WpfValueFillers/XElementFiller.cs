namespace QA.AutomatedMagic.WpfManagingFillers.WpfValueFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Xml.Linq;

    public class XElementFiller : BaseWpfValueFiller
    {
        public override void FillCreateControls(StackPanel panel, Type type)
        {
            var textBox = new TextBox();
            panel.Children.Add(textBox);

            GetValue = () => XElement.Parse(textBox.Text);
        }

        public override void FillEditControls(StackPanel panel, object obj, Type type)
        {
            var textBox = new TextBox { Text = obj?.ToString() };
            panel.Children.Add(textBox);

            GetValue = () => XElement.Parse(textBox.Text);
        }

        public override bool IsMatch(Type type)
        {
            return type == typeof(XElement);
        }
    }
}
