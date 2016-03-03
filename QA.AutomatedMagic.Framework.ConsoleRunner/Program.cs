namespace QA.AutomatedMagic.Framework.ConsoleRunner
{
    using MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using System.IO;
    using System.Reflection;
    using Reports.HtmlReport;
    class Program
    {
        static string _libPath = "Libs";
        static List<string> _skippedExts = new List<string> { ".config", ".exe", ".manifest" };

        static void Main(string[] args)
        {
            _libPath = Path.Combine(Directory.GetCurrentDirectory(), _libPath);
            
            ReflectionManager.LoadAssemblies();
            ReflectionManager.LoadAssemblies(Directory.GetCurrentDirectory());

            var runConfigXml = XDocument.Load("RunConfig.xml");
            var runConfig = MetaType.Parse<RunConfig>(runConfigXml.Elements().First());

            foreach (var path in runConfig.LibrariesPaths)
            {
                CopyLibraries(path);
            }

            ReflectionManager.LoadAssemblies(_libPath, true);

            var projectConfig = XDocument.Load(runConfig.PathToProjectConfig);
            var project = MetaType.Parse<TestProject>(projectConfig.Elements().First());
            project = (TestProject)project.Build().First();
            project.Execute();
            
            var result = project.GetReportItem();

            var rg = new HtmlReportGenerator("out.html");
            rg.CreateReport(result, new TestInfo.TestEnvironmentInfo(), null);
        }

        static void CopyLibraries(string pathToLibraryFolder)
        {
            var di = new DirectoryInfo(pathToLibraryFolder);
            var files = di.GetFiles("*.*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                if (_skippedExts.Contains(file.Extension))
                    continue;

                var relativePath = file.FullName.Replace(di.FullName, "");
                var newPath = _libPath + relativePath;
                var locarDir = Path.GetDirectoryName(newPath);

                if (!Directory.Exists(locarDir))
                    Directory.CreateDirectory(locarDir);

                if(!File.Exists(newPath))
                    file.CopyTo(newPath);
            }
        }
    }
}
