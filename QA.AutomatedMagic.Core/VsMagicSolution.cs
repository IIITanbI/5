namespace QA.AutomatedMagic.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Visual studio Solution with AutomatedMagicAssemblies")]
    public class VsMagicSolution : BaseMetaObject
    {
        [MetaTypeValue("Path to solution folder")]
        public string Path { get; set; }

        [MetaTypeValue("Solution name")]
        public string Name { get; set; }

        [MetaTypeCollection("List of Projects")]
        public List<VsMagicProject> Projects { get; set; }

        [MetaTypeCollection("List of Projects")]
        public List<VsReference> References { get; set; }
    }
}
