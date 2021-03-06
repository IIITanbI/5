﻿namespace QA.AutomatedMagic.MetaMagic
{
    using MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [MetaType("Base interface for MetaObjects")]
    public interface IMetaObject
    {
        List<string> GetPaths();
        object ResolvePath(string path);
        void MetaInit();
    }
}
