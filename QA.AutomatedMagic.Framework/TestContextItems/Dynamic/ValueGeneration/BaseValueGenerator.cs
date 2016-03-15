namespace QA.AutomatedMagic.Framework.TestContextItems.Dynamic.ValueGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Interface for generators")]
    public abstract class BaseValueGenerator : BaseMetaObject
    {
        protected static Random _random = new Random();
        public abstract object GenerateValue();
    }
}
