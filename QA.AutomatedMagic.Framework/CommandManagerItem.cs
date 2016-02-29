﻿namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using MetaMagic;

    [MetaType("ManagerConfig")]
    [MetaLocation("manager", "commandManager", "managerItem")]
    public class CommandManagerItem : BaseMetaObject
    {
        [MetaTypeValue("Manager type name")]
        [MetaLocation("type")]
        public string ManagerType { get; set; }

        [MetaTypeValue("Command manager name", IsRequired = false)]
        public string Name { get; set; } = null;

        [MetaTypeValue("Manager config")]
        [MetaLocation(true)]
        public XElement Config { get; set; }
    }
}
