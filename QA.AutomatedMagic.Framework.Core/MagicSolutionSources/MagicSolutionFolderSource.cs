namespace QA.AutomatedMagic.Framework.Core.MagicSolutionSources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.IO;

    public class MagicSolutionFolderSource : MagicSolutionSource
    {
        public string SourceSolutionFolder { get; set; }

        public override void Prepare(string solutionsFolder)
        {
            var sourceDi = new DirectoryInfo(SourceSolutionFolder);
            var targetDi = new DirectoryInfo(solutionsFolder + "\\" + sourceDi.Name);

            var sourceFiles = sourceDi.GetFiles("*.*", SearchOption.AllDirectories).ToList();
            foreach (var file in sourceFiles)
            {
                var targetPath = targetDi.FullName + file.FullName.Replace(sourceDi.FullName, "");
                var targetFile = new FileInfo(targetPath);
                targetFile.Directory.Create();
                file.CopyTo(targetFile.FullName);
            }

            SolutionFolderPath = targetDi.FullName;
        }
    }
}
