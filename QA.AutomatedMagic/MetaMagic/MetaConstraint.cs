namespace QA.AutomatedMagic.MetaMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MetaConstraint
    {
        public Dictionary<string, List<ConstraintEntry>> Constraints { get; private set; }

        public MetaConstraint(MetaType metaType, List<MetaConstraintAttribute> constraintAttributes)
        {
            Constraints = new Dictionary<string, List<ConstraintEntry>>();

            foreach (var constraintAttribute in constraintAttributes)
            {
                var mark = constraintAttribute.Mark ?? Guid.NewGuid().ToString();

                if (!Constraints.ContainsKey(mark))
                    Constraints.Add(mark, new List<ConstraintEntry>());

                Constraints[mark].Add(new ConstraintEntry
                {
                    Values = constraintAttribute.Values,
                    Member = new Lazy<MetaTypeMember>(() => metaType.Members.First(m => m.Info.Name == constraintAttribute.MemberName)),
                    IsPositive = constraintAttribute.IsPositive
                });
            }
        }

        public class ConstraintEntry
        {
            public Lazy<MetaTypeMember> Member;
            public List<object> Values;
            public bool IsPositive;
        }
    }
}
