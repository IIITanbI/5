namespace QA.AutomatedMagic.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Visual studio project compiled into AutomatedMagicAssembly")]
    public class VsMagicProject : BaseMetaObject
    {
        [MetaTypeValue("Project name")]
        public string Name { get; set; }

        public VsMagicSolution Solution { get; set; }
    }
}
