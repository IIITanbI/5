namespace QA.AutomatedMagic.WpfManagingFillers.Editors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using MetaMagic;
    using System.Windows;

    public class CollectionEditor
    {
        private Button _editBtn;
        private MetaTypeCollectionMember _collectionMember;
        private object _parentObj;
        private Window _editWindow;
        private Button _createBtn;
        private Button _saveBtn;
        private Button _cancelBtn;

        public CollectionEditor(Panel buttonsPanel, MetaTypeCollectionMember collectionMember, object parentObj)
        {
            _editBtn = new Button { Content = "Edit" };
            buttonsPanel.Children.Add(_editBtn);
            _editBtn.Click += _editBtn_Click;

            _collectionMember = collectionMember;
            _parentObj = parentObj;
        }

        private void _editBtn_Click(object sender, RoutedEventArgs e)
        {
            _editWindow = new Window();

            var scrollViewer = new ScrollViewer();
            _editWindow.Content = scrollViewer;
            var rootStackPanel = new StackPanel();
            scrollViewer.Content = rootStackPanel;

            var buttonsPanel = new WrapPanel();
            rootStackPanel.Children.Add(buttonsPanel);

            _saveBtn = new Button { Content = "Save" };
            buttonsPanel.Children.Add(_saveBtn);
            _saveBtn.Click += _saveBtn_Click;

            _cancelBtn = new Button { Content = "Cancel" };
            buttonsPanel.Children.Add(_cancelBtn);
            _cancelBtn.Click += _cancelBtn_Click;

            _createBtn = new Button { Content = "Create new" };
            buttonsPanel.Children.Add(_createBtn);
            _createBtn.Click += _createBtn_Click;

            var collectionObj = _collectionMember.GetValue(_parentObj);
            var children = _collectionMember.CollectionWrapper.GetChildren(collectionObj);

            var counter = 1;
            if (_collectionMember.ChildrenMetaType == null)
            {
                var managingValueFiller = _collectionMember.ParentType.ManagingFiller.GetManagingValueFiller();

                foreach (var obj in children)
                {
                    managingValueFiller.FillInfoControls(rootStackPanel, obj, $"Item: {counter++}");
                }
            }
            else
            {
                var managingObjectFiller = _collectionMember.ChildrenMetaType.Value.ManagingFiller.GetManagingObjectFiller();
                foreach (var obj in children)
                {
                    managingObjectFiller.FillInfoControls(rootStackPanel, obj, _collectionMember.ChildrenMetaType.Value, $"Item: {counter++}", _collectionMember.IsAssignableTypesAllowed);
                }
            }
        }

        private void _cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _saveBtn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _createBtn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
