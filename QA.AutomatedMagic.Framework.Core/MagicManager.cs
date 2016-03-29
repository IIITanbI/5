namespace QA.AutomatedMagic.Framework.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MagicManager
    {
        public MagicManagerConfig Config { get; set; }

        public void Load()
        {

        }

        public void Prepare()
        {
            foreach (var link in Config.SolutionLinks)
            {
            }
        }
    }
}
