namespace QA.AutomatedMagic.MetaMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class MetaLocationAttribute : Attribute
    {
        public List<string> PossibleaNames { get; private set; }
        public bool CouldBeValue { get; private set; }

        public MetaLocationAttribute(bool couldBeValue, params string[] possibleNames)
        {
            CouldBeValue = couldBeValue;
            PossibleaNames = possibleNames.ToList();
        }

        public MetaLocationAttribute(params string[] possibleNames)
        {
            CouldBeValue = false;
            PossibleaNames = possibleNames.ToList();
        }
    }
}
