namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;
    using System.Management;

    [MetaType("Test project")]
    public class TestProject : TestSuite
    {
        public TestProject()
        {
            ItemType = TestItemType.Project;
        }

        public override void Build()
        {
            base.Build();
            ClearSteps();
            BuildSteps();
        }

        public override void Execute()
        {
            EnvironmentInfo = new TestInfo.TestEnvironmentInfo
            {
                User = Environment.UserName,
                UserDomain = Environment.UserDomainName,
                MachineName = Environment.MachineName,
                OSVersion = Environment.OSVersion.Version.ToString(),
                Platform = Environment.OSVersion.Platform.ToString(),
                CLRVersion = Environment.Version.ToString()
            };

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
            foreach (ManagementObject os in searcher.Get())
            {
                EnvironmentInfo.OSName = os["Caption"].ToString();
                break;
            }

            SWatch.Start();
            base.Execute();
            SWatch.Stop();
        }

        public TestInfo.TestEnvironmentInfo EnvironmentInfo { get; private set; }
    }
}
