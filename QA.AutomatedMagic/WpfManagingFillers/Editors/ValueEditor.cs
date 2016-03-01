namespace QA.AutomatedMagic.WpfManagingFillers.Editors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using MetaMagic;

    public class ValueEditor
    {
        private Button _editBtn;
        private Button _saveBtn;
        private Button _cancelBtn;
        private BaseWpfValueFiller _filler;
        private StackPanel _container;
        private object _savedValue;
        private MetaTypeValueMember _valueMember;
        private object _parentObj;

        public ValueEditor(Panel buttonsContainer, BaseWpfValueFiller filler, StackPanel container, MetaTypeValueMember valueMember, object parentObj)
        {
            _valueMember = valueMember;
            _parentObj = parentObj;
            
            _editBtn = new Button { Content = "Edit" };
            buttonsContainer.Children.Add(_editBtn);
            _saveBtn = new Button { Content = "Save" };
            buttonsContainer.Children.Add(_saveBtn);
            _cancelBtn = new Button { Content = "Cancel" };
            buttonsContainer.Children.Add(_cancelBtn);
            
            _editBtn.Click += _editBtn_Click;
            _saveBtn.Visibility = Visibility.Hidden;
            _saveBtn.Click += _saveBtn_Click;
            _cancelBtn.Visibility = Visibility.Hidden;
            _cancelBtn.Click += _cancelBtn_Click;

            _container = container;
            _container.IsEnabled = false;

            _filler = filler;
            _savedValue = filler.GetValue();
        }

        public ValueEditor(Panel buttonsContainer, BaseWpfValueFiller filler, StackPanel container, object obj)
        {
            _editBtn = new Button { Content = "Edit" };
            buttonsContainer.Children.Add(_editBtn);
            _saveBtn = new Button { Content = "Save" };
            buttonsContainer.Children.Add(_saveBtn);
            _cancelBtn = new Button { Content = "Cancel" };
            buttonsContainer.Children.Add(_cancelBtn);

            _editBtn.Click += _editBtn_Click;
            _saveBtn.Visibility = Visibility.Hidden;
            _saveBtn.Click += _saveBtn_Click;
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

            if (_savedValue != _filler.GetValue())
                _filler.SetValue(_savedValue);
        }

        private void _saveBtn_Click(object sender, RoutedEventArgs e)
        {
            _editBtn.Visibility = Visibility.Visible;
            _saveBtn.Visibility = Visibility.Hidden;
            _cancelBtn.Visibility = Visibility.Hidden;
            _container.IsEnabled = false;

            _valueMember.SetValue(_parentObj, _filler.GetValue());
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
