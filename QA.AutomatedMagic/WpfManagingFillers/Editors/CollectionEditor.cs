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
        private List<object> _children;
        private StackPanel _childrenStackPanel;
        private object _savedValue;


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
            _savedValue = _collectionMember.GetValue(MetaType.CopyObject(_parentObj));

            _editWindow = new Window();

            var scrollViewer = new ScrollViewer();
            _editWindow.Content = scrollViewer;
            var rootStackPanel = new StackPanel();
            scrollViewer.Content = rootStackPanel;

            var infoLabel = new Label { Content = $"{_collectionMember.Info.Name} : {_collectionMember.CollectionWrapper.GetCollectionType()} of {(_collectionMember.ChildrenMetaType?.Value.TargetType ?? _collectionMember.ChildrenType).Name}";
            rootStackPanel.Children.Add(infoLabel);

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

            _childrenStackPanel = new StackPanel();
            rootStackPanel.Children.Add(_childrenStackPanel);

            var collectionObj = _collectionMember.GetValue(_parentObj);
            _children = _collectionMember.CollectionWrapper.GetChildren(collectionObj);

            var counter = 1;

            if (_collectionMember.ChildrenMetaType == null)
            {
                var managingValueFiller = _collectionMember.ParentType.ManagingFiller.GetManagingValueFiller();

                foreach (var obj in _children)
                {
                    StackPanel childStackPanel = CreateChildStackPanel(counter, obj);

                    _childrenStackPanel.Children.Add(childStackPanel);

                    managingValueFiller.FillEditControls(childStackPanel, obj, $"Item: {counter++}");
                }
            }
            else
            {
                var managingObjectFiller = _collectionMember.ChildrenMetaType.Value.ManagingFiller.GetManagingObjectFiller();
                foreach (var obj in _children)
                {
                    StackPanel childStackPanel = CreateChildStackPanel(counter, obj);

                    managingObjectFiller.FillEditControls(childStackPanel, obj, _collectionMember.ChildrenMetaType.Value, $"Item: {counter++}", _collectionMember.IsAssignableTypesAllowed);
                }
            }

            _editWindow.ShowDialog();
        }

        private StackPanel CreateChildStackPanel(int counter, object obj)
        {
            var childStackPanel = new StackPanel();
            var childButtonsWrapPanel = new WrapPanel();
            childStackPanel.Children.Add(childButtonsWrapPanel);

            var removeBtn = new Button { Content = "Remove" + counter };
            childButtonsWrapPanel.Children.Add(removeBtn);
            removeBtn.Click += RemoveBtn_Click;
            removeBtn.Tag = childStackPanel;

            if (counter > 1)
            {
                var upBtn = new Button { Content = "Up" };
                childButtonsWrapPanel.Children.Add(upBtn);
                upBtn.Click += UpBtn_Click;
                upBtn.Tag = childStackPanel;
            }

            if (counter < _children.Count)
            {
                var downBtn = new Button { Content = "Down" + obj };
                childButtonsWrapPanel.Children.Add(downBtn);
                downBtn.Click += DownBtn_Click;
                downBtn.Tag = childStackPanel;
            }

            return childStackPanel;
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var childPanel = (StackPanel)btn.Tag;

            _children.RemoveAt(_childrenStackPanel.Children.IndexOf(childPanel));
            _childrenStackPanel.Children.Remove(childPanel);

            e.Handled = true;
        }
        private void DownBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var childPanel = (StackPanel)btn.Tag;
            var counter = (int)childPanel.Tag;

            var index = _childrenStackPanel.Children.IndexOf(childPanel);
            var tmp1 = _childrenStackPanel.Children[index - 1];
            _childrenStackPanel.Children[index - 1] = _childrenStackPanel.Children[index];
            _childrenStackPanel.Children[index] = tmp1;

            var tmp2 = _children[index - 1];
            _children[index - 1] = _children[index];
            _children[index] = tmp2;

            e.Handled = true;
        }
        private void UpBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var childPanel = (StackPanel)btn.Tag;
            var counter = (int)childPanel.Tag;

            var index = _childrenStackPanel.Children.IndexOf(childPanel);
            var tmp1 = _childrenStackPanel.Children[index + 1];
            _childrenStackPanel.Children[index + 1] = _childrenStackPanel.Children[index];
            _childrenStackPanel.Children[index] = tmp1;

            var tmp2 = _children[index + 1];
            _children[index + 1] = _children[index];
            _children[index] = tmp2;

            e.Handled = true;
        }

        private void _cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            _editWindow.Close();
            _collectionMember.SetValue(_parentObj, _savedValue);
        }
        private void _saveBtn_Click(object sender, RoutedEventArgs e)
        {
            _editWindow.Close();

            var collection = _collectionMember.CollectionWrapper.CreateNew(_collectionMember.ChildrenType, null);
            foreach (var child in _children)
                _collectionMember.CollectionWrapper.Add(collection, child, _collectionMember.ChildrenType);

            _collectionMember.SetValue(_parentObj, collection);
        }

        private void _createBtn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
