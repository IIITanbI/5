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
        public ParseException(object source, MetaTypeMember member)
            : base($"Couldn't parse:\n{member}\nusing its location:\n{member.Location}\nfrom source:\n{source}")
        {

        }

        public ParseException(object source, Type type)
            : base($"Couldn't parse {type} from source:\n{source}")
        {

        }

        public ParseException(string message, object source, MetaTypeMember member)
            : base($"{message}\nCouldn't parse:\n{member}\nusing its location:\n{member.Location}\nfrom source:\n{source}")
        {

        }
    }
}
