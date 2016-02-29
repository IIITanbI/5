namespace QA.AutomatedMagic.MetaMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlSourceResolver;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class MetaTypeAttribute : Attribute
    {
        public string Description { get; private set; }
        public string KeyName { get; private set; }
        public Type SourceResolverType { get; set; } = null;
        public Type ManagingFillerType { get; set; } = null;

        public MetaTypeAttribute(string description, string keyName = null)
        {
            Description = description;
            KeyName = keyName;
        }
    }
}
