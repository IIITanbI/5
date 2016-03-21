namespace QA.AutomatedMagic.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;
    using CommandsMagic;

    [MetaType("Test step")]
    public class TestStep : TestStepBase
    {
        [MetaTypeValue("Manager")]
        public string Manager { get; set; }

        [MetaTypeValue("Command")]
        public string Command { get; set; }

        [MetaTypeCollection("List of argument for test step", "argument", "arg", IsRequired = false)]
        [MetaLocation("args")]
        public List<TestStepArgument> Arguments { get; set; } = new List<TestStepArgument>();

        private string _managerType;
        private string _managerName;
        private CommandManager _commandManager;
        private BaseCommandManager _manager;

        public override void Execute()
        {
            ItemStatus = TestItemStatus.Unknown;
            Log.INFO($"Start executing {this}");
            Parent.Log.INFO($"Start executing {this}");

            var argObjs = new List<object>();

            #region Resolve arguments
            foreach (var arg in Arguments)
            {
                switch (arg.Type)
                {
                    case TestStepArgumentType.Context:

                        object argObj = null;
                        try
                        {
                            argObj = Context.ResolveValue(arg.Value);
                        }
                        catch (Exception ex)
                        {
                            var ftte = new FrameworkTestExecutionException(this, "Error occurred during resolving Context value", ex,
                                $"Value to resolve: {arg.Value}");

                            ItemStatus = TestItemStatus.Failed;

                            Log.ERROR("Error occurred during resolving Context value", ftte);
                            Log.ERROR($"Execution of {this} completed with status: {ItemStatus}");
                            Parent.Log.ERROR($"Execution of {this} completed with status: {ItemStatus}", ftte);
                            return;
                        }

                        if (argObj == null)
                        {
                            var ftte = new FrameworkTestExecutionException(this, "Couldn't resolve Context value",
                                $"Value to resolve: {arg.Value}");

                            ItemStatus = TestItemStatus.Failed;

                            Log.ERROR("Couldn't resolve Context value", ftte);
                            Log.ERROR($"Execution of {this} completed with status: {ItemStatus}");
                            Parent.Log.ERROR($"Execution of {this} completed with status: {ItemStatus}", ftte);
                            return;
                        }

                        argObjs.Add(argObj);

                        break;
                    case TestStepArgumentType.Manager:

                        object managerObj = Context.ResolveManager(arg.Value);

                        if (managerObj == null)
                        {
                            var ftte = new FrameworkTestExecutionException(this, $"Couldn't resolve Context manager value",
                                $"Manager value: {arg.Value}");

                            ItemStatus = TestItemStatus.Failed;

                            Log.ERROR("Couldn't resolve Context manager value", ftte);
                            Log.ERROR($"Execution of {this} completed with status: {ItemStatus}");
                            Parent.Log.ERROR($"Execution of {this} completed with status: {ItemStatus}", ftte);
                            return;
                        }

                        argObjs.Add(managerObj);

                        break;
                    case TestStepArgumentType.Value:

                        argObjs.Add(arg.Value);

                        break;
                    case TestStepArgumentType.StepResult:

                        object stepResult = Context.ResolveStepResult(arg.Value);

                        if (stepResult == null)
                        {
                            var ftte = new FrameworkTestExecutionException(this, $"Couldn't resolve Context StepResult value",
                                $"Step name: {arg.Value}");

                            ItemStatus = TestItemStatus.Failed;

                            Log.ERROR("Couldn't resolve Context StepResult value", ftte);
                            Log.ERROR($"Execution of {this} completed with status: {ItemStatus}");
                            Parent.Log.ERROR($"Execution of {this} completed with status: {ItemStatus}", ftte);
                            return;
                        }

                        argObjs.Add(stepResult);

                        break;
                    default:
                        break;
                }
            }
            #endregion

            #region Found Command
            CommandExecutionInfo commandExecutionInfo = null;
            try
            {
                commandExecutionInfo = _commandManager.GetCommandExecutionInfo(_manager, Command, argObjs, Log);
            }
            catch (Exception ex)
            {
                ItemStatus = TestItemStatus.Failed;

                var ftee = new FrameworkTestExecutionException(this, "Error occurred during getting CommandExecutionInfo", ex,
                    $"Command name: {Command}",
                    $"Manager type: {_managerType}",
                    $"Manager name: {_managerName}");

                Log.ERROR("Error occurred during getting CommandExecutionInfo", ftee);
                Parent.Log.ERROR("Error occurred during getting CommandExecutionInfo", ftee);
                return;
            }

            if (commandExecutionInfo == null)
            {
                ItemStatus = TestItemStatus.Failed;

                var ftee = new FrameworkTestExecutionException(this, $"Couldn't find command with name {Command}",
                    $"Manager type: {_managerType}",
                    $"Manager name: {_managerName}");

                Log.ERROR($"Couldn't find command with name {Command}", ftee);
                Parent.Log.ERROR($"Couldn't find command with name {Command}", ftee);
                return;
            }
            #endregion

            #region Execute Command with tries
            object result = null;
            for (; _tryNumber < TryCount; _tryNumber++)
            {
                ItemStatus = TestItemStatus.Unknown;

                Log.DEBUG($"Start try #{_tryNumber} of {TryCount}");
                try
                {
                    result = commandExecutionInfo.Execute();
                    ItemStatus = TestItemStatus.Passed;
                }
                catch (Exception ex)
                {
                    Log.WARN($"Try #{_tryNumber} of {TryCount} completed with error. Try again", ex);
                }

                if (ItemStatus == TestItemStatus.Passed)
                    break;
            }

            if (ItemStatus != TestItemStatus.Passed)
            {
                Log.DEBUG($"Start try #{_tryNumber} of {TryCount}");
                try
                {
                    result = commandExecutionInfo.Execute();
                }
                catch (Exception ex)
                {
                    if (IsSkippedOnFail)
                    {
                        ItemStatus = TestItemStatus.Skipped;

                        Log.WARN($"Try #{_tryNumber} of {TryCount} completed with error", ex);
                        Log.WARN($"Execution of {this} completed with status: {ItemStatus}");
                        Parent.Log.WARN($"Execution of {this} completed with status: {ItemStatus}", ex);
                        return;
                    }
                    else
                    {
                        ItemStatus = TestItemStatus.Failed;

                        Log.ERROR($"Try #{_tryNumber} of {TryCount} completed with error", ex);
                        Log.ERROR($"Execution of {this} completed with status: {ItemStatus}");
                        Parent.Log.ERROR($"Execution of {this} completed with status: {ItemStatus}", ex);
                        return;
                    }
                }
            }
            #endregion

            if (result != null)
                AddStepResult(Info.Name, result);

            ItemStatus = TestItemStatus.Passed;

            Log.DEBUG($"Try #{_tryNumber} of {TryCount} was successfully completed");
            Log.INFO($"Execution of {this} completed with status: {ItemStatus}");
            Parent.Log.INFO($"Execution of {this} completed with status: {ItemStatus}");
        }

        public override void Build()
        {
            TestManager.Log.INFO($"Start building item: {this}");
            base.Build();

            Manager = Manager.Trim();
            Command = Command.Trim();

            if (Manager == "")
                throw new FrameworkBuildingException(this, "Manager bind is empty");

            var parts = Manager.Split('.');

            if (parts.Length > 2)
                throw new FrameworkBuildingException(this, "Manager bind contains more than two parts separated by dot '.'", $"Manager bind: {Manager}");

            _managerType = parts[0].Trim();
            _commandManager = AutomatedMagicManager.GetCommandManagerByTypeName(_managerType);

            if (_commandManager == null)
                throw new FrameworkBuildingException(this, $"Couldn't find manager descriptor with type name: {_managerType}");

            _managerName = parts.Length == 2
                ? parts[1].Trim()
                : _managerType;

            _manager = Context.GetManager(_managerType, _managerName);
            if (_manager == null)
                throw new FrameworkBuildingException(this, $"Couldn't find manager object in TestContext",
                    $"Manager type: {_managerType}",
                    $"Manager name: {_managerName}");

            TestManager.Log.INFO($"Build was successfully completed for item: {this}");
        }
    }
}
