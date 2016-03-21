namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    public class ParseException : Exception
    {
        private StringBuilder _sb = new StringBuilder();

        public ParseException(object source, MetaTypeMember member)
        {
            _sb.AppendLine($"Couldn't parse:\n{member}");
            _sb.AppendLine($"Location:\n{member.Location}");
            _sb.AppendLine($"Source:\n{source}");
            _sb.AppendLine($"StackTrace:\n{StackTrace}");
        }

        public ParseException(object source, Type type)
            : base($"Couldn't parse {type} from source:\n{source}")
        {

        }

        public ParseException(string message, object source, MetaTypeMember member)
        {
            _sb.AppendLine($"{message}");
            _sb.AppendLine($"Couldn't parse:\n{member}");
            _sb.AppendLine($"Location:\n{member.Location}");
            _sb.AppendLine($"Source:\n{source}");
            _sb.AppendLine($"StackTrace:\n{StackTrace}");
        }

        public override string ToString()
        {
            if (_sb.Length == 0)
                return base.ToString();
            return _sb.ToString();
        }
    }
}
