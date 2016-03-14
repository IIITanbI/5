namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Base class for all Test items")]
    public abstract class TestItem : BaseMetaObject
    {
        [MetaTypeObject("Test item info")]
        [MetaLocation(true)]
        public TestItemInfo Info { get; set; }

        [MetaTypeValue("Number of tries to execute", IsRequired = false)]
        public int TryCount { get; set; } = 1;

        [MetaTypeObject("Test item Context", IsRequired = false)]
        public TestContext Context { get; set; } = new TestContext();

        [MetaTypeValue("Is item enabled?", IsRequired = false)]
        public bool IsEnabled { get; set; } = true;

        public TestItem Parent { get; set; } = null;
        public TestLogger Log { get; set; } = null;

        public abstract TestItemType ItemType { get; protected set; }
        public TestItemStatus ItemStatus { get; set; } = TestItemStatus.NotExecuted;
        protected int _tryNumber = 1;

        public virtual void Build()
        {
            Log = new TestLogger(GetFullName());

            TestManager.Log.INFO($"Start building context for item: {this}");
            Context.Item = this;
            Context.Build();
            TestManager.Log.INFO($"Context was successfully built for item: {this}");
        }
        public abstract void Execute();

        public override string ToString()
        {
            return $"{GetType().Name} : {Info}";
        }

        public virtual string GetName()
        {
            return $"({GetType().Name}) {Info.Name}";
        }

        public virtual string GetFullName()
        {
            if (Parent == null)
                return GetName();

            return $"{Parent.GetFullName()} $ {GetName()}";
        }
    }
}
