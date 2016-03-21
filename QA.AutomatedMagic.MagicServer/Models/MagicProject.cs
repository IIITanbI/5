namespace QA.AutomatedMagic.MagicServer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using MetaMagic;

    [MetaType("Magic Project")]
    public class MagicProject : BaseMetaObject
    {
        [MetaTypeValue("Project name")]
        public string Name { get; set; }

        public MagicSolution Solution { get; set; }
    }
}