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

        public virtual List<string> GetPaths()
        {
            var metaType = ReflectionManager.GetMetaType(GetType());
            return metaType.GetPaths(this);
        }

        public virtual object ResolvePath(string path)
        {
            var metaType = ReflectionManager.GetMetaType(GetType());
            return metaType.ResolvePath(path, this);
        }
    }
}
