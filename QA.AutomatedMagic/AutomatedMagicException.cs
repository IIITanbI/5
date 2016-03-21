namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Reflection;

    public class AutomatedMagicException : Exception
    {
        private StringBuilder _sb = new StringBuilder();

        public AutomatedMagicException(string message, Exception innerException = null)
            : base(message, innerException)
        {

        }

        public AutomatedMagicException(string message, Type type, MemberInfo memberInfo, Exception innerException = null)
        {
            _sb.AppendLine(message);
            _sb.AppendLine($"Type: {type}");
            _sb.AppendLine($"Member: {memberInfo}");
            _sb.AppendLine($"StackTrace: {StackTrace}");
            _sb.AppendLine($"Inner Exception:\n{innerException}");
        }

        public override string ToString()
        {
            if (_sb.Length == 0)
                return base.ToString();
            else return _sb.ToString();
        }
    }
}
