namespace QA.AutomatedMagic.MetaMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MetaTypeObjectAttribute : MetaTypeMemberAttribute
    {
        public bool IsAssignableTypesAllowed { get; set; } = false;

        public MetaTypeObjectAttribute(string description)
            : base(description)
        {

        }
    }
}
