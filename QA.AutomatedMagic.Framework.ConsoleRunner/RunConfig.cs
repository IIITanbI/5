namespace QA.AutomatedMagic.Framework.ConsoleRunner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Config for Test Run")]
    public class RunConfig : BaseMetaObject
    {
        [MetaTypeValue("Path to Project config xml")]
        public string PathToProjectConfig { get; set; }

        [MetaTypeValue("Need to copy libraries?")]
        public bool NeedToCopyLibraries { get; set; }

        [MetaTypeCollection("Paths to libraries", "path")]
        public List<string> LibrariesPaths { get; set; }
    }
}
