namespace QA.AutomatedMagic.WpfManagingFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using MetaMagic;
    using System.Windows;

    public class Creator
    {
        private MetaTypeObjectMember _objectMember;
        private MetaTypeCollectionMember _collectionMember;
        private Panel _headerPanel;
        private object _parentObj;
        private Button _createButton;
        private Button _cancelBtn;
        private Button _createNewBtn;
        private Window _window;
        private string _name = null;
        public object CreatedObject = null;
        private MetaType _metaType;
        private bool _isAssignableTypesAllowed;
        private Window _selectTypeWindow;
        private MetaType _selectedType;

        public Creator(Panel headerPanel, MetaTypeObjectMember objectMember, object parentObj)
        {
            _headerPanel = headerPanel;
            _objectMember = objectMember;
            _parentObj = parentObj;

            _createButton = new Button { Content = "Create" };
            _headerPanel.Children.Add(_createButton);
            _createButton.Click += _createButton_Click;

            _metaType = objectMember.MemberMetaType.Value;
            _isAssignableTypesAllowed = objectMember.IsAssignableTypesAllowed;
        }

        public Creator(Panel headerPanel, MetaTypeCollectionMember collectionMember, object parentObj)
        {
            _headerPanel = headerPanel;
            _collectionMember = collectionMember;
            _parentObj = parentObj;

            _createButton = new Button { Content = "Create" };
            _headerPanel.Children.Add(_createButton);
            _createButton.Click += _createButton_Click;

            _metaType = collectionMember.ChildrenMetaType?.Value;
            _isAssignableTypesAllowed = collectionMember.IsAssignableTypesAllowed;
        }

        public Creator(Button createButton, MetaType metaType, string name, bool isAssignableTypesAllowed)
        {
            _isAssignableTypesAllowed = isAssignableTypesAllowed;
            _name = name;
            _metaType = metaType;
            _createButton = createButton;
            _createButton.Click += _createButton_Click;
        }

        private void _createButton_Click(object sender, RoutedEventArgs e)
        {
            if (_objectMember != null && _metaType != null && _isAssignableTypesAllowed)
            {
                if (_metaType.AssignableTypes.Count == 1)
                    _selectedType = _metaType.AssignableTypes[0];
                else
                {
                    _selectTypeWindow = new Window();
                    var sStackPanel = new StackPanel();
                    _selectTypeWindow.Content = sStackPanel;
                    var assignableTypesComboBox = new ComboBox();
                    sStackPanel.Children.Add(assignableTypesComboBox);

                    foreach (var aType in _metaType.AssignableTypes)
                    {
                        assignableTypesComboBox.Items.Add(aType);
                    }
                    assignableTypesComboBox.SelectionChanged += AssignableTypesComboBox_SelectionChanged;
                    assignableTypesComboBox.SelectedIndex = 0;

                    var bwp = new WrapPanel();
                    sStackPanel.Children.Add(bwp);

                    var cancelSelectButton = new Button { Content = "Cancel" };
                    bwp.Children.Add(cancelSelectButton);
                    cancelSelectButton.Click += CancelSelectButton_Click;

                    var okBtn = new Button { Content = "OK" };
                    bwp.Children.Add(okBtn);
                    okBtn.Click += OkBtn_Click;

                    _selectTypeWindow.ShowDialog();

                    if (_selectedType == null)
                        return;

                    _metaType = _selectedType;
                }
            }

            _window = new Window();

            var scrollViewer = new ScrollViewer();
            _window.Content = scrollViewer;
            var rootStackPanel = new StackPanel();
            scrollViewer.Content = rootStackPanel;

            var infoLabel = new Label { Content = $"Creating : {_name ?? _objectMember?.Info.Name ?? _collectionMember?.Info.Name}" };
            rootStackPanel.Children.Add(infoLabel);

            var buttonsPanel = new WrapPanel();
            rootStackPanel.Children.Add(buttonsPanel);

            _createNewBtn = new Button { Content = "Create" };
            buttonsPanel.Children.Add(_createNewBtn);
            _createNewBtn.Click += _createNewBtn_Click;

            _cancelBtn = new Button { Content = "Cancel creation" };
            buttonsPanel.Children.Add(_cancelBtn);
            _cancelBtn.Click += _cancelBtn_Click;

            var creationStackPanel = new StackPanel();
            rootStackPanel.Children.Add(creationStackPanel);

            if (_collectionMember != null)
            {
                CreatedObject = _collectionMember.CollectionWrapper.CreateNew(_collectionMember.ChildrenType, null);
                _collectionMember.SetValue(_parentObj, CreatedObject);
                _collectionMember.ManagingCollectionFiller.FillEditControls(creationStackPanel, _parentObj, _collectionMember);
            }
            else
            {
                CreatedObject = Activator.CreateInstance(_metaType.TargetType);

                foreach (var metaTypeMember in _metaType.Members)
                {
                    var valueMember = metaTypeMember as MetaTypeValueMember;
                    if (valueMember != null)
                    {
                        valueMember.ManagingValueFiller.FillEditControls(creationStackPanel, CreatedObject, valueMember);
                    }

                    var collectionMember = metaTypeMember as MetaTypeCollectionMember;
                    if (collectionMember != null)
                    {
                        collectionMember.ManagingCollectionFiller.FillEditControls(creationStackPanel, CreatedObject, collectionMember);
                    }

                    var objectMember1 = metaTypeMember as MetaTypeObjectMember;
                    if (objectMember1 != null)
                    {
                        objectMember1.MemberMetaType.Value.ManagingFiller.GetManagingObjectFiller().FillEditControls(creationStackPanel, CreatedObject, objectMember1);
                    }
                }
            }

            _window.ShowDialog();
        }

        private void CancelSelectButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedType = null;
            CreatedObject = null;
            _selectTypeWindow.Close();
        }
        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            _selectTypeWindow.Close();
        }

        private void AssignableTypesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox)sender;
            _selectedType = (MetaType)cb.SelectedItem;
        }

        private void _cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            CreatedObject = null;
            _collectionMember?.SetValue(_parentObj, CreatedObject);
            _objectMember?.SetValue(_parentObj, CreatedObject);
            _window.Close();
        }

        private void _createNewBtn_Click(object sender, RoutedEventArgs e)
        {
            _objectMember?.SetValue(_parentObj, CreatedObject);
            _window.Close();
        }
    }
}
