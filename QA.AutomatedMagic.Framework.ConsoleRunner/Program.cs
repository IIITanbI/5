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
    using TestInfo;
    class Program
    {
        static string _libPath = "Libs";
        static List<string> _skippedExts = new List<string> { ".config", ".exe", ".manifest" };

        static void Main(string[] args)
        {
            try
            {
                _libPath = Path.Combine(Directory.GetCurrentDirectory(), _libPath);

                AutomatedMagicManager.LoadAssemblies();
                AutomatedMagicManager.LoadAssemblies(Directory.GetCurrentDirectory());

                var runConfigXml = XDocument.Load("RunConfig.xml");
                var runConfig = MetaType.Parse<RunConfig>(runConfigXml.Elements().First());

                if (runConfig.NeedToCopyLibraries)
                {
                    if (Directory.Exists(_libPath))
                        Directory.Delete(_libPath, true);

                    foreach (var path in runConfig.LibrariesPaths)
                    {
                        CopyLibraries(path);
                    }
                }

                AutomatedMagicManager.LoadAssemblies(_libPath, SearchOption.AllDirectories);
                TestManager.Log.AddLogger(new FileLogger("Log\\ManagerLog.txt"), LogLevel.TRACE);
                TestManager.Log.INFO(AutomatedMagicManager.GetReflectionInfo());


                var projectConfig = XDocument.Load(runConfig.PathToProjectConfig);
                var project = MetaType.Parse<TestProject>(projectConfig.Elements().First());
                project.Build();

                try
                {
                    project.Execute();
                }
                catch (Exception ex)
                {
                    TestManager.Log.ERROR("Error occurred during project executing", ex);
                }

                var ts = AutomatedMagicManager.LoadedMetaTypes;

                var result = project.GetTestInfo();
                var xel = MetaType.SerializeObject(result) as XElement;
                xel.Save("result.xml");

                var rg = new HtmlReportGenerator("out.html");
                rg.CreateReport(result, project.EnvironmentInfo, null);
            }
            catch (Exception ex)
            {
                TestManager.Log.ERROR("Error occurred during project building", ex);
            }
            Console.ReadLine();
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

                if (!File.Exists(newPath))
                    file.CopyTo(newPath);
            }
        }

        public class FileLogger : ILogger
        {
            private string _fileName;
            private object _lock = new object();

            public FileLogger(string fileName)
            {
                _fileName = fileName;
            }
            public void AddLogger(ILogger logger, LogLevel level)
            {
            }

            #region Simple actions
            public void TRACE(string message, Exception exception = null)
            {
                LOG(LogLevel.TRACE, message, exception);
            }
            public void TRACE(string message, LoggedFileType fileType, string filePath)
            {
                LOG(LogLevel.TRACE, message, fileType, filePath);
            }

            public void DEBUG(string message, Exception exception = null)
            {
                LOG(LogLevel.DEBUG, message, exception);
            }
            public void DEBUG(string message, LoggedFileType fileType, string filePath)
            {
                LOG(LogLevel.DEBUG, message, fileType, filePath);
            }

            public void WARN(string message, Exception exception = null)
            {
                LOG(LogLevel.WARN, message, exception);
            }
            public void WARN(string message, LoggedFileType fileType, string filePath)
            {
                LOG(LogLevel.WARN, message, fileType, filePath);
            }

            public void INFO(string message, Exception exception = null)
            {
                LOG(LogLevel.INFO, message, exception);
            }
            public void INFO(string message, LoggedFileType fileType, string filePath)
            {
                LOG(LogLevel.INFO, message, fileType, filePath);
            }

            public void ERROR(string message, Exception exception = null)
            {
                LOG(LogLevel.ERROR, message, exception);
            }
            public void ERROR(string message, LoggedFileType fileType, string filePath)
            {
                LOG(LogLevel.ERROR, message, fileType, filePath);
            }
            #endregion

            public void LOG(LogLevel level, string message, Exception exception = null)
            {
                lock (_lock)
                {
                    using (var sw = new StreamWriter(_fileName, true))
                    {
                        if (exception == null)
                            sw.WriteLine($"{level}\t{message}");
                        else
                            sw.WriteLine($"{level}\t{message}\nException:\n{exception}");
                        sw.Flush();
                    }
                }
            }
            public void LOG(LogLevel level, string message, LoggedFileType fileType, string filePath)
            {
            }

            public string GetLoggedFilesFolder()
            {
                return null;
            }
        }
    }
}
