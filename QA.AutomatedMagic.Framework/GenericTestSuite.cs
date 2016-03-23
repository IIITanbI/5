namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;
    using System.Xml.Linq;
    using System.Collections;

    [MetaType("Generic TestSuite")]
    public class GenericTestSuite : TestSuite
    {
        [MetaTypeObject("Generic config")]
        public GenericConfig Config { get; set; }

        public override void Build()
        {
            base.Build();

            TestManager.Log.INFO($"Start generating for {this}");

            #region Generate
            try
            {
                IEnumerable source = null;
                try
                {
                    source = (IEnumerable)Context.ResolveValue(Config.ForeachItemIn);
                }
                catch (Exception ex)
                {
                    throw new FrameworkGeneratingException(this, "Error occurred during resolving generation source", ex);
                }

                TestManager.Log.INFO($"Start children generating");
                foreach (var item in source)
                {
                    IMetaObject obj = null;
                    try
                    {
                        obj = (IMetaObject)item;
                    }
                    catch (Exception ex)
                    {
                        throw new FrameworkGeneratingException(this, "Genetic item type is not IMetaObject", ex,
                            $"Item type: {item.GetType()}");
                    }

                    var metaType = AutomatedMagicManager.GetMetaType(obj.GetType());

                    if (metaType.Key == null)
                        throw new FrameworkGeneratingException(this, "Genetic item MetaType doesn't contain Key property",
                            $"Genetic item MetaType: {metaType}");

                    var key = "";

                    try
                    {
                        key = metaType.Key.GetValue(item)?.ToString() ?? "";
                    }
                    catch (Exception ex)
                    {
                        throw new FrameworkGeneratingException(this, "Error occurred during parsing Key property", ex,
                            $"Context value object MetaType: {metaType}");
                    }

                    if (key == "")
                        throw new FrameworkContextBuildingException(this, "Genetic item Key is null or empty",
                            $"Context value object MetaType: {metaType}");

                    var templateXml = new XElement(Config.ChildTemplate.Elements().First());
                    ResolveGenericNode(templateXml, obj);

                    var child = (TestCase)MetaType.Parse(typeof(TestCase), templateXml, true);
                    child.Context.AddContextValue(new TestContextValueInfo { ValueMetaType = metaType, ValueKey = key, ValueValue = new Lazy<IMetaObject>(() => obj) });
                    Children.Add(child);
                }
            }
            catch (Exception ex)
            {
                TestManager.Log.ERROR($"Error occurred during generating item: {this}", ex);
            }
            #endregion

            TestManager.Log.INFO($"Generating for {this} successfully completed");

            TestManager.Log.INFO($"Start building children for item: {this}");
            TestManager.Log.INFO($"Children count: {Children.Count}");

            foreach (var child in Children)
            {
                child.Parent = this;
                child.Build();
            }

            TestManager.Log.INFO($"Children were successfully built for item: {this}");
            TestManager.Log.INFO($"Build was successfully completed for item: {this}");
        }

        private void ResolveGenericNode(XElement element, IMetaObject item)
        {
            if (element.Name == nameof(GenericTestSuite))
                return;
            if (element.Name == "GenericNode")
            {
                try
                {
                    var valueToResolve = element.Value;
                    var value = item.ResolvePath(valueToResolve).ToString();
                    element.ReplaceWith(new XCData(value));
                }
                catch (Exception ex)
                {
                    throw new FrameworkContextBuildingException(null, $"Error occurred during generic node resolving", ex);
                }
                return;
            }

            var children = element.Elements().ToList();

            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                ResolveGenericNode(child, item);
            }
        }
    }
}
