namespace QA.AutomatedMagic.CommandsMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Reflection;


    public class CommandManager
    {
        public string Description { get; private set; }
        public Type CommandManagerType { get; private set; }
        public Type CommandManagerConfigType { get; private set; }

        public List<Command> Commands { get; private set; } = new List<Command>();
        public Lazy<List<CommandManager>> ChildManagers { get; private set; } = null;
        private Dictionary<Type, PropertyInfo> _childManagersProperties = null;

        public CommandManager(CommandManagerAttribute commandManagerAttribute, Type commandManagerType)
        {
            Description = commandManagerAttribute.Description;
            CommandManagerType = commandManagerType;
            CommandManagerConfigType = commandManagerAttribute.ConfigType;

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
            var childManagerProperties = props.Where(p => typeof(ICommandManager).IsAssignableFrom(p.PropertyType)).ToList();

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
                            var manager = ReflectionManager.GetCommandManagerByTypeName(childManagerProperty.PropertyType.Name);
                            list.Add(manager);
                        }

                        return list;
                    }
                );
            }
        }

        public object CreateInstance(object config)
        {
            if (CommandManagerConfigType == null)
            {
                return Activator.CreateInstance(CommandManagerType);
            }
            else
            {
                return Activator.CreateInstance(CommandManagerType, new object[] { config });
            }
        }

        public object ExecuteCommand(object managerObject, string commandName, List<string> parameters, IContext context, ILogger log)
        {
            var propInfos = new List<PropertyInfo>();
            var possibleCommands = FindCommand(commandName, propInfos);

            if (possibleCommands == null || possibleCommands.Count == 0)
                throw new NotImplementedException();

            var parObjs = new List<object>();
            foreach (var parameterString in parameters)
            {
                if (parameterString.StartsWith("$"))
                {
                    var objStr = context.ResolveBind(parameterString.Substring(2, parameterString.Length - 3));
                    var parObj = context.ResolveValue(objStr);
                    parObjs.Add(parObj);
                }
                else
                {
                    parObjs.Add(context.ResolveBind(parameterString));
                }
            }

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
                    if (paramInfo.ParameterType == typeof(IContext))
                    {
                        parArray.Add(context);
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

        public List<Command> FindCommand(string commandName, List<PropertyInfo> propInfos)
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
    }
}
