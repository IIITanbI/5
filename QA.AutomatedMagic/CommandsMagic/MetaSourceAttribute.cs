namespace QA.AutomatedMagic.CommandsMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MetaSourceAttribute : Attribute
    {
        public string Path { get; private set; }
        public string RootElementXPath { get; private set; }

        public MetaSourceAttribute(string path, string rootElementXPath = "*")
        {
            Path = path;
            RootElementXPath = rootElementXPath;
        }
    }
}
