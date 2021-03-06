﻿namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("Test context manager item")]
    public class TestContextManagerItem : BaseMetaObject
    {
        [MetaTypeValue("Manager type")]
        [MetaLocation("type")]
        public string ManagerType { get; set; }

        [MetaTypeValue("Manager name", IsRequired = false)]
        public string Name { get; set; } = null;

        [MetaTypeObject("Manager configuration object", IsRequired = false, IsAssignableTypesAllowed = true)]
        [MetaLocation("config")]
        public IMetaObject ManagerConfig { get; set; } = null;
    }
}
