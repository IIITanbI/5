namespace QA.AutomatedMagic.MetaMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class MetaConstraintAttribute : Attribute
    {
        public string MemberName { get; private set; }
        public List<object> Values { get; private set; }
        public string Mark { get; set; } = null;

        public bool IsPositive { get; set; } = true;

        public MetaConstraintAttribute(string memberName, params object[] values)
        {
            MemberName = memberName;
            Values = values.ToList();
        }
    }
}
