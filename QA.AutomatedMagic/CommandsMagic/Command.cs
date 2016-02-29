namespace QA.AutomatedMagic.CommandsMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class Command
    {
        public string Description { get; private set; }
        public List<string> PossibleNames { get; private set; }
        public ParameterInfo[] Parameters { get; private set; }
        public MethodInfo Method { get; private set; }

        public Command(CommandAttribute commandAttribute, MethodInfo methodInfo)
        {
            Description = commandAttribute.Description;
            PossibleNames = commandAttribute.Names;
            PossibleNames.Add(methodInfo.Name);
            Method = methodInfo;
            Parameters = methodInfo.GetParameters();
        }
    }
}
