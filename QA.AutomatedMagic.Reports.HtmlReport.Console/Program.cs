namespace QA.AutomatedMagic.Reports.HtmlReport.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TestInfo;
    using MetaMagic;
    using System.Xml.Linq;

    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Args:");
            args.ToList().ForEach(a => Console.WriteLine(a));
            try
            {
                var resultXml = args[0];
                var outFileName = args[1];

                Console.WriteLine($"Source xml report: {resultXml}");
                Console.WriteLine($"Out html: {outFileName}");

                AutomatedMagicManager.LoadAssemblies();
                AutomatedMagicManager.LoadAssemblies(Environment.CurrentDirectory);

                var buildOneFile = false;
                if (args.Length == 3)
                    buildOneFile = args[2].ToLower() == "true";

                var testItem = MetaType.Parse<TestItem>(XDocument.Load(resultXml).Elements().First());
                var rg = new HtmlReportGenerator(outFileName);
                rg.BuildInOneFile = buildOneFile;
                rg.CreateReport(testItem, testItem.EnvironmentInfo, null);

                Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.WriteLine("Press any key...");
                Console.ReadLine();
            }
        }
    }
}
