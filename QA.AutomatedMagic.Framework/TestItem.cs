namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using MetaMagic;

    [MetaType("TestItem config")]
    public abstract class TestItem : Source
    {
        [MetaTypeValue("Name of TestItem")]
        public string Name { get; set; }

        [MetaTypeValue("Description for TestItem")]
        public string Description { get; set; }

        [MetaTypeObject("TestingContext config", IsRequired = false)]
        public TestContext Context { get; set; } = new TestContext();

        [MetaTypeCollection("List of TestSteps", "step", "testingStep", "testStep", IsRequired = false)]
        [MetaLocation("testSteps", "testingSteps")]
        public LinkedList<TestStep> Steps { get; set; } = new LinkedList<TestStep>();

        [MetaTypeValue("Number of tries for executing testItem", IsRequired = false)]
        [MetaLocation("retries")]
        public int RetryCount { get; set; } = 1;

        [MetaTypeCollection("List of tags for TestItem", "tag", IsRequired = false)]
        public List<string> Tags { get; set; } = new List<string>();

        [MetaTypeValue("Is TestItem enabled?", IsRequired = false)]
        [MetaLocation("enabled")]
        public bool IsEnabled { get; set; } = true;

        public TestItem ParentItem { get; protected set; }
        public TestLogger Log { get; set; }
        public TestItemStatus Status { get; set; } = TestItemStatus.NotExecuted;
        public TestItemType ItemType { get; set; } = TestItemType.Test;

        protected int _tryNumber = 1;

        public List<TestItem> FailedTries { get; set; } = new List<TestItem>();
        protected List<StepMeta> StepsMeta { get; set; } = new List<StepMeta>();

        public virtual List<TestItem> Build()
        {
            switch (ItemSourceType)
            {
                case SourceType.Xml:

                    Log = new TestLogger(Name, ItemType.ToString());
                    foreach (var step in Steps)
                    {
                        step.Build();
                    }
                    return new List<TestItem> { this };

                case SourceType.External:

                    var doc = XDocument.Load(Path);
                    var xml = doc.Element(RootElementName);
                    var testItem = MetaType.Parse<TestItem>(xml);

                    return testItem.Build();

                case SourceType.Generic:
                case SourceType.ExternalGeneric:

                    throw new NotImplementedException();

                default:
                    throw new NotImplementedException();
            }
        }

        private TestLogger _realLog = null;
        public void Execute()
        {
            if (_realLog == null)
            {
                Log.INFO($"Start executing {ItemType}: {Name}");
                Log.INFO($"Description: {Description}");
                ParentItem?.Log.INFO($"Start executing {ItemType}: {Name}");
                ParentItem?.Log.INFO($"Description: {Description}");

                try
                {
                    Log.DEBUG("Start initialization");
                    ExecuteStageInitialization();
                    Log.DEBUG("Initialization completed successfully");
                }
                catch (Exception ex)
                {
                    Log.ERROR("Error occurred during initialization", ex);
                    Log.ERROR($"Executing of {ItemType}: {Name}  completed with status: FAILED");
                    ParentItem?.Log.ERROR("Error occurred during initialization", ex);
                    ParentItem?.Log.ERROR($"Executing of {ItemType}: {Name}  completed with status: FAILED");
                    MarkAsFailedOrSkipped();
                    return;
                }

                _realLog = Log;
            }

            Log = new TestLogger(Name, ItemType.ToString());
            if (RetryCount > 1)
            {
                Log.INFO($"Start try #{_tryNumber} of {RetryCount} ");
            }

            Status = TestItemStatus.Unknown;
            StepsMeta.Clear();


            try
            {
                ExecuteStagePre();
                ExecuteStageCase();
            }
            catch (Exception ex)
            {
                if (_tryNumber < RetryCount)
                {
                    Log.WARN($"Error occurred during execution {ItemType}: {Name}", ex);
                }
                else
                {
                    if (RetryCount == 1)
                    {
                        ParentItem?.Log.ERROR($"Error occurred during execution {ItemType}: {Name}", ex);
                    }
                    Log.ERROR($"Error occurred during execution {ItemType}: {Name}", ex);
                }
                Status = TestItemStatus.Failed;
            }
            finally
            {
                try
                {
                    ExecuteStagePost();
                }
                catch (Exception ex)
                {
                    Log.WARN("Error occurred during Post steps execution", ex);
                }
                finally
                {
                    if (Status == TestItemStatus.Failed)
                    {
                        if (_tryNumber < RetryCount)
                        {
                            Log.WARN($"Try #{_tryNumber} of {RetryCount}  completed with error");
                            _tryNumber++;

                            var state = GetState();
                            FailedTries.Add(state);

                            Execute();
                        }
                        else
                        {
                            if (_tryNumber > 1)
                            {
                                Log.ERROR($"Try #{_tryNumber} of {RetryCount}  completed with error");
                            }

                            Log.SpamTo(_realLog);
                            Log = _realLog;


                            Log.ERROR($"Executing of {ItemType}: {Name}  completed with status: FAILED");
                            ParentItem?.Log.ERROR($"Executing of {ItemType}: {Name}  completed with status: FAILED");
                            MarkAsFailedOrSkipped();
                        }
                    }
                    else
                    {
                        Status = TestItemStatus.Passed;
                        Log.INFO($"Executing of {ItemType}: {Name}  completed with status: PASSED");
                        ParentItem?.Log.INFO($"Executing of {ItemType}: {Name}  completed with status: PASSED");

                        Log.SpamTo(_realLog);
                        Log = _realLog;
                    }
                }
            }
        }

        public virtual TestItem GetState()
        {
            var testItem = MetaType.CopyObjectWithCast(this);
            testItem.Log = Log;
            return testItem;
        }

        public virtual void ExecuteStageInitialization()
        {
            Log.TRACE("Start of Context initialization");
            Context.Initialize();
            Log.DEBUG("Context has been initialized");
        }

        public virtual void ExecuteStagePre()
        {
            var stepsToExecute = Steps.Where(step => step.StepOrder == TestStep.Order.Pre).ToList();
            Log.TRACE($"Found: {stepsToExecute.Count} Pre steps");
            if (stepsToExecute.Count > 0)
            {
                Log.TRACE("Start executing Pre steps");

                foreach (var step in stepsToExecute)
                {
                    ExecuteStep(step);
                }

                Log.TRACE("Pre steps have been executed");
            }
        }
        public virtual void ExecuteStageCase()
        {
            var stepsToExecute = Steps.Where(step => step.StepOrder == TestStep.Order.Case || step.StepOrder == TestStep.Order.CasePost).ToList();
            Log.TRACE($"Found: {stepsToExecute.Count} Case steps");
            if (stepsToExecute.Count > 0)
            {
                Log.TRACE("Start executing Case steps");

                foreach (var step in stepsToExecute)
                {
                    ExecuteStep(step);
                }

                Log.TRACE("Case steps have been executed");
            }
        }
        public virtual void ExecuteStagePost()
        {
            var stepsToExecute = Steps.Where(step => step.StepOrder == TestStep.Order.Post).ToList();
            Log.TRACE($"Found: {stepsToExecute.Count} Post steps");
            if (stepsToExecute.Count > 0)
            {
                Log.TRACE("Start executing Post steps");

                foreach (var step in stepsToExecute)
                {
                    ExecuteStep(step);
                }

                Log.TRACE("Post steps have been executed");
            }
        }
        public virtual void ExecuteStep(TestStep testStep)
        {
            Log.INFO($"Start executing TestStep: {testStep.Name}");
            Log.INFO($"Description: {testStep.Description}");

            var stepMeta = new StepMeta();
            stepMeta.Step = testStep;
            stepMeta.TestItemStatus = TestItemStatus.Unknown;
            stepMeta.Log = new TestLogger(testStep.Name, "Step");
            stepMeta.Log.SetParent(Log);
            StepsMeta.Add(stepMeta);

            for (int i = 1; i <= testStep.TryCount; i++)
            {
                try
                {
                    testStep.Execute(Context, stepMeta.Log);
                    stepMeta.TestItemStatus = TestItemStatus.Passed;
                }
                catch (Exception ex)
                {
                    if (i == testStep.TryCount)
                    {
                        if (i > 1)
                            stepMeta.Log.WARN($"Try: {i} of {testStep.TryCount} completed with error");

                        if (testStep.IsSkippedOnFail)
                        {
                            stepMeta.Log.WARN($"Step completed with error but skipped", ex);
                            stepMeta.TestItemStatus = TestItemStatus.Skipped;
                        }
                        else
                        {
                            stepMeta.TestItemStatus = TestItemStatus.Failed;
                            stepMeta.Log.ERROR($"Error occurred during step execution", ex);
                            stepMeta.Log.ERROR($"TestStep: {testStep.Name} completed with status: FAILED");
                            Log.ERROR($"Error occurred during executing TestStep: {testStep.Name}", ex);
                            Log.ERROR($"TestStep: {testStep.Name} completed with status: FAILED");
                            throw ex;
                        }
                    }
                    else
                    {
                        stepMeta.Log.WARN($"Try: {i} of {testStep.TryCount} completed with error", ex);
                        stepMeta.Log.WARN("Try again");
                    }
                }
            }
            if (stepMeta.TestItemStatus == TestItemStatus.Unknown)
            {
                stepMeta.TestItemStatus = TestItemStatus.Passed;
                Log.INFO($"TestStep: {testStep.Name} has been successfully completed with status: PASSED");
            }
        }

        public virtual void MarkAsFailedOrSkipped(TestItemStatus status = TestItemStatus.Failed)
        {
            if (Status == TestItemStatus.NotExecuted)
                Status = status;
            if (Status == TestItemStatus.Unknown)
                Status = TestItemStatus.Failed;
        }

        public virtual void SetParent(TestSuite parent)
        {
            if (parent != null)
            {
                ParentItem = parent;
                Context.ParentContext = parent.Context;
                MergeSteps(parent.Steps);
                Log.SetParent(parent.Log);
            }
        }
        public void MergeSteps(LinkedList<TestStep> parentSteps)
        {
            foreach (var step in parentSteps)
            {
                switch (step.StepOrder)
                {
                    case TestStep.Order.Pre:
                        break;

                    case TestStep.Order.Case:

                        var lastPre = Steps.LastOrDefault(cs => cs.StepOrder == TestStep.Order.Pre);
                        if (lastPre != null)
                            Steps.AddAfter(Steps.Find(lastPre), step);
                        else
                            Steps.AddFirst(step);

                        break;

                    case TestStep.Order.CasePost:

                        var firstPost = Steps.LastOrDefault(cs => cs.StepOrder == TestStep.Order.Post);
                        if (firstPost != null)
                            Steps.AddBefore(Steps.Find(firstPost), step);
                        else
                            Steps.AddLast(step);

                        break;

                    case TestStep.Order.Post:
                        break;
                    default:
                        throw new FrameworkException($"Unknown TestStepOrder: {step.StepOrder}");
                }
            }
        }

        public virtual TestInfo.TestItem GetReportItem()
        {
            var reportItem = new TestInfo.TestItem();
            reportItem.Name = Name;
            reportItem.Description = Description;
            reportItem.Status = Status;
            reportItem.Type = ItemType;

            foreach (var logMessage in Log.LogMessages)
            {
                reportItem.LogMessages.Add(logMessage);
            }

            foreach (var stepMeta in StepsMeta)
            {
                var reportStep = new TestInfo.Step();
                reportStep.Name = stepMeta.Step.Name;
                reportStep.Description = stepMeta.Step.Description;
                reportStep.Status = stepMeta.TestItemStatus;

                foreach (var stepMessage in stepMeta.Log.LogMessages)
                {
                    reportStep.Messages.Add(stepMessage);
                }

                reportItem.Steps.Add(reportStep);
            }

            foreach (var failedTry in FailedTries)
            {
                reportItem.FailedTries.Add(failedTry.GetReportItem());
            }

            return reportItem;
        }

        protected class StepMeta
        {
            public TestStep Step { get; set; }
            public TestItemStatus TestItemStatus { get; set; }
            public TestLogger Log { get; set; }
        }
    }
}
