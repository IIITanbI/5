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

        public CommandManager(CommandManagerAttribute commandManagerAttribute, Type commandManagerType)
        {
            Description = commandManagerAttribute.Description;
            CommandManagerType = commandManagerType;
            CommandManagerConfigType = commandManagerAttribute.ConfigType;

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
    }
}
