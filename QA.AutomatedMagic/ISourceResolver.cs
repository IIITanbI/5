namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public interface ISourceResolver
    {
        ICollectionSourceResolver GetCollectionSourceReolver();
        IObjectSourceResolver GetObjectSourceResolver();
        IValueSourceResolver GetValueSourceResolver();
        string GetSourceNodeName(object obj);
    }
}
