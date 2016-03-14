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

    [MetaType("Resolvable context object item")]
    public class DynamicContextObjectItem : TestContextItem
    {
        [MetaTypeValue("Config for element with binds")]
        public XElement DynamicObjects { get; set; }

        public override Dictionary<string, Lazy<IMetaObject>> Build(TestContext context)
        {
            var children = DynamicObjects.Elements().ToList();
            var dict = new Dictionary<string, Lazy<IMetaObject>>();

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

                if (dict.ContainsKey(key))
                    throw new FrameworkContextBuildingException(context.Item, $"Item with key: {key} has already present",
                        $"Context value object MetaType: {metaType}");

                dict.Add(key, new Lazy<IMetaObject>(() =>
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
                    }));
            }

            return dict;
        }

        private void ResolveNode(string key, XElement element, TestContext context)
        {
            var children = element.Elements().ToList();
            if (children.Count == 1)
            {
                var child = children[0];
                if (child.Name == "DynamicNode")
                {
                    var type = child.Attribute("type")?.Value;
                    if (type == null)
                        throw new FrameworkContextBuildingException(context.Item, "Dynamic node type isn't specified",
                            $"Object key: {key}");

                    switch (type)
                    {
                        case "Context":

                            var obj = context.ResolveValue(child.Value);
                            element.Value = obj.ToString();

                            break;
                        default:
                            throw new FrameworkContextBuildingException(context.Item, $"Unknown dynamic node type: {type}",
                                $"Object key: {key}");
                    }
                }
            }

            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                ResolveNode(key, child, context);
            }
        }
    }
}
