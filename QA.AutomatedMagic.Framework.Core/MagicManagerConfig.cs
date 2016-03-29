namespace QA.AutomatedMagic.Framework.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MagicManagerConfig
    {
        public List<MagicSolutionSource> SolutionLinks { get; set; }

        public string NugetFolder { get; set; }
        public string SolutionsFolder { get; set; }
    }
}
