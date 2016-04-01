using QA.AutomatedMagic;
using QA.AutomatedMagic.MetaMagic;
using QA.AutomatedMagic.TestInfo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TestItem testItem { get; set; }

        public static DependencyProperty StackPanelProperty =  DependencyProperty.Register("test", typeof(int), typeof(StackPanel));

        public MainWindow()
        {
            Parse();
            InitializedComponent();
        }

        private void Parse()
        {
            try
            {
                AutomatedMagicManager.LoadAssemblies();
                AutomatedMagicManager.LoadAssemblies(Directory.GetCurrentDirectory());

                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

                var xml = XDocument.Load(@"C:\Users\Artsiom_Kuis\Desktop\QA.AutomatedMagic\Test\result(2).xml").Elements().First();
                var res = MetaType.Parse<TestItem>(xml);

                this.testItem = res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
        private List<Tuple<string, string>> EnvironmentToMap(TestEnvironmentInfo environment)
        {
            var res = new List<Tuple<string, string>>();

            res.Add(new Tuple<string, string>("CLR version" , environment.CLRVersion));
            res.Add(new Tuple<string, string>("OS name"     , environment.OSName));
            res.Add(new Tuple<string, string>("OS version"  , environment.OSVersion));
            res.Add(new Tuple<string, string>("Platform"    , environment.Platform));
            res.Add(new Tuple<string, string>("Machine name", environment.MachineName));
            res.Add(new Tuple<string, string>("User"        , environment.User));
            res.Add(new Tuple<string, string>("User domain" , environment.UserDomain));

            return res;
        }

        private Panel EnvironmentGrid;
        private void InitializedComponent()
        {
            // Новая форма
            // this.Width = this.Height = 300;
            this.Width = 500;
            this.Height = 500;


            //this.Left = this.Top = 100;
            this.Title = "Моя кнопка";
            this.Background = Brushes.Aqua;
            // Переходим к компоновке
            var panel = new Grid();
            Button Btn1 = new Button();
            Btn1.Height = Btn1.Width = 100;
            Btn1.Content = "sadasds";
            //Btn1.Margin = new Thickness(40);



            EnvironmentGrid = GetEnvironmentPanel(this.testItem.EnvironmentInfo);

            StackPanel stack = new StackPanel();
            IAddChild container = stack;

            container.AddChild(EnvironmentGrid);
            this.SizeChanged += SizeChangedEventHandler;

            var test = GetOverall(this.testItem);
            container.AddChild(test);

            var tt = GetTestInfoTable(this.testItem);
            container.AddChild(tt);

            this.AddChild(stack);
            //this.AddChild(Btn1);

            //IAddChild container = panel;
            //(panel as IAddChild).AddChild(Btn1);

            //container = this;
            //(this as IAddChild).AddChild(Btn1);

        }

        private Panel GetEnvironmentPanel(TestEnvironmentInfo environment)
        {
            StackPanel main = new StackPanel();
            main.Orientation = Orientation.Vertical;

            TextBlock header = new TextBlock();
            header.FontSize = 14;
            header.FontWeight = FontWeights.Bold;
            header.Foreground = new SolidColorBrush(Colors.Green);
            header.VerticalAlignment = VerticalAlignment.Center;
            header.HorizontalAlignment = HorizontalAlignment.Left;
            header.Padding = new Thickness(10, 0, 10, 0);
            header.Text = "Environment";


            Grid envGrid = new Grid();
            //envGrid.Width = SystemParameters.PrimaryScreenWidth;
            //envGrid.Width = 400;
           
            envGrid.HorizontalAlignment = HorizontalAlignment.Left;
            envGrid.VerticalAlignment = VerticalAlignment.Top;
            envGrid.Background = new SolidColorBrush(Colors.Black);
            envGrid.ShowGridLines = false;
            

            const int columnCount = 7;
            const int rowCount = 2;
          
            for (int i = 0; i < columnCount; i++)
            {
                var column = new ColumnDefinition();
                column.Width = GridLength.Auto;
                column.Width = new GridLength(100, GridUnitType.Star);
                envGrid.ColumnDefinitions.Add(column);
            }

            for (int i = 0; i < rowCount; i++)
            {
                RowDefinition Row = new RowDefinition();
                envGrid.RowDefinitions.Add(Row);
            }


            var environmentList = EnvironmentToMap(environment);
            for (int i = 0; i < columnCount; i++)
            {
                TextBlock columnName = new TextBlock();
                columnName.FontSize = 14;
                columnName.FontWeight = FontWeights.Bold;
                columnName.Foreground = new SolidColorBrush(Colors.Green);
                columnName.VerticalAlignment = VerticalAlignment.Center;
                columnName.HorizontalAlignment = HorizontalAlignment.Center;
                columnName.Padding = new Thickness(10, 0, 10, 0);

                columnName.Text = environmentList[i].Item1;
                Grid.SetRow(columnName, 0);
                Grid.SetColumn(columnName, i);
                envGrid.Children.Add(columnName);


                TextBlock value = new TextBlock();
                value.FontSize = 14;
                value.FontWeight = FontWeights.Normal;
                value.Foreground = new SolidColorBrush(Colors.Black);
                value.VerticalAlignment = VerticalAlignment.Center;
                value.HorizontalAlignment = HorizontalAlignment.Center;
                value.Padding = new Thickness(10, 0, 10, 0);

                value.Text = environmentList[i].Item2;
                Grid.SetRow(value, 1);
                Grid.SetColumn(value, i);
                envGrid.Children.Add(value);
            }


            IAddChild container = main;
            container.AddChild(header);
            container.AddChild(envGrid);
            //return envGrid;
            return main;
        }




        public Grid GetTestInfoTable(TestItem testItem)
        {

            Grid iTable = new Grid();

            RowDefinition row = new RowDefinition();
            iTable.RowDefinitions.Add(row);
            row = new RowDefinition();
            iTable.RowDefinitions.Add(row);


            const int columnCount = 7;

            for (int i = 0; i < columnCount; i++)
            {
                var column = new ColumnDefinition();
                column.Width = GridLength.Auto;
                //column.Width = new GridLength(100, GridUnitType.Star);
                iTable.ColumnDefinitions.Add(column);
            }


            TextBlock value = new TextBlock();
            value.FontSize = 14;
            value.FontWeight = FontWeights.Normal;
            value.Foreground = new SolidColorBrush(Colors.Black);
            value.VerticalAlignment = VerticalAlignment.Center;
            value.HorizontalAlignment = HorizontalAlignment.Center;
            value.Padding = new Thickness(10, 0, 10, 0);
            value.Text = $"{testItem.Type}: {testItem.Description}";

            TextBlock value1 = new TextBlock();
            value1.FontSize = 14;
            value1.FontWeight = FontWeights.Normal;
            value1.Foreground = new SolidColorBrush(Colors.Black);
            value1.VerticalAlignment = VerticalAlignment.Center;
            value1.HorizontalAlignment = HorizontalAlignment.Center;
            value1.Padding = new Thickness(10, 0, 10, 0);
            value1.Text = $"{testItem.Type}: {testItem.Description}";

            TextBlock value2 = new TextBlock();
            value2.FontSize = 14;
            value2.FontWeight = FontWeights.Normal;
            value2.Foreground = new SolidColorBrush(Colors.Black);
            value2.VerticalAlignment = VerticalAlignment.Center;
            value2.HorizontalAlignment = HorizontalAlignment.Center;
            value2.Padding = new Thickness(10, 0, 10, 0);
            value2.Text = $"{testItem.Type}: {testItem.Description}";

            Grid.SetRow(value, 0);
            Grid.SetColumn(value, 0);
            Grid.SetColumnSpan(value, 7);

            Grid.SetRow(value1, 1);
            Grid.SetColumn(value1, 0);

            Grid.SetRow(value2, 1);
            Grid.SetColumn(value2, 1);

            iTable.Children.Add(value);
            iTable.Children.Add(value1);
            iTable.Children.Add(value2);


            return iTable;
            XElement infoTable = new XElement("table",
                                    new XAttribute("class", "itemInfo"),
                                    new XElement("tbody",
                                        new XElement("tr",
                                            new XElement("td",
                                                new XAttribute("colspan", "3"),
                                                $"{testItem.Type}: {testItem.Description}"
                                            )
                                        ),
                                        new XElement("tr",
                                            new XElement("td", $"Status: {testItem.Status}", new XAttribute("class", $"status{testItem.Status}")),
                                            new XElement("td", $"Duration: {testItem.Duration}"),
                                            new XElement("td", $"Name: {testItem.Name}")
                                        )
                                     )
                                 );
           // return infoTable;

        }



        private Panel GetOverall(TestItem testItem)
        {
            if (testItem.Type == TestItemType.Test) return null;

            StackPanel panel = new StackPanel();
            StackPanel panel1 = new StackPanel();

            panel.SetValue(StackPanelProperty, 123);
            panel1.SetValue(StackPanelProperty, 256);
             
            
            var rr1 =  (int)panel1.GetValue(StackPanelProperty);
            var rr = (int)panel.GetValue(StackPanelProperty);
            IAddChild container = panel;

            container.AddChild(GetOverallCheckBox("Total", testItem.GetTotal(), "passed failed skipped notexecuted", true));
            container.AddChild(GetOverallCheckBox("NotExecuted", testItem.GetWithStatus(TestItemStatus.NotExecuted), "notexecuted"));
            container.AddChild(GetOverallCheckBox("Passed", testItem.GetWithStatus(TestItemStatus.Passed), "passed"));
            container.AddChild(GetOverallCheckBox("Failed", testItem.GetWithStatus(TestItemStatus.Failed), "failed"));
            container.AddChild(GetOverallCheckBox("Skipped", testItem.GetWithStatus(TestItemStatus.Skipped), "skipped"));



            var mainContainer = new XElement("div", new XAttribute("class", "checkboxes overall test-fltr-btns"));

            mainContainer.Add(GetOverallCheckBox("Total", testItem.GetTotal(), "passed failed skipped notexecuted", true));
            mainContainer.Add(GetOverallCheckBox("NotExecuted", testItem.GetWithStatus(TestItemStatus.NotExecuted), "notexecuted"));
            mainContainer.Add(GetOverallCheckBox("Passed", testItem.GetWithStatus(TestItemStatus.Passed), "passed"));
            mainContainer.Add(GetOverallCheckBox("Failed", testItem.GetWithStatus(TestItemStatus.Failed), "failed"));
            mainContainer.Add(GetOverallCheckBox("Skipped", testItem.GetWithStatus(TestItemStatus.Skipped), "skipped"));

            return panel;
        }






        public CheckBox GetOverallCheckBox(string text, int count, string filters, bool defaultExpander = false)
        {
            var check = new CheckBox();
            return check;
            


            var checkBox = new XElement("div",
                new XAttribute("class", "checkbox")
            );

            var input = new XElement("input", new XAttribute("type", "checkbox"), new XAttribute("filter", filters));
            var label = new XElement("label", text);
            var labelCount = new XElement("label", count);

            if (defaultExpander)
            {
                checkBox.Add(new XAttribute("defaultExpander", true));
            }

            checkBox.Add(input);
            checkBox.Add(label);
            checkBox.Add(labelCount);

            
        }







        private void SizeChangedEventHandler(object sender, SizeChangedEventArgs e)
        {
            var new_size = e.NewSize;

            foreach (var ch in EnvironmentGrid.Children)
            {
                var elem = ch as Grid;
                if (elem == null) continue;

                elem.Width = new_size.Width;
            }
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(string.Format("You clicked on the {0}. button.", (sender as Button).Tag));
        }
    }
}
