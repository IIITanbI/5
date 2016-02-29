namespace QA.AutomatedMagic.MetaMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MetaTypeValueAttribute : MetaTypeMemberAttribute
    {
        public Type ValueParserType { get; set; } = null;
        public Type ValueManagingFillerType { get; set; } = null;

        public MetaTypeValueAttribute(string description)
            : base(description)
        {

        }
    }
}
