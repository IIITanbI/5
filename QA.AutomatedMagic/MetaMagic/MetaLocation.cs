namespace QA.AutomatedMagic.MetaMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MetaLocation
    {
        public List<string> PossibleNames { get; set; } = new List<string>();
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
            PossibleNames.Add(possibleName);
        }
    }
}
