namespace QA.AutomatedMagic.CommandsMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CommandAttribute : Attribute
    {
        public List<string> Names { get; private set; }
        public string Description { get; set; }

        public CommandAttribute(string description, params string[] names)
        {
            Description = description;
            Names = names.ToList();
        }
    }
}
