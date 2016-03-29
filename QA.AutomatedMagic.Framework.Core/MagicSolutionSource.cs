namespace QA.AutomatedMagic.Framework.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class MagicSolutionSource
    {
        public string SolutionFolderPath { get; set; }

        public abstract void Prepare(string solutionsFolder);

        public MagicSolution Parse()
        {
            var slnFile = Directory.GetFiles(SolutionFolderPath, "*.sln")[0];
            var solution = new MagicSolution { Path = slnFile };

            return solution;
        }
    }
}
