namespace QA.AutomatedMagic.Framework.UI
{
    using MetaMagic;
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
    using System.Xml.Linq;
    using WpfManagingFillers;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ReflectionManager.LoadAssemblies();
            InitializeComponent();

            var creator = new Creator(B1, ReflectionManager.GetMetaType(typeof(ComboProst)), "Test", true);
        }

        private void B1_Click(object sender, RoutedEventArgs e)
        {
            //Test();
        }

        public void Test()
        {
            var prostConfig1 = new XElement("ProstProperty",
                new XElement("StringProp", "StringProp1"),
                new XElement("StringField", "StringField1"),
                new XElement("telepuziki",
                    new XElement("telepuzik", "TinkiVinki"),
                    new XElement("telepuzik", "Dipsi")
                ),
                new XElement("ListStringField",
                    new XElement("String", "val1"),
                    new XElement("String", "val2")
                )
            );

            var prostConfig2 = new XElement("Prost1",
                new XElement("StringProp", "StringProp1"),
                new XElement("StringField", "StringField1"),
                new XElement("telepuziki",
                    new XElement("telepuzik", "TinkiVinki"),
                    new XElement("telepuzik", "Dipsi")
                ),
                new XElement("ListStringField",
                    new XElement("String", "val1"),
                    new XElement("String", "val2")
                )
            );

            var metaType = ReflectionManager.GetMetaType(typeof(Prost));
            var obj1 = metaType.Parse(prostConfig1);
            var obj2 = metaType.Parse(prostConfig2);

            var comboProstConfig1 = new XElement("ComboProst",
                prostConfig1,
                new XElement("ProstField", prostConfig2)
            );

            var comboMetaType = ReflectionManager.GetMetaType(typeof(ComboProst));
            var obj3 = comboMetaType.Parse(comboProstConfig1);



            var prostConfig3 = new XElement("Prost",
                new XElement("StringProp", "StringProp1"),
                new XElement("StringField", "StringField1"),
                new XElement("telepuziki",
                    new XElement("telepuzik", "TinkiVinki"),
                    new XElement("telepuzik", "Dipsi")
                ),
                new XElement("ListStringField",
                    new XElement("String", "val1"),
                    new XElement("String", "val2")
                )
            );

            var comboProstConfig2 = new XElement("ComboProst",
                prostConfig1,
                new XElement("ProstField", prostConfig2),
                new XElement("Prosts",
                    prostConfig3,
                    prostConfig2
                )
            );

            var comboProstConfig3 = new XElement("ComboProst",
                prostConfig1,
                new XElement("ProstField", prostConfig2)
            );

            var obj4 = MetaType.Parse<ComboProst>(comboProstConfig2);
            obj4.Ts = TimeSpan.FromMilliseconds(20110646);
            comboMetaType.ManagingFiller.GetManagingObjectFiller().FillEditControls(S1, obj4, comboMetaType, "Test", false);
        }

        [MetaType("Test Prost class", "StringProp")]
        class Prost : BaseMetaObject
        {
            public Prost()
            {
                Console.WriteLine(1);
            }

            [MetaTypeValue("Value String property")]
            public string StringProp { get; set; }

            [MetaTypeValue("Value String field")]
            public string StringField;

            [MetaTypeCollection("Collection ListString property", "telepuzik")]
            [MetaLocation("telepuziki")]
            public List<string> ListStringProperty { get; set; }

            [MetaTypeCollection("Collection ListString field")]
            public List<string> ListStringField;
        }

        [MetaType("Extended Prost")]
        class Prost1 : Prost
        {
            [MetaTypeValue("Int field", IsRequired = false)]
            public int IntField = 5;
        }

        [MetaType("Test ComboProst class")]
        class ComboProst : BaseMetaObject
        {
            [MetaTypeObject("Object Prost property")]
            public Prost ProstProperty { get; set; }

            [MetaTypeObject("Object Prost field", IsAssignableTypesAllowed = true)]
            public Prost ProstField;

            [MetaTypeCollection("List of Prosts", IsAssignableTypesAllowed = true, IsRequired = false)]
            public List<Prost> Prosts;

            [MetaTypeValue("Timestamp", IsRequired = false)]
            public TimeSpan Ts;
        }
    }
}
