namespace QA.AutomatedMagic.Framework.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommandsMagic;
    using System.Threading;

    [CommandManager("FrameworkManager")]
    public class FrameworkManager : BaseCommandManager
    {
        [Command("Sleep")]
        public void Sleep(string seconds, ILogger log)
        {
            log?.INFO($"Sleep for: {seconds} seconds");
            Thread.Sleep(int.Parse(seconds) * 1000);
        }
    }
}
