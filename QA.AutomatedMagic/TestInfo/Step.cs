namespace QA.AutomatedMagic.TestInfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Step config")]
    public class Step : BaseMetaObject
    {
        [MetaTypeCollection("Step log", IsRequired = false)]
        public List<LogItem> Messages { get; set; } = new List<LogItem>();

        [MetaTypeValue("Step status")]
        public TestItemStatus Status { get; set; }

        [MetaTypeValue("Step name")]
        public string Name { get; set; }

        [MetaTypeValue("Step description")]
        public string Description { get; set; }

        [MetaTypeValue("Step duration")]
        public TimeSpan Duration { get; set; }
    }
}
