namespace QA.AutomatedMagic.WpfManagingFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    public class BaseEditor
    {
        protected object _savedValue;
        protected object _editedValue;
        private bool _isCanceled = true;

        protected virtual void _saveBtn_Click(object sender, RoutedEventArgs e)
        {
            _isCanceled = false;
        }

        protected virtual void _cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            _isCanceled = true;
        }

        public Func<object> GetGet()
        {
            return () => _isCanceled
                ? _savedValue
                : _editedValue;
        }
    }
}
