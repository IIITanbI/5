namespace QA.AutomatedMagic.MetaMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MetaLocation
    {
        public List<string> PossibleNames { get; private set; } = new List<string>();
        public bool CouldBeValue { get; set; } = false;

        public MetaLocation(List<MetaLocationAttribute> locationAttributes)
        {
            foreach (var locationAtt in locationAttributes)
            {
                Add(locationAtt.PossibleaNames);
                CouldBeValue = CouldBeValue || locationAtt.CouldBeValue;
            }
        }
        public MetaLocation(bool couldBeValue)
        {
            CouldBeValue = couldBeValue;
        }
        public MetaLocation(string possibleName)
        {
            Add(possibleName);
        }

        public void Add(List<string> possibleNames)
        {
            possibleNames.ForEach(Add);
        }
        public void Add(string possibleName)
        {
            if (!PossibleNames.Contains(possibleName))
                PossibleNames.Add(possibleName);

            var invariantName = char.IsLower(possibleName[0])
                ? char.ToUpper(possibleName[0]) + possibleName.Substring(1)
                : char.ToLower(possibleName[0]) + possibleName.Substring(1);

            if (!PossibleNames.Contains(invariantName))
                PossibleNames.Add(invariantName);
        }

        public override string ToString()
        {
            var possNames = "";
            PossibleNames.ForEach(pn => possNames += $"\t- {pn}\n");

            return $"Could be value? : {CouldBeValue}\nPossible Names:\n{possNames}";
        }
    }
}
