namespace QA.AutomatedMagic.MagicServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using MetaMagic;
    using Models;

    [MetaType("Magic storage")]
    public class MagicStorage : BaseMetaObject
    {
        [MetaTypeCollection("List of Magic solutions")]
        public List<MagicSolution> MagicSolutions { get; set; }
    }
}