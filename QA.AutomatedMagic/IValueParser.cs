using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QA.AutomatedMagic
{
    public interface IValueParser
    {
        bool IsMatch(Type type);
        object Parse(XObject source, Type type);
    }
}
