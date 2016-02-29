namespace QA.AutomatedMagic.CommandsMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CommandManagerAttribute : Attribute
    {
        public string Description { get; private set; }
        public Type ConfigType { get; private set; } = null;

        public CommandManagerAttribute(Type configType, string description)
        {
            ConfigType = configType;
            Description = description;
        }

        public CommandManagerAttribute(string description)
        {
            Description = description;
        }
    }
}
