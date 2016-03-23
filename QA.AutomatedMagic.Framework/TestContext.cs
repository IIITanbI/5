namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;
    using CommandsMagic;

    [MetaType("Test context")]
    public class TestContext : BaseMetaObject, IContext
    {
        [MetaTypeCollection("List of context items", IsRequired = false, IsAssignableTypesAllowed = true)]
        [MetaLocation("items")]
        public List<TestContextItem> ContextItems { get; set; } = new List<TestContextItem>();

        [MetaTypeCollection("List of context manager items", "manager", IsRequired = false)]
        [MetaLocation("managers")]
        public List<TestContextManagerItem> ContextManagerItems { get; set; } = new List<TestContextManagerItem>();

        public TestItem Item { get; set; } = null;
        public Dictionary<string, Dictionary<string, Lazy<IMetaObject>>> ContextValues { get; set; } = new Dictionary<string, Dictionary<string, Lazy<IMetaObject>>>();
        public Dictionary<string, Dictionary<string, BaseCommandManager>> ContextManagers { get; set; } = new Dictionary<string, Dictionary<string, BaseCommandManager>>();
        public Dictionary<string, object> StepResults { get; set; } = new Dictionary<string, object>();

        public void Build()
        {
            #region Building context values
            foreach (var contextItem in ContextItems)
            {
                List<TestContextValueInfo> contextValues = null;
                try
                {
                    contextValues = contextItem.Build(this);
                }
                catch (Exception ex)
                {
                    throw new FrameworkContextBuildingException(Item, $"Error occurred during building context item with name: {contextItem.Name}", ex);
                }

                if (contextValues != null)
                {
                    foreach (var contextValue in contextValues)
                    {
                        try
                        {
                            AddContextValue(contextValue);
                        }
                        catch (FrameworkContextBuildingException fcbEx)
                        {
                            throw new FrameworkContextBuildingException(Item, "Error occurred during adding context value", fcbEx, $"Context item name: {contextItem.Name}");
                        }
                    }
                }
            }
            #endregion

            #region Build context managers
            foreach (var contextManagerItem in ContextManagerItems)
            {
                var managerInfo = AutomatedMagicManager.GetCommandManagerByTypeName(contextManagerItem.ManagerType);

                if (managerInfo == null)
                    throw new FrameworkContextBuildingException(Item, $"Couldn't find manager descriptor with type name: {contextManagerItem.ManagerType}");

                var managerName = contextManagerItem.Name ?? managerInfo.CommandManagerType.Name;

                if (!ContextManagers.ContainsKey(managerInfo.CommandManagerType.Name))
                    ContextManagers.Add(managerInfo.CommandManagerType.Name, new Dictionary<string, BaseCommandManager>());

                if (ContextManagers[managerInfo.CommandManagerType.Name].ContainsKey(managerName))
                    throw new FrameworkContextBuildingException(Item, $"Context managers have already contained manager with name: {managerName}",
                        $"Manager type: {managerInfo.CommandManagerType.Name}");

                BaseCommandManager managerObj = null;
                try
                {
                    managerObj = managerInfo.CreateInstance(contextManagerItem.ManagerConfig);
                }
                catch (Exception ex)
                {
                    throw new FrameworkContextBuildingException(Item, $"Error occurred during creating manager", ex,
                        $"Manager type: {managerInfo.CommandManagerType.Name}",
                        $"Manager name: {managerName}");
                }

                ContextManagers[managerInfo.CommandManagerType.Name].Add(managerName, managerObj);
            }
            #endregion
        }

        public void AddContextValue(TestContextValueInfo contextValue)
        {
            var metaType = contextValue.ValueMetaType;

            if (!ContextValues.ContainsKey(metaType.Info.Name))
                ContextValues.Add(metaType.Info.Name, new Dictionary<string, Lazy<IMetaObject>>());

            if (ContextValues[metaType.Info.Name].ContainsKey(contextValue.ValueKey))
                throw new FrameworkContextBuildingException(Item, "Context values have already contained object with the same Key",
                    $"Key: {contextValue.ValueKey}",
                    $"Context value object MetaType: {metaType}");

            ContextValues[metaType.Info.Name].Add(contextValue.ValueKey, contextValue.ValueValue);
        }

        public void AddStepResult(string stepName, object result)
        {
            if (!StepResults.ContainsKey(stepName))
                StepResults.Add(stepName, result);
            else
                StepResults[stepName] = result;
        }

        public BaseCommandManager GetManager(string typeName, string name)
        {
            if (ContextManagers.ContainsKey(typeName) && ContextManagers[typeName].ContainsKey(name))
                return ContextManagers[typeName][name];

            return Item.Parent?.Context.GetManager(typeName, name);
        }

        public object ResolveValue(string path)
        {
            path = path.Trim();

            if (path == "")
                throw new FrameworkContextResolvingException(Item, "Context Path is empty");

            var parts = path.Split('.');

            if (parts.Length < 2)
                throw new FrameworkContextResolvingException(Item, "Context Path contains less then two parts separated by dot '.'",
                    $"Path: {path}");

            var typeName = parts[0].Trim();
            var objectKey = parts[1].Trim();

            if (ContextValues.ContainsKey(typeName) && ContextValues[typeName].ContainsKey(objectKey))
            {
                var obj = ContextValues[typeName][objectKey].Value;

                if (parts.Length == 2)
                    return obj;

                if (parts.Length > 2)
                {
                    var innerPath = path.Substring(path.IndexOf('.') + 1);
                    innerPath = innerPath.Substring(innerPath.IndexOf('.') + 1);

                    try
                    {
                        return obj.ResolvePath(innerPath);
                    }
                    catch (Exception ex)
                    {
                        throw new FrameworkContextResolvingException(Item, "Context Object couldn't resolve inner path", ex,
                            $"Full path: {path}",
                            $"Type name: {typeName}",
                            $"Object key: {objectKey}");
                    }
                }
            }

            try
            {
                return Item.Parent?.Context.ResolveValue(path);
            }
            catch (FrameworkContextResolvingException ex)
            {
                throw new FrameworkContextResolvingException(Item, "Error occurred during Parent Context path resolving", ex);
            }
        }

        public object ResolveManager(string path)
        {
            path = path.Trim();

            if (path == "")
                throw new FrameworkContextResolvingException(Item, "Manager Path is empty");

            var parts = path.Split('.');

            if (parts.Length > 2)
                throw new FrameworkContextResolvingException(Item, "Manager Path contains more then two parts separated by dot '.'",
                    $"Path: {path}");

            var typeName = parts[0].Trim();
            var managerName = parts.Length == 2
                ? parts[1].Trim()
                : typeName;

            if (ContextManagers.ContainsKey(typeName) && ContextManagers[typeName].ContainsKey(managerName))
                return ContextManagers[typeName][managerName];

            return Item.Parent?.Context.GetManager(typeName, managerName);
        }

        public object ResolveStepResult(string stepName)
        {
            stepName = stepName.Trim();

            if (StepResults.ContainsKey(stepName))
                return StepResults[stepName];

            return Item.Parent?.Context.ResolveStepResult(stepName);
        }


        public object ResolveValue(Type type, string name)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Type type, string name)
        {
            throw new NotImplementedException();
        }

        public void Add(Type type, string name, object value)
        {
            throw new NotImplementedException();
        }

        public void Add(string path, object value)
        {
            throw new NotImplementedException();
        }

        public string ResolveBind(string stringWithBind)
        {
            throw new NotImplementedException();
        }
    }
}
