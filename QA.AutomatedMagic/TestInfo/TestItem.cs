namespace QA.AutomatedMagic.TestInfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using MetaMagic;

    [MetaType("TestItem config")]
    public class TestItem : BaseMetaObject
    {
        [MetaTypeValue("Test item name")]
        public string Name { get; set; }

        [MetaTypeValue("Test item description")]
        public string Description { get; set; }

        [MetaTypeValue("Test item type")]
        public TestItemType Type { get; set; }

        [MetaTypeCollection("Test item log")]
        public List<LogItem> LogMessages { get; set; } = new List<LogItem>();

        [MetaTypeValue("Test item status")]
        public TestItemStatus Status { get; set; }

        [MetaTypeValue("Test item duration")]
        public TimeSpan Duration { get; set; }

        [MetaTypeCollection("Test item steps", IsRequired = false)]
        public List<Step> Steps { get; set; } = new List<Step>();

        [MetaTypeCollection("List of test item childes", IsRequired = false)]
        public List<TestItem> Childs { get; set; } = new List<TestItem>();

        [MetaTypeCollection("Test item tags", "tag", IsRequired = false)]
        public List<string> Tags { get; set; }

        public int GetTotal()
        {
            if (this.Type == TestItemType.Test)
            {
                return 1;
            }

            int tmp = 0;
            foreach (var child in Childs)
            {
                tmp += child.GetTotal();
            }
            
            return tmp;
        }

        public int GetWithStatus(TestItemStatus status)
        {
            if (this.Type == TestItemType.Test)
            {
                return this.Status == status? 1 : 0;
            }

            int tmp = 0;
            foreach (var child in Childs)
            {
                tmp += child.GetWithStatus(status);
            }

            return tmp;
        }
    }
}
