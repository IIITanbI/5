namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FrameworkException : Exception
    {
        private StringBuilder _sb = new StringBuilder();

        public FrameworkException(TestItem item, string message, params string[] infos)
        {
            _sb.AppendLine($"TestItem with problem: {item.GetFullName()}");
            _sb.AppendLine($"Message: {message}");

            foreach (var info in infos)
            {
                _sb.AppendLine(info);
            }
        }

        public FrameworkException(TestItem item, string message, Exception innerException, params string[] infos)
            : this(item, message, infos)
        {
            _sb.AppendLine("Inner exception info:");
            _sb.AppendLine(innerException.ToString());
        }

        public FrameworkException(string message, Exception innerException, params string[] infos)
        { }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}
