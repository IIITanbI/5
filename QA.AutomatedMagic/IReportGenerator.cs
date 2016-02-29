namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TestInfo;

    public interface IReportGenerator
    {
        void CreateReport(TestItem testItem, TestEnvironmentInfo testEnvironmentInfo, List<TestItem> previous);
    }
}
