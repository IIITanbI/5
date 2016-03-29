namespace QA.AutomatedMagic.Framework.UI.ReportViewer
{
    using QA.AutomatedMagic.TestInfo;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;


    /// <summary>
    /// Interaction logic for ReportViewer.xaml
    /// </summary>
    public partial class ReportViewer : UserControl
    {
        private Image GetImageForStatus(TestItemStatus status)
        {
            var image = new Image();
            Uri uri = null;
            switch (status)
            {
                case TestItemStatus.NotExecuted:
                    uri = new Uri("Resources/NotExecuted.png", UriKind.Relative);
                    break;
                case TestItemStatus.Unknown:
                    uri = new Uri("Resources/Unknown.png", UriKind.Relative);
                    break;
                case TestItemStatus.Passed:
                    uri = new Uri("Resources/Passed.png", UriKind.Relative);
                    break;
                case TestItemStatus.Failed:
                    uri = new Uri("Resources/Failed.png", UriKind.Relative);
                    break;
                case TestItemStatus.Skipped:
                    uri = new Uri("Resources/Skipped.png", UriKind.Relative);
                    break;
                default:
                    break;
            }
            var bi = new BitmapImage(uri);
            image.Source = bi;
            image.Height = 15;
            return image;
        }
        private WrapPanel GetWrapPanelWithStatusImg(TestItemStatus status)
        {
            var wrapPanel = new WrapPanel();
            wrapPanel.Children.Add(GetImageForStatus(status));
            return wrapPanel;
        }

        public ReportViewer()
        {
            InitializeComponent();

            InfoTree.SelectedItemChanged += InfoTree_SelectedItemChanged;
        }

        private TreeViewItem _selectedNode = null;
        private void InfoTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var tree = (TreeView)sender;
            var selectedItem = (TreeViewItem)tree.SelectedItem;
            if (_selectedNode != selectedItem)
            {
                _selectedNode = selectedItem;

                InfoContentPanel.Children.Clear();

                var messages = _selectedNode.Tag as TestItem != null
                    ? ((TestItem)_selectedNode.Tag).LogMessages
                    : ((Step)_selectedNode.Tag).Messages;

                InfoContentPanel.Children.Add(new Label { Content = $"Message count: {messages?.Count ?? 0}" });

                foreach (var message in messages)
                {
                    var sp = new StackPanel { Tag = message };
                    var infoPanel = new WrapPanel();
                    infoPanel.Children.Add(new Label { Content = message.Level, Width = 50 });
                    infoPanel.Children.Add(new Label { Content = message.DataStemp, Width = 100 });
                    sp.Children.Add(infoPanel);

                    var messagePanel = new WrapPanel();
                    messagePanel.Children.Add(new Label { Content = "Message", Width = 80 });
                    messagePanel.Children.Add(new TextBox { Text = message.Message, IsReadOnly = true });
                    sp.Children.Add(messagePanel);

                    if (message as LogMessage != null)
                    {
                        var extext = ((LogMessage)message).ExceptionString;
                        if (extext != null && extext != "")
                        {
                            messagePanel = new WrapPanel();
                            messagePanel.Children.Add(new Label { Content = "Exception", Width = 80 });
                            messagePanel.Children.Add(new TextBox { Text = extext, IsReadOnly = true });
                            sp.Children.Add(messagePanel);
                        }
                    }
                    else
                    {

                    }

                    InfoContentPanel.Children.Add(sp);
                }
            }

            e.Handled = true;
        }

        private void InfoNode_Expanded(object sender, RoutedEventArgs e)
        {
            var infoNode = (TreeViewItem)sender;
            var testItem = infoNode.Tag;

            if (infoNode.Items.Count == 1)
            {
                if (infoNode.Items.Contains("*"))
                {
                    var wp = (WrapPanel)infoNode.Header;

                    if (((Label)wp.Children[1]).Content.ToString() == "Steps")
                    {
                        infoNode.Items.Clear();
                        FillSteps((TestItem)testItem, infoNode);
                    }
                    else
                    {
                        var nodeType = testItem.GetType();
                        if (nodeType == typeof(TestItem))
                        {
                            infoNode.Items.Clear();
                            FillItem((TestItem)testItem, infoNode);
                        }
                        if (nodeType == typeof(Step))
                        {
                            infoNode.Items.Clear();
                            FillSteps((Step)testItem, infoNode);
                        }
                    }
                }
            }

            e.Handled = true;
        }

        public void AddTestItem(TestItem testItem)
        {
            var wrapPanel = GetWrapPanelWithStatusImg(testItem.Status);
            wrapPanel.Children.Add(new Label { Content = $"{testItem.Type}: {testItem.Name}" });
            var root = new TreeViewItem { Header = wrapPanel, Tag = testItem };

            root.Expanded += InfoNode_Expanded;
            InfoTree.Items.Add(root);
            root.Items.Add("*");
        }

        private void FillItem(TestItem testItem, TreeViewItem node)
        {
            if (testItem.Steps != null && testItem.Steps.Count > 0)
            {
                var status = testItem.Steps.Any(s => s.Status == TestItemStatus.Failed)
                    ? TestItemStatus.Failed
                    : testItem.Steps.All(s => s.Status == TestItemStatus.Failed)
                        ? TestItemStatus.Skipped
                        : testItem.Steps.All(s => s.Status == TestItemStatus.Passed)
                            ? TestItemStatus.Passed
                            : TestItemStatus.Unknown;

                var wrapPanel = GetWrapPanelWithStatusImg(status);
                wrapPanel.Children.Add(new Label { Content = $"Steps" });
                var stepsNode = new TreeViewItem { Header = wrapPanel, Tag = testItem };
                node.Items.Add(stepsNode);
                stepsNode.Expanded += InfoNode_Expanded;
                stepsNode.Items.Add("*");
            }

            if (testItem.Childs != null && testItem.Childs.Count > 0)
            {
                foreach (var child in testItem.Childs)
                {
                    var wrapPanel = GetWrapPanelWithStatusImg(child.Status);
                    wrapPanel.Children.Add(new Label { Content = $"{child.Type}: {child.Name}" });
                    var childNode = new TreeViewItem { Header = wrapPanel, Tag = child };
                    childNode.Expanded += InfoNode_Expanded;
                    node.Items.Add(childNode);
                    childNode.Items.Add("*");
                }
            }
        }

        private void FillSteps(TestItem testItem, TreeViewItem node)
        {
            foreach (var testStep in testItem.Steps)
            {
                var wrapPanel = GetWrapPanelWithStatusImg(testStep.Status);
                wrapPanel.Children.Add(new Label { Content = $"{testStep.GetType().Name}: {testStep.Name}" });
                var childNode = new TreeViewItem { Header = wrapPanel, Tag = testStep };
                childNode.Expanded += InfoNode_Expanded;
                node.Items.Add(childNode);
                if (testStep.Steps != null && testStep.Steps.Count > 0)
                    childNode.Items.Add("*");
            }
        }

        private void FillSteps(Step step, TreeViewItem node)
        {
            foreach (var testStep in step.Steps)
            {
                var wrapPanel = GetWrapPanelWithStatusImg(testStep.Status);
                wrapPanel.Children.Add(new Label { Content = $"{testStep.GetType().Name}: {testStep.Name}" });
                var childNode = new TreeViewItem { Header = wrapPanel, Tag = testStep };
                childNode.Expanded += InfoNode_Expanded;
                node.Items.Add(childNode);
                if (testStep.Steps != null && testStep.Steps.Count > 0)
                    childNode.Items.Add("*");
            }
        }
    }
}
