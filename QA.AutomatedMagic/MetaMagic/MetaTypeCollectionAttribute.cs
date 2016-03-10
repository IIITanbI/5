namespace QA.AutomatedMagic.MetaMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MetaTypeCollectionAttribute : MetaTypeObjectAttribute
    {
        public List<string> PossibleChildrenNames { get; private set; }
        public Type CollectionWrapperType { get; set; } = null;
        public Type ChildValueParserType { get; set; } = null;

        public MetaTypeCollectionAttribute(string description, params string[] possibleChildrenNames)
            : base(description)
        {
            PossibleChildrenNames = possibleChildrenNames.ToList();
        }
    }
}
