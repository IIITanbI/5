﻿namespace QA.AutomatedMagic.CommandsMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Reflection;
    using System.Xml.Linq;
    using MetaMagic;
    using System.Xml.XPath;
    using System.IO;
    public class CommandManager
    {
        public string Description { get; private set; }
        public Type CommandManagerType { get; private set; }
        public Type CommandManagerConfigType { get; private set; }

        public List<Command> Commands { get; private set; } = new List<Command>();
        public Lazy<List<CommandManager>> ChildManagers { get; private set; } = null;
        private Dictionary<Type, PropertyInfo> _childManagersProperties = null;
        private Dictionary<PropertyInfo, MetaSourceAttribute> _sourcableProperties = new Dictionary<PropertyInfo, MetaSourceAttribute>();

        public string AssemblyDirectoryPath { get; private set; }

        public CommandManager(CommandManagerAttribute commandManagerAttribute, Type commandManagerType)
        {
            Description = commandManagerAttribute.Description;
            CommandManagerType = commandManagerType;
            CommandManagerConfigType = commandManagerAttribute.ConfigType;
            AssemblyDirectoryPath = Path.GetDirectoryName(commandManagerType.Assembly.Location);

            Commands = new List<Command>();
            var methods = CommandManagerType.GetMethods();

            foreach (var method in methods)
            {
                var commandAttribute = method.GetCustomAttribute<CommandAttribute>();
                if (commandAttribute != null)
                {
                    var command = new Command(commandAttribute, method);
                    Commands.Add(command);
                }
            }

            var props = CommandManagerType.GetProperties();
            var childManagerProperties = props.Where(p => typeof(BaseCommandManager).IsAssignableFrom(p.PropertyType)).ToList();

            if (childManagerProperties.Count > 0)
            {
                _childManagersProperties = new Dictionary<Type, PropertyInfo>();

                foreach (var childManagerProperty in childManagerProperties)
                {
                    _childManagersProperties.Add(childManagerProperty.PropertyType, childManagerProperty);
                }

                ChildManagers = new Lazy<List<CommandManager>>(
                    () =>
                    {
                        var list = new List<CommandManager>();

                        foreach (var childManagerProperty in childManagerProperties)
                        {
                            var manager = AutomatedMagicManager.GetCommandManagerByTypeName(childManagerProperty.PropertyType.Name);
                            list.Add(manager);
                        }

                        return list;
                    }
                );
            }

            foreach (var property in props)
            {
                var metaSourceAttribute = property.GetCustomAttribute<MetaSourceAttribute>();
                if (metaSourceAttribute != null)
                {
                    _sourcableProperties.Add(property, metaSourceAttribute);
                }
            }
        }

        public BaseCommandManager CreateInstance(object config)
        {
            BaseCommandManager managerObject = null;

            try
            {
                if (CommandManagerConfigType == null)
                {
                    managerObject = (BaseCommandManager)Activator.CreateInstance(CommandManagerType);
                }
                else
                {
                    managerObject = (BaseCommandManager)Activator.CreateInstance(CommandManagerType, new object[] { config });
                }
            }
            catch (Exception ex)
            {
                throw new AutomatedMagicException($"Error occurred during creating manager object for Manager: {CommandManagerType.Name}", ex);
            }

            foreach (var sourcableProperty in _sourcableProperties)
            {
                try
                {
                    var sourceXml = XDocument.Load(Path.Combine(AssemblyDirectoryPath, sourcableProperty.Value.Path));
                    var configElement = sourceXml.XPathSelectElement(sourcableProperty.Value.RootElementXPath);
                    var propertyObject = MetaType.Parse(sourcableProperty.Key.PropertyType, configElement, true);
                    sourcableProperty.Key.SetValue(managerObject, propertyObject);
                }
                catch (Exception ex)
                {
                    throw new AutomatedMagicException($"Error occurred during initialization property {sourcableProperty.Key.Name} for Manager: {CommandManagerType.Name}", ex);
                }
            }

            try
            {
                managerObject.Init();
            }
            catch (Exception ex)
            {
                throw new AutomatedMagicException($"Error occurred during initialization manager object for Manager: {CommandManagerType.Name}", ex);
            }

            return managerObject;
        }

        public object ExecuteCommand(object managerObject, string commandName, List<object> parObjs, ILogger log)
        {
            var propInfos = new List<PropertyInfo>();
            var possibleCommands = FindCommand(commandName, propInfos);

            if (possibleCommands == null || possibleCommands.Count == 0)
                throw new NotImplementedException();

            Command acceptedCommand = null;
            var parArray = new List<object>();
            foreach (var command in possibleCommands)
            {
                var curIndex = 0;
                var isBad = false;
                for (int i = 0; i < command.Parameters.Length; i++)
                {
                    var paramInfo = command.Parameters[i];
                    if (paramInfo.ParameterType == typeof(ILogger))
                    {
                        parArray.Add(log);
                        continue;
                    }
                    if (paramInfo.ParameterType.IsAssignableFrom(parObjs[curIndex].GetType()))
                    {
                        parArray.Add(parObjs[curIndex]);
                        curIndex++;
                    }
                    else
                    {
                        isBad = true;
                        break;
                    }
                }

                if (!isBad)
                {
                    acceptedCommand = command;
                    break;
                }
                else
                {
                    parArray.Clear();
                }
            }

            if (acceptedCommand == null)
                throw new NotImplementedException();

            foreach (var propInfo in propInfos)
            {
                managerObject = propInfo.GetValue(managerObject);
            }

            var result = acceptedCommand.Method.Invoke(managerObject, parArray.ToArray());

            return result;
        }

        public CommandExecutionInfo GetCommandExecutionInfo(object managerObject, string commandName, List<object> parObjs, ILogger log)
        {
            var propInfos = new List<PropertyInfo>();
            var possibleCommands = FindCommand(commandName, propInfos);

            if (possibleCommands == null || possibleCommands.Count == 0)
                return null;

            Command acceptedCommand = null;
            var parArray = new List<object>();
            foreach (var command in possibleCommands)
            {
                var curIndex = 0;
                var isBad = false;
                for (int i = 0; i < command.Parameters.Length; i++)
                {
                    var paramInfo = command.Parameters[i];
                    if (paramInfo.ParameterType == typeof(ILogger))
                    {
                        parArray.Add(log);
                        continue;
                    }
                    if (paramInfo.ParameterType.IsAssignableFrom(parObjs[curIndex].GetType()))
                    {
                        parArray.Add(parObjs[curIndex]);
                        curIndex++;
                    }
                    else
                    {
                        isBad = true;
                        break;
                    }
                }

                if (!isBad)
                {
                    acceptedCommand = command;
                    break;
                }
                else
                {
                    parArray.Clear();
                }
            }

            if (acceptedCommand == null)
                return null;

            foreach (var propInfo in propInfos)
            {
                managerObject = propInfo.GetValue(managerObject);
            }

            var commandExecutor = new CommandExecutionInfo { Method = acceptedCommand.Method, Arguments = parArray.ToArray(), ManagerObject = managerObject };
            return commandExecutor;
        }

        private List<Command> FindCommand(string commandName, List<PropertyInfo> propInfos)
        {
            if (commandName.StartsWith(CommandManagerType.Name + "."))
            {
                commandName = commandName.Substring(CommandManagerType.Name.Length);
                return Commands.Where(c => c.PossibleNames.Contains(commandName)).ToList();
            }

            var commands = Commands.Where(c => c.PossibleNames.Contains(commandName)).ToList();

            if (commands.Count == 0)
            {
                if (ChildManagers == null)
                {
                    return null;
                }
                else
                {
                    foreach (var childManager in ChildManagers.Value)
                    {
                        propInfos.Add(_childManagersProperties[childManager.CommandManagerType]);
                        commands = childManager.FindCommand(commandName, propInfos);

                        if (commands != null && commands.Count > 0)
                            return commands;

                        propInfos.Remove(_childManagersProperties[childManager.CommandManagerType]);
                    }
                }
            }
            else
            {
                return commands;
            }
            return null;
        }

        public override string ToString()
        {
            return $"{CommandManagerType}";
        }
    }
}
