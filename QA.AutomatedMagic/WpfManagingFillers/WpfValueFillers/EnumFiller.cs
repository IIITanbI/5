namespace QA.AutomatedMagic.WpfManagingFillers.WpfValueFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;

    public class EnumFiller : BaseWpfValueFiller
    {
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        public override bool IsMatch(Type type)
        {
            return type.IsEnum;
        }

        public override void FillCreateControls(StackPanel panel, Type type)
        {
            var values = Enum.GetNames(type);
            var comboBox = new ComboBox();
            panel.Children.Add(comboBox);
            foreach (var value in values)
            {
                comboBox.Items.Add(value);
            }
            comboBox.SelectedIndex = 0;
            comboBox.SelectionChanged += ComboBox_SelectionChanged;

            GetValue = () => Enum.Parse(type, comboBox.SelectedItem.ToString());
        }

        public override void FillEditControls(StackPanel panel, object obj, Type type)
        {
            var values = Enum.GetNames(type);
            var comboBox = new ComboBox();
            panel.Children.Add(comboBox);
            foreach (var value in values)
            {
                comboBox.Items.Add(value);
            }
            comboBox.SelectedIndex = values.ToList().IndexOf(obj.ToString());
            comboBox.SelectionChanged += ComboBox_SelectionChanged;

            GetValue = () => Enum.Parse(type, comboBox.SelectedItem.ToString());
            SetValue = val => comboBox.SelectedIndex = values.ToList().IndexOf(val.ToString()); ;
        }
    }
}
