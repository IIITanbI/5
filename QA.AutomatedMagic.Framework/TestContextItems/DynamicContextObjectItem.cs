namespace QA.AutomatedMagic.Framework.TestContextItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;
    using System.Xml.Linq;
    using XmlSourceResolver;
    using Dynamic;

    [MetaType("Resolvable context object item")]
    public class DynamicContextObjectItem : TestContextItem
    {
        [MetaTypeValue("Config for element with binds")]
        public XElement DynamicObjects { get; set; }

        public override List<TestContextValueInfo> Build(TestContext context)
        {
            var children = DynamicObjects.Elements().ToList();
            var valueInfos = new List<TestContextValueInfo>();

            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                var metaType = AutomatedMagicManager.GetMetaType(child.Name.LocalName);
                if (metaType == null)
                    throw new FrameworkContextBuildingException(context.Item, $"Couldn't find MetaType with name: {child.Name.LocalName}");

                if (metaType.Key == null)
                    throw new FrameworkContextBuildingException(context.Item, "Context value object MetaType doesn't contain Key property",
                        $"Context value object MetaType: {metaType}");

                var key = "";

                try
                {
                    key = metaType.Key.Parse(child)?.ToString() ?? "";
                }
                catch (Exception ex)
                {
                    throw new FrameworkContextBuildingException(context.Item, "Error occurred during parsing Key property", ex,
                        $"Context value object MetaType: {metaType}");
                }

                if (key == "")
                    throw new FrameworkContextBuildingException(context.Item, "Context value object Key is null or empty",
                        $"Context value object MetaType: {metaType}");

                if (valueInfos.Any(vi => vi.ValueKey == key))
                    throw new FrameworkContextBuildingException(context.Item, $"Item with key: {key} has already present",
                        $"Context value object MetaType: {metaType}");

                valueInfos.Add(new TestContextValueInfo
                {
                    ValueKey = key,
                    ValueMetaType = metaType,
                    ValueValue = new Lazy<IMetaObject>(
                        () =>
                        {
                            ResolveNode(key, child, context);
                            try
                            {
                                var obj = (IMetaObject)metaType.Parse(child);
                                return obj;
                            }
                            catch (Exception ex)
                            {
                                throw new FrameworkContextBuildingException(context.Item, $"Error occurred during parsing object", ex,
                                    $"Object key: {key}",
                                    $"Context value object MetaType: {metaType}");
                            }
                        })
                });
            }

            return valueInfos;
        }

        private void ResolveNode(string key, XElement element, TestContext context)
        {
            if (element.Name == "DynamicNode")
            {
                try
                {
                    var node = MetaType.Parse<DynamicNode>(element);
                    element.ReplaceWith(new XCData(node.GetValue(context).ToString()));
                }
                catch (Exception ex)
                {
                    throw new FrameworkContextBuildingException(context.Item, $"Error occurred during dynamic node resolving", ex,
                        $"Object key: {key}");
                }
                return;
            }

            var children = element.Elements().ToList();

            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                ResolveNode(key, child, context);
            }
        }
    }
}
