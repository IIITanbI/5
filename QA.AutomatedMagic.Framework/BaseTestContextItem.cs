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

        public virtual List<IMetaObject> GetItems()
        {
            return null;
        }

        public virtual IMetaObject GetItem()
        {
            return null;
        }
    }
}
