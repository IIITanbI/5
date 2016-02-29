namespace QA.AutomatedMagic.CommandMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CommandManagerAttribute : Attribute
    {
        public List<string> Names { get; private set; }
        public string Description { get; set; }
        public Type ConfigType { get; private set; }
        public CommandManagerAttribute(Type configType, params string[] names)
        {
            Names = names.ToList();
            ConfigType = configType;
        }

        public CommandManagerAttribute(params string[] names)
        {
            Names = names.ToList();
            ConfigType = null;
        }
    }
}
