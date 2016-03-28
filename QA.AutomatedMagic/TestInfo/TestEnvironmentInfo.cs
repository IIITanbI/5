namespace QA.AutomatedMagic.TestInfo
{
    using MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [MetaType("TestEnvironmentInfo")]
    public class TestEnvironmentInfo : BaseMetaObject
    {
        [MetaTypeValue("CLRVersion", IsRequired = false)]
        public string CLRVersion { get; set; }

        [MetaTypeValue("OSName", IsRequired = false)]
        public string OSName { get; set; }

        [MetaTypeValue("OSVersion", IsRequired = false)]
        public string OSVersion { get; set; }

        [MetaTypeValue("Platform", IsRequired = false)]
        public string Platform { get; set; }

        [MetaTypeValue("MachineName", IsRequired = false)]
        public string MachineName { get; set; }

        [MetaTypeValue("User", IsRequired = false)]
        public string User { get; set; }

        [MetaTypeValue("UserDomain", IsRequired = false)]
        public string UserDomain { get; set; }

    }
}
