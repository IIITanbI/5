namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Base class for context items")]
    public abstract class BaseTestContextItem : BaseMetaObject
    {
        public virtual void Build(IContext context)
        {

        }

        public virtual List<BaseMetaObject> GetItems()
        {
            return null;
        }

        public virtual BaseMetaObject GetItem()
        {
            return null;
        }
    }
}
