namespace QA.AutomatedMagic.Framework.Core.MagicSolutionSources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading;
    using System.Diagnostics;
    using System.IO;

    public class MagicSolutionGitSource : MagicSolutionFolderSource
    {
        public string GitUrl { get; set; }

        public override void Prepare(string solutionsFolder)
        {
            CloneGitRepo();
            base.Prepare(solutionsFolder);
        }

        private void CloneGitRepo()
        {
            var processInfo = new ProcessStartInfo();
            processInfo.FileName = "git";
            processInfo.Arguments = $"clone --recursive {GitUrl} {SourceSolutionFolder}";
            processInfo.UseShellExecute = false;
            processInfo.CreateNoWindow = true;

            var process = new Process();
            process.StartInfo = processInfo;
            process.Start();
            process.WaitForExit();
        }
    }
}
