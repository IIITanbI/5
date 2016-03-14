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

        public override Dictionary<string, Lazy<IMetaObject>> Build(TestContext context)
        {
            var dict = new Dictionary<string, Lazy<IMetaObject>>();
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

                if (dict.ContainsKey(key))
                    throw new FrameworkContextBuildingException(context.Item, $"Item with key: {key} has already present",
                        $"Context value object MetaType: {metaType}");

                dict.Add(key, new Lazy<IMetaObject>(() => obj));
            }
            return dict;
        }
    }
}
