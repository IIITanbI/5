namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Test item info")]
    public class TestItemInfo : BaseMetaObject
    {
        [MetaTypeValue("Test item Name")]
        public string Name { get; set; }

        [MetaTypeValue("Test item Description")]
        public string Description { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Description: {Description}";
        }
    }
}
