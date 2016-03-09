namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;
    using System.Text.RegularExpressions;

    [MetaType("Test step config")]
    [MetaLocation("step", "testingStep")]
    public class TestStep : BaseMetaObject
    {
        [MetaTypeValue("Name of TestStep")]
        public string Name { get; set; }

        [MetaTypeValue("Description for TestStep")]
        public string Description { get; set; }

        [MetaTypeValue("Order of TestStep. Pre, Case, CasePost or Post", IsRequired = false)]
        [MetaLocation("order")]
        public Order StepOrder { get; set; } = Order.Case;

        [MetaTypeValue("Is step skipped on fail", IsRequired = false)]
        [MetaLocation("skipOnFail")]
        public bool IsSkippedOnFail { get; set; } = false;

        [MetaTypeValue("Is TestStep enabled?", IsRequired = false)]
        [MetaLocation("enabled")]
        public bool IsEnabled { get; set; } = true;

        [MetaTypeValue("Number of tries for the step execution", IsRequired = false)]
        [MetaLocation("retries")]
        public int TryCount { get; set; } = 1;

        [MetaTypeValue("TestStep phrase")]
        [MetaLocation(true)]
        public string Phrase { get; set; }

        public string Manager { get; private set; }
        public string CommandName { get; private set; }
        public List<string> Parameters { get; private set; } = new List<string>();

        public void Execute(TestContext context, TestLogger log)
        {
            log.INFO($"Start executing of TestStep: {Name}");
            log.INFO($"Description: {Description}");

            var managerParts = Manager.Split('.');
            var managerTypeName = managerParts[0];

            string managerName = null;
            if (managerParts.Length == 2)
                managerName = managerParts[1];
            else if (managerParts.Length > 2)
                throw new FrameworkException($"Unexpected Manager: {Manager}");

            var manager = AutomatedMagicManager.GetCommandManagerByTypeName(managerTypeName);
            var managerObj = context.Managers[managerTypeName][managerName ?? managerTypeName];

            var result = manager.ExecuteCommand(managerObj, CommandName, Parameters, context, log);

            if (result != null)
                context.Add($"Step.{Name}", result);
        }

        public static Regex KeyWordRegex = new Regex(@"((?:\$?\{.*?\})(?:[^{]*\})*)", RegexOptions.Compiled);
        private string TrimBraces(string value)
        {
            if (value.StartsWith("$"))
                return value;
            else return value.Substring(1, value.Length - 2);
        }
        public bool IsBuilded { get; set; } = false;
        public void Build()
        {
            if (IsBuilded) return;

            var matches = KeyWordRegex.Matches(Phrase);
            if (matches.Count == 0)
                throw new FrameworkException($"Couldn't parse test step from phrase: {Phrase}. There are no required marks");

            if (matches.Count < 2)
                throw new FrameworkException($"Couldn't parse test step from phrase: {Phrase}. There are not enough required marks");

            Manager = TrimBraces(matches[0].Value);
            CommandName = TrimBraces(matches[1].Value);

            for (int i = 2; i < matches.Count; i++)
            {
                Parameters.Add(TrimBraces(matches[i].Value));
            }

            IsBuilded = true;
        }

        public enum Order
        {
            Pre, Case, CasePost, Post
        }
    }
}
