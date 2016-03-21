namespace QA.AutomatedMagic.MetaMagic
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
        public virtual List<string> GetPaths()
        {
            var metaType = AutomatedMagicManager.GetMetaType(GetType());
            return metaType.GetPaths(this);
        }

        public virtual void MetaInit()
        {
            
        }

        public virtual object ResolvePath(string path)
        {
            var metaType = AutomatedMagicManager.GetMetaType(GetType());
            return metaType.ResolvePath(path, this);
        }
    }
}
