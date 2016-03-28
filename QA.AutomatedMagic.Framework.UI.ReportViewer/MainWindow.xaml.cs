namespace QA.AutomatedMagic.Framework.UI.ReportViewer
{
    using Microsoft.Win32;
    using QA.AutomatedMagic.MetaMagic;
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
    using System.Windows.Shapes;
    using System.Xml.Linq;


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            AutomatedMagicManager.LoadAssemblies();
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Report Files (*.xml)|*.xml";
            if (ofd.ShowDialog() == true)
            {
                var doc = XDocument.Load(ofd.FileName);

                try
                {
                    var testItem = MetaType.Parse<TestItem>(doc.Elements().First());
                    LoadTestItem(testItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void LoadTestItem(TestItem testItem)
        {
            RViewer.AddTestItem(testItem);
        }

        private void ReportsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lb = (ListBox)sender;
            var testItem = lb.SelectedItem;
            e.Handled = true;
        }
    }
}
