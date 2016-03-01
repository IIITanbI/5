﻿namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using MetaMagic;
    using System.Reflection;

    [MetaType("TestingContext config")]
    [MetaLocation("context", "testingContext")]
    public class TestContext : BaseMetaObject, IContext
    {
        [MetaTypeCollection("List of TestContextItems", IsAssignableTypesAllowed = true, IsRequired = false)]
        [MetaLocation("contextItems")]
        public List<BaseMetaObject> TestContextItems { get; set; } = new List<BaseMetaObject>();

        [MetaTypeCollection("List of Managers", IsRequired = false)]
        [MetaLocation("managerItems", "managers")]
        public List<CommandManagerItem> CommandManagersItems { get; set; } = new List<CommandManagerItem>();

        public TestContext ParentContext { get; set; }
        public static Regex BindRegex = new Regex(@"\$\{([\w\s.]+)\}", RegexOptions.Compiled);

        public Dictionary<string, Dictionary<string, object>> ContextValues { get; set; } = new Dictionary<string, Dictionary<string, object>>();
        public Dictionary<string, Dictionary<string, object>> Managers = new Dictionary<string, Dictionary<string, object>>();
        public Dictionary<string, object> StepResults { get; set; } = new Dictionary<string, object>();

        public void Initialize()
        {
            ContextValues.ToList().ForEach(cv => cv.Value.Clear());
            ContextValues.Clear();

            if (ParentContext != null)
            {
                foreach (var TestItemTypeKey in ParentContext.ContextValues.Keys)
                {
                    ContextValues.Add(TestItemTypeKey, new Dictionary<string, object>());
                    foreach (var itemNameKey in ParentContext.ContextValues[TestItemTypeKey].Keys)
                    {
                        ContextValues[TestItemTypeKey].Add(itemNameKey, ParentContext.ContextValues[TestItemTypeKey][itemNameKey]);
                    }
                }

                foreach (var managerKey in ParentContext.Managers.Keys)
                {
                    Managers.Add(managerKey, ParentContext.Managers[managerKey]);
                }

                foreach (var stepResult in ParentContext.StepResults)
                {
                    StepResults.Add(stepResult.Key, stepResult.Value);
                }
            }

            foreach (var contextItem in TestContextItems)
            {
                var contextItemType = contextItem.GetType();
                var metaType = ReflectionManager.GetMetaType(contextItemType);

                var key = metaType.Key.GetValue(contextItem).ToString();

                if (key == null)
                    throw new FrameworkException($"Context object with type: {contextItemType} doesn't contains Key");

                if (!ContextValues.ContainsKey(metaType.Info.Name))
                    ContextValues.Add(metaType.Info.Name, new Dictionary<string, object>());

                if (ContextValues[metaType.Info.Name].ContainsKey(key))
                    throw new FrameworkException($"Error occurs during creating context object: {contextItemType.ToString()}. Object with the same name: {key} already present");

                ContextValues[metaType.Info.Name].Add(key, contextItem);
            }

            foreach (var managerItem in CommandManagersItems)
            {
                var manager = ReflectionManager.GetCommandManagerByTypeName(managerItem.ManagerType);
                var managerObj = manager.CreateInstance(managerItem.Config);

                if (!Managers.ContainsKey(managerItem.ManagerType))
                    Managers.Add(managerItem.ManagerType, new Dictionary<string, object>());

                if (Managers[managerItem.ManagerType].ContainsKey(managerItem.Name ?? managerItem.ManagerType))
                    Managers[managerItem.ManagerType][managerItem.Name ?? managerItem.ManagerType] = managerObj;
                else
                    Managers[managerItem.ManagerType].Add(managerItem.Name ?? managerItem.ManagerType, managerObj);
            }
        }

        public void Add(string path, object value)
        {
            var nameParts = path.Split('.');

            if (nameParts[0] == "Step")
            {
                StepResults.Add(nameParts[1], value);
            }
        }

        public void Add(Type type, string name, object value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Type type, string name)
        {
            if (!ContextValues.ContainsKey(type.Name))
                return false;
            if (!ContextValues[type.Name].ContainsKey(name))
                return false;
            return true;
        }

        public string ResolveBind(string stringWithBind)
        {
            var match = BindRegex.Match(stringWithBind);

            if (!match.Success)
                return stringWithBind;

            var sb = new StringBuilder(stringWithBind);
            var val = System.Security.SecurityElement.Escape(ResolveValue(match.Groups[1].Value).ToString());

            sb.Replace(match.Value, val, match.Index, match.Length);

            return ResolveBind(sb.ToString());
        }
        public object ResolveValue(string name)
        {
            var nameParts = name.Split('.');

            if (nameParts[0] == "Step")
            {
                return StepResults[nameParts[1]];
            }
            else if (nameParts[0] == "Manager")
            {
                var managerType = nameParts[1];
                var managerName = managerType;
                if (nameParts.Length == 3)
                    managerName = nameParts[2];

                return Managers[managerType][managerName];
            }
            else
            {
                var typeName = nameParts[0];
                var itemName = nameParts[1];

                return ContextValues[typeName][itemName];
            }
        }

        public object ResolveValue(Type type, string name)
        {
            return ContextValues[type.Name][name];
        }
    }
}
