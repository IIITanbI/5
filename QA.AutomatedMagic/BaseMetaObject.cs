namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Base class for all MetaObjects")]
    public abstract class BaseMetaObject : IMetaObject
    {
        public virtual void Init()
        {

        }
    }
}
