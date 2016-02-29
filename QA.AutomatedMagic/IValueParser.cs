using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.AutomatedMagic
{
    public interface IValueParser
    {
        bool IsMatch(Type type);
        object Parse(object source, Type type);
    }
}
