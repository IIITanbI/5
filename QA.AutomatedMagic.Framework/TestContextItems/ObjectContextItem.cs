namespace QA.AutomatedMagic.Framework.TestContextItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Object context item config")]
    public class ObjectContextItem : BaseTestContextItem
    {
        [MetaTypeObject("Object config", IsAssignableTypesAllowed = true)]
        [MetaLocation("object")]
        public BaseMetaObject ObjectItem { get; set; }

        public override BaseMetaObject GetItem()
        {
            return ObjectItem;
        }
    }
}
