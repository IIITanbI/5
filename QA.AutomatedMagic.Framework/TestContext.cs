namespace QA.AutomatedMagic.Framework
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
        public List<BaseTestContextItem> TestContextItems { get; set; } = new List<BaseTestContextItem>();

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
                contextItem.Build(this);

                var itemsToAdd = new List<BaseMetaObject>();

                var item = contextItem.GetItem();
                if (item != null)
                {
                    itemsToAdd.Add(item);
                }

                var items = contextItem.GetItems();
                if (items != null)
                {
                    itemsToAdd.AddRange(items);
                }

                foreach (var itemToAdd in itemsToAdd)
                {
                    var itemToAddType = itemToAdd.GetType();
                    var metaType = AutomatedMagicManager.GetMetaType(itemToAddType);

                    var key = metaType.Key?.GetValue(itemToAdd).ToString();

                    if (key == null)
                        throw new FrameworkException($"Context object with type: {itemToAddType} doesn't contains Key");

                    Add(itemToAddType, key, itemToAdd);
                }
            }

            foreach (var managerItem in CommandManagersItems)
            {
                var manager = AutomatedMagicManager.GetCommandManagerByTypeName(managerItem.ManagerType);
                var managerObj = manager.CreateInstance(managerItem.Config, this);

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
            if (!ContextValues.ContainsKey(type.Name))
                ContextValues.Add(type.Name, new Dictionary<string, object>());

            if (ContextValues[type.Name].ContainsKey(name))
                throw new FrameworkException($"Error occurs during creating context object with type: {type}. Object with the same name: {name} already present");

            ContextValues[type.Name].Add(name, value);
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
            object objToReturn = null;

            if (nameParts[0] == "Step")
            {
                objToReturn = StepResults[nameParts[1]];
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

                objToReturn = ContextValues[typeName][itemName];
            }

            if (nameParts.Length > 2)
            {
                var bmo = objToReturn as BaseMetaObject;
                if (bmo != null)
                {
                    var path = name.Replace($"{nameParts[0]}.{nameParts[1]}.", "");
                    objToReturn = bmo.ResolvePath(path);
                }
            }

            return objToReturn;
        }
        public object ResolveValue(Type type, string name)
        {
            return ContextValues[type.Name][name];
        }
    }
}
