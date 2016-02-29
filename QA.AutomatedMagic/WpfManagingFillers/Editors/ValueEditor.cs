namespace QA.AutomatedMagic.WpfManagingFillers.Editors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;

    public class ValueEditor
    {
        private Button _editBtn;
        private Button _saveBtn;
        private Button _cancelBtn;
        private BaseWpfValueFiller _filler;
        private StackPanel _container;
        private object _savedValue;

        public ValueEditor(Button editBtn, Button saveBtn, Button cancelBtn, BaseWpfValueFiller filler, StackPanel container)
        {
            _editBtn = editBtn;
            _editBtn.Click += _editBtn_Click;

            _saveBtn = saveBtn;
            _saveBtn.Visibility = Visibility.Hidden;
            _saveBtn.Click += _saveBtn_Click;

            _cancelBtn = cancelBtn;
            _cancelBtn.Visibility = Visibility.Hidden;
            _cancelBtn.Click += _cancelBtn_Click;

            _container = container;
            _container.IsEnabled = false;

            _filler = filler;
            _savedValue = filler.GetValue();
        }

        private void _cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            _editBtn.Visibility = Visibility.Visible;
            _saveBtn.Visibility = Visibility.Hidden;
            _cancelBtn.Visibility = Visibility.Hidden;
            _container.IsEnabled = false;
        }

        private void _saveBtn_Click(object sender, RoutedEventArgs e)
        {
            _editBtn.Visibility = Visibility.Visible;
            _saveBtn.Visibility = Visibility.Hidden;
            _cancelBtn.Visibility = Visibility.Hidden;
            _container.IsEnabled = false;

        }

        private void _editBtn_Click(object sender, RoutedEventArgs e)
        {
            _editBtn.Visibility = Visibility.Hidden;
            _saveBtn.Visibility = Visibility.Visible;
            _cancelBtn.Visibility = Visibility.Visible;
            _container.IsEnabled = true;
        }
    }
}
