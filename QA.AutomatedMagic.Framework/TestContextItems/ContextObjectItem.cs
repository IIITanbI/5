namespace QA.AutomatedMagic.Framework.TestContextItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Context object item")]
    public class ContextObjectItem : TestContextItem
    {
        [MetaTypeCollection("List of context objects. Each must have Key", "object", IsAssignableTypesAllowed = true)]
        public List<IMetaObject> Objects { get; set; }

        public override List<TestContextValueInfo> Build(TestContext context)
        {
            var valueInfos = new List<TestContextValueInfo>();
            foreach (var obj in Objects)
            {
                var metaType = AutomatedMagicManager.GetMetaType(obj.GetType());

                if (metaType.Key == null)
                    throw new FrameworkContextBuildingException(context.Item, "Context value object MetaType doesn't contain Key property",
                        $"Context value object MetaType: {metaType}");

                var key = metaType.Key.GetValue(obj)?.ToString() ?? "";

                if (key == "")
                    throw new FrameworkContextBuildingException(context.Item, "Context value object Key is null or empty",
                        $"Context value object MetaType: {metaType}");

                if (valueInfos.Any(vi => vi.ValueKey == key))
                    throw new FrameworkContextBuildingException(context.Item, $"Item with key: {key} has already present",
                        $"Context value object MetaType: {metaType}");

                valueInfos.Add(new TestContextValueInfo { ValueKey = key, ValueValue = new Lazy<IMetaObject>(() => obj), ValueMetaType = metaType });
            }
            return valueInfos;
        }
    }
}
