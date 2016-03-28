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
        public ReportViewer()
        {
            InitializeComponent();
        }

        private void InfoNode_Expanded(object sender, RoutedEventArgs e)
        {
            var infoNode = (TreeViewItem)sender;
            var testItem = (TestItem)infoNode.Tag;

            if (infoNode.Items.Count == 1)
            {
                if (infoNode.Items.Contains("*"))
                {
                    infoNode.Items.Clear();
                    FillItem(testItem, infoNode);
                }
            }

            e.Handled = true;
        }

        public void AddTestItem(TestItem testItem)
        {
            var root = new TreeViewItem { Header = $"{testItem.Type}: {testItem.Name}", Tag = testItem };
            root.Expanded += InfoNode_Expanded;
            InfoTree.Items.Add(root);
            root.Items.Add("*");
        }

        private void FillItem(TestItem testItem, TreeViewItem node)
        {
            if (testItem.Steps != null && testItem.Steps.Count > 0)
            {
                var stepsNode = new TreeViewItem { Header = "Steps" };
                node.Items.Add(stepsNode);
                stepsNode.Items.Add("*");
            }

            if (testItem.Childs != null && testItem.Childs.Count > 0)
            {
                foreach (var child in testItem.Childs)
                {
                    var childNode = new TreeViewItem { Header = $"{child.Type}: {child.Name}", Tag = child };
                    childNode.Expanded += InfoNode_Expanded;
                    node.Items.Add(childNode);
                    childNode.Items.Add("*");
                }
            }
        }

        private void FillSteps(TestItem testItem, TreeViewItem node)
        {
        }
    }
}
