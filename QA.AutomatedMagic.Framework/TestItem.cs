namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using MetaMagic;
    using TestLogger;

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

        public void Execute()
        {
            Status = TestItemStatus.Unknown;
            StepsMeta.Clear();
            var realLog = Log;
            Log = new TestLogger(Name, ItemType.ToString());

            Log.INFO($"Start executing {ItemType}: {Name}");
            Log.INFO($"Description: {Description}");
            Log.DEBUG("Start of initialization");
            try
            {
                ExecuteStageInitialization();
            }
            catch (Exception ex)
            {
                Log.ERROR("Error has occurred during initialization", ex);
                Log.SpamToLog(realLog);
                Log = realLog;
                MarkAsFailedOrSkipped();
            }
            Log.DEBUG("Initialization has been completed");


            if (RetryCount != 1)
            {
                realLog.DEBUG($"Start try {_tryNumber} of {RetryCount}");
            }

            try
            {
                ExecuteStagePre();
                ExecuteStageCase();
            }
            catch (Exception ex)
            {
                if (RetryCount == 1)
                {
                    Log.ERROR("Error occurred during step execution", ex);
                    Log.SpamToLog(realLog);
                    Log = realLog;
                    MarkAsFailedOrSkipped();
                }
                else
                {
                    Log.WARN("Error occurred during step execution", ex);
                }
            }
            finally
            {
                try
                {
                    ExecuteStagePost();

                    if (Status != TestItemStatus.Failed)
                    {
                        Log.SpamToLog(realLog);
                        Log = realLog;
                    }
                }
                catch (Exception ex)
                {
                    if (RetryCount == 1)
                    {
                        if (Status != TestItemStatus.Failed)
                        {
                            Log.ERROR("Error occurred during step execution", ex);
                            Log.SpamToLog(realLog);
                            Log = realLog;
                            MarkAsFailedOrSkipped();
                        }
                        else
                        {
                            Log.WARN("Error occurred during step execution", ex);
                        }
                    }
                }
                finally
                {
                    if (Status == TestItemStatus.Failed)
                    {
                        if (_tryNumber < RetryCount)
                        {
                            realLog.WARN($"Try {_tryNumber} of {RetryCount} completed with error. Try again");

                            FailedTries.Add(GetState());

                            Log = realLog;
                            _tryNumber++;
                            Execute();
                        }
                    }
                }
            }
        }

        public virtual TestItem GetState()
        {
            TestItem testItem = (TestItem)Activator.CreateInstance(GetType());

            testItem.Name = Name;
            testItem.Description = Description;
            testItem.Context.ContextValues = Context.ContextValues;
            testItem.Context.CommandManagersItems = Context.CommandManagersItems;
            testItem.ItemType = ItemType;
            testItem.Log = Log;
            testItem.StepsMeta = StepsMeta;

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
            stepMeta.Log.AddParent(Log);
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
                Log.AddParent(parent.Log);
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

            foreach (var logMessage in Log.Messages)
            {
                reportItem.LogMessages.Add(new TestInfo.LogMessage { Level = logMessage.Level, DataStemp = logMessage.Time, Message = logMessage.Message, Exception = logMessage.Ex });
            }

            foreach (var stepMeta in StepsMeta)
            {
                var reportStep = new TestInfo.Step();
                reportStep.Name = stepMeta.Step.Name;
                reportStep.Description = stepMeta.Step.Description;
                reportStep.Status = stepMeta.TestItemStatus;

                foreach (var stepMessage in stepMeta.Log.Messages)
                {
                    reportStep.Messages.Add(new TestInfo.LogMessage { Level = stepMessage.Level, DataStemp = stepMessage.Time, Message = stepMessage.Message, Exception = stepMessage.Ex });
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
