namespace QA.AutomatedMagic.Framework.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.IO;

    public class MagicSolution
    {
        public string Path { get; set; }
        public string Name { get; set; }

        public List<MagicProject> Projects { get; set; }

        public void Parse()
        {
            Name = System.IO.Path.GetFileNameWithoutExtension(Path);
            var lines = File.ReadAllLines(Path);
            var projectLines = lines.Where(l => l.StartsWith("Project")).ToList();

            Projects = new List<MagicProject>();
            foreach (var projectLine in projectLines)
            {
                var parts = projectLine.Split(new[] { " = \"", "\", \"" }, StringSplitOptions.RemoveEmptyEntries);
                var project = new MagicProject();
                project.Name = parts[1];
                project.Path = Path + "\\" + parts[2];
                project.Parse();
                Projects.Add(project);
            }
        }
    }
}
