namespace QA.AutomatedMagic.MagicServer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using MetaMagic;

    [MetaType("Magic solution")]
    public class MagicSolution : BaseMetaObject
    {
        [MetaTypeValue("Path to solution")]
        public string Path { get; set; }

        [MetaTypeValue("Solution name")]
        public string Name { get; set; }

        [MetaTypeCollection("List of Magic projects")]
        public List<MagicProject> Projects { get; set; } = new List<MagicProject>();

        [MetaTypeCollection("List of referenced solutions", IsRequired = false)]
        public List<MagicSolution> References { get; set; } = new List<MagicSolution>();

        public void Init()
        {
            Projects.ForEach(p => p.Solution = this);

            for (int i = 0; i < References.Count; i++)
            {
                References[i] = SolutionManager.Storage.MagicSolutions.First(s => s.Name == References[i].Name);
            }
        }
    }
}