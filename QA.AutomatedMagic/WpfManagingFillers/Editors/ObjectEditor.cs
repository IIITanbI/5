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

    public class ObjectEditor : BaseEditor
    {
        private MetaTypeObjectMember _objectMember;
        private object _parentObj;
        private Button _editBtn;
        private Button _saveBtn;
        private Button _cancelBtn;
        private Window _editWindow;
        private object _obj;
        private MetaType _metaType;
        private string _name = null;

        public ObjectEditor(Panel buttonsPanel, MetaTypeObjectMember objectMember, object parentObj)
        {
            _objectMember = objectMember;
            _parentObj = parentObj;

            _editBtn = new Button { Content = "Edit" };
            buttonsPanel.Children.Add(_editBtn);
            _editBtn.Click += _editBtn_Click;
            _savedValue = MetaType.CopyObject(_objectMember.GetValue(_parentObj));
        }

        public ObjectEditor(Panel buttonsPanel, MetaType metaType, object obj, string name)
        {
            _editBtn = new Button { Content = "Edit" };
            buttonsPanel.Children.Add(_editBtn);

            _name = name;
            _editBtn.Click += _editBtn_Click;
            _savedValue = MetaType.CopyObject(obj);
            _obj = obj;
            _metaType = metaType;
        }

        private void _editBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_objectMember != null)
            {
                _savedValue = MetaType.CopyObject(_objectMember.GetValue(_parentObj));
                _editedValue = _objectMember.GetValue(_parentObj);
            }
            else
            {
                _savedValue = MetaType.CopyObject(_obj);
                _editedValue = _obj;
            }

            _editWindow = new Window();

            var scrollViewer = new ScrollViewer();
            _editWindow.Content = scrollViewer;
            var rootStackPanel = new StackPanel();
            scrollViewer.Content = rootStackPanel;

            var infoLabel = new Label { Content = $"Editing : {_name ?? _objectMember.Info.Name}" };
            rootStackPanel.Children.Add(infoLabel);

            var buttonsPanel = new WrapPanel();
            rootStackPanel.Children.Add(buttonsPanel);

            _saveBtn = new Button { Content = "Save" };
            buttonsPanel.Children.Add(_saveBtn);
            _saveBtn.Click += _saveBtn_Click;

            _cancelBtn = new Button { Content = "Cancel" };
            buttonsPanel.Children.Add(_cancelBtn);
            _cancelBtn.Click += _cancelBtn_Click;

            var metaType = _objectMember?.MemberMetaType.Value ?? _metaType;
            foreach (var metaTypeMember in metaType.Members)
            {
                var valueMember = metaTypeMember as MetaTypeValueMember;
                if (valueMember != null)
                {
                    valueMember.ManagingValueFiller.FillEditControls(rootStackPanel, _editedValue, valueMember);
                }

                var collectionMember = metaTypeMember as MetaTypeCollectionMember;
                if (collectionMember != null)
                {
                    collectionMember.ManagingCollectionFiller.FillEditControls(rootStackPanel, _editedValue, collectionMember);
                }

                var objectMember1 = metaTypeMember as MetaTypeObjectMember;
                if (objectMember1 != null)
                {
                    objectMember1.MemberMetaType.Value.ManagingFiller.GetManagingObjectFiller().FillEditControls(rootStackPanel, _editedValue, objectMember1);
                }
            }

            _editWindow.ShowDialog();
        }

        protected override void _cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            base._cancelBtn_Click(sender, e);

            _editWindow.Close();
            _objectMember?.SetValue(_parentObj, _savedValue);
        }

        protected override void _saveBtn_Click(object sender, RoutedEventArgs e)
        {
            base._saveBtn_Click(sender, e);

            _editWindow.Close();
            _objectMember?.SetValue(_parentObj, _editedValue);
        }
    }
}
