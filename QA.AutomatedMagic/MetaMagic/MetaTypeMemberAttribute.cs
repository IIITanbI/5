namespace QA.AutomatedMagic.MetaMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class MetaTypeMemberAttribute : Attribute
    {
        public Type SourceResolverType { get; set; } = null;
        public bool IsRequired { get; set; } = true;
        public string Description { get; private set; }

        public MetaTypeMemberAttribute(string description)
        {
            Description = description;
        }
    }
}
