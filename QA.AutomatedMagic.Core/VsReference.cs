namespace QA.AutomatedMagic.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Reference to some Visual Studio solution")]
    public class VsReference : BaseMetaObject
    {
        [MetaTypeValue("Solution Name")]
        public string SolutionName { get; set; }

        [MetaTypeValue("Project Name")]
        public string ProjectName { get; set; }
    }
}
