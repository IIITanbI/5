namespace QA.AutomatedMagic.Reports.HtmlReport
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using TestInfo;
    using QA.AutomatedMagic.MetaMagic;

    public class HtmlReportGenerator : IReportGenerator
    {
        private string Path;

        public HtmlReportGenerator(string path)
        {
            this.Path = path;            
        }

        public void CreateReport(TestItem testItem, TestEnvironmentInfo testEnvironmentInfo, List<TestItem> previous)
        {
            var html = new XElement("html",
                new XAttribute("lang", "en"),
                GetHead(),
                GetBody(testItem, testEnvironmentInfo)
            );

            html.Save(Path);
        }


        public XElement GetHead()
        {
            var head = new XElement("head");

            var charset = new XElement("meta", new XAttribute("charset", "utf-8"));

            var httpEquiv = new XElement("meta",
                new XAttribute("http-equiv", "X-UA-Compatible"),
                new XAttribute("content", "IE=edge")
            );

            var name = new XElement("meta",
                new XAttribute("name", "viewport"),
                new XAttribute("content", "width=device-width, initial-scale=1")
            );

            var title = new XElement("title", "Test result");
            var css = new XElement("link",
                new XAttribute("rel", "stylesheet"),
                new XAttribute("href", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css")
            );

            var css1 = new XElement("link",
                new XAttribute("rel", "stylesheet"),
                new XAttribute("href", "css/css.css")
            );

            head.Add(charset, httpEquiv, name, title, css, css1);
            return head;
        }

        public XElement GetBody(TestItem testItem, TestEnvironmentInfo testEnvironmentInfo)
        {
            var body = new XElement("body");

            var js = new XElement("script", "", new XAttribute("src", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"));

            //var jQuery = new XElement("script", "", new XAttribute("src", "D:/Visual Studio 2015/Projects/QA/TestConsole/jquery-1.12.0.min.js"));

            //var jsCustom = new XElement("script", "", new XAttribute("src", "D:/Visual Studio 2015/Projects/QA/TestConsole/custom.js"));

            var jQuery = new XElement("script", "", new XAttribute("src", "https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"));

            var jsCustom = new XElement("script", "", new XAttribute("src", "filter js/init.js"));
            var jsCustom1 = new XElement("script", "", new XAttribute("src", "filter js/filter.js"));
            var jsCustom2 = new XElement("script", "", new XAttribute("src", "filter js/testFilter.js"));
            var jsCustom3 = new XElement("script", "", new XAttribute("src", "filter js/logFilter.js"));
            var jsCustom4 = new XElement("script", "", new XAttribute("src", "filter js/stepFilter.js"));

            var container = new XElement("div", new XAttribute("class", "container"),
                GetEnvironment(testEnvironmentInfo),
                GetReport(testItem)
            );

            body.Add(container, jQuery, js, jsCustom, jsCustom1, jsCustom2, jsCustom3, jsCustom4);

            return body;
        }

        public XElement GetEnvironment(TestEnvironmentInfo testEnvironmentInfo)
        {
            var environment = new XElement("div", new XAttribute("class", "panel panel-primary environment"));

            var heading = new XElement("div", "Environment", new XAttribute("class", "panel-heading"));

            var table = new XElement("table", new XAttribute("class", "table"));

            var thead = new XElement("thead",
                new XElement("tr",
                    new XElement("th", "CLR version"),
                    new XElement("th", "OS name"),
                    new XElement("th", "OS version"),
                    new XElement("th", "Platform"),
                    new XElement("th", "Machine name"),
                    new XElement("th", "User"),
                    new XElement("th", "User domain")
                )
            );

            var tbody = new XElement("tbody",
                new XElement("tr",
                    new XElement("td", testEnvironmentInfo.CLRVersion),
                    new XElement("td", testEnvironmentInfo.OSName),
                    new XElement("td", testEnvironmentInfo.Platform),
                    new XElement("td", testEnvironmentInfo.MachineName),
                    new XElement("td", testEnvironmentInfo.User),
                    new XElement("td", testEnvironmentInfo.UserDomain)
                )
            );

            table.Add(thead, tbody);

            environment.Add(heading, table);
            return environment;
        }

        public XElement GetOverall(TestItem testItem)
        {
            if (testItem.Type == TestItemType.Test)
                return null;

            var table = new XElement("table", new XAttribute("class", "table overall"));

            var thead = new XElement("thead",
                new XElement("tr",
                    new XAttribute("class", "test-fltr-btns"),
                    new XElement("th", new XElement("button", "Total", new XAttribute("class", "btn test-fltr-btn-all"), new XAttribute("filter", "passed failed skipped"))),
                    new XElement("th", new XElement("button", "Passed", new XAttribute("class", "btn test-fltr-btn-psd"), new XAttribute("filter", "passed"))),
                    new XElement("th", new XElement("button", "Failed", new XAttribute("class", "btn test-fltr-btn-fld"), new XAttribute("filter", "failed"))),
                    new XElement("th", new XElement("button", "Skipped", new XAttribute("class", "btn test-fltr-btn-skpd"), new XAttribute("filter", "skipped")))
                )
            );

            var tbody = new XElement("tbody",
                new XElement("tr",
                    new XElement("td", testItem.GetTotal()),
                    new XElement("td", testItem.GetWithStatus(TestItemStatus.Passed)),
                    new XElement("td", testItem.GetWithStatus(TestItemStatus.Failed)),
                    new XElement("td", testItem.GetWithStatus(TestItemStatus.Skipped))
                )
            );

            table.Add(thead, tbody);

            return table;
        }

        public string GetContainerColor(TestItemStatus testItemStatus)
        {
            switch (testItemStatus)
            {
                case TestItemStatus.NotExecuted:
                    return "info";
                case TestItemStatus.Unknown:
                    return "info";
                case TestItemStatus.Passed:
                    return "success";
                case TestItemStatus.Failed:
                    return "danger";
                case TestItemStatus.Skipped:
                    return "warning";
                default:
                    return "default";
            }
        }

        public string GetLogColor(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.TRACE:
                    return "primary";
                case LogLevel.DEBUG:
                    return "success";
                case LogLevel.WARN:
                    return "warning";
                case LogLevel.INFO:
                    return "info";
                case LogLevel.ERROR:
                    return "danger";
                default:
                    return "info";
            }
        }

        public XElement GetTestExpander(TestItem testItem)
        {
            XElement btn = null;
            if (testItem.Childs.Count != 0)
            {
                btn = new XElement("button", new XAttribute("class", "btn btnexp btn-warning"), testItem.Childs[0].Type.ToString() + "s");
            }

            return btn;
        }
        public XElement GetStepExpander(TestItem testItem)
        {
            XElement btn = null;
            if (testItem.Steps.Count != 0)
            {
                btn = new XElement("button", new XAttribute("class", "btn btnstep btn-warning"), "Steps");
            }

            return btn;
        }
        public XElement GetLogExpander(BaseMetaObject obj)
        {
            if (!(obj is TestItem || obj is Step))
                return null;

            string name = (obj as TestItem)?.Type.ToString() ?? (obj as Step)?.Name;
            XElement btn = new XElement("button", new XAttribute("class", "btn btnlog btn-info"), name + " logs");
            return btn;
        }

        public XElement GetException(LogMessage logMessage)
        {
            XElement log = null;
            if (logMessage.Exception != null)
            {
                log = new XElement("div", $"Exception: {logMessage.Exception}", new XAttribute("class", "log-exception"));
            }
            else
            {
                log = new XElement("div", "");
            }
            return log;
        }
        public XElement GetMessage(LogMessage logMessage)
        {
            XElement msg = null;
            if (logMessage.Message != null)
            {
                msg = new XElement("div", $"Message: {logMessage.Message}", new XAttribute("class", "log-message"));
            }
            else
            {
                msg = new XElement("div", "");
            }
            return msg;
        }

        public XElement GetLogButtons()
        {
            //TRACE, DEBUG, WARN, INFO, ERROR
            var btns = new XElement("div",
                new XAttribute("class", "log-fltr-btns"),
                new XElement("button", "Trace", new XAttribute("class", "btn btn-xs log-fltr-btn-trc"), new XAttribute("filter", "trace debug warn info error")),
                new XElement("button", "Debug", new XAttribute("class", "btn btn-xs log-fltr-btn-dbg"), new XAttribute("filter", "debug warn info error")),
                new XElement("button", "Warn", new XAttribute("class", "btn btn-xs log-fltr-btn-wrn"), new XAttribute("filter", "warn info error")),
                new XElement("button", "Info", new XAttribute("class", "btn btn-xs log-fltr-btn-inf"), new XAttribute("filter", "info error")),
                new XElement("button", "Error", new XAttribute("class", "btn btn-xs log-fltr-btn-err"), new XAttribute("filter", "error"))
            );

            return btns;
        }
        public XElement GetStepButtons(TestItem testItem)
        {
            if (testItem.Steps.Count == 0)
                return null;

            //NotExecuted, Unknown, Passed, Failed, Skipped
            var bdiv = new XElement("div",
                new XAttribute("class", "step-fltr-btns"),
                new XElement("button", "All", new XAttribute("class", "btn step-fltr-btn-all"), new XAttribute("filter", "passed failed skipped unknown")),
                new XElement("button", "NotExecuted", new XAttribute("class", "btn step-fltr-btn-notexctd"), new XAttribute("filter", "notexecuted")),
                new XElement("button", "Passed", new XAttribute("class", "btn step-fltr-btn-psd"), new XAttribute("filter", "passed")),
                new XElement("button", "Failed", new XAttribute("class", "btn step-fltr-btn-fld"), new XAttribute("filter", "failed")),
                new XElement("button", "Skipped", new XAttribute("class", "btn step-fltr-btn-skpd"), new XAttribute("filter", "skipped")),
                new XElement("button", "Unknown", new XAttribute("class", "btn step-fltr-btn-unkwn"), new XAttribute("filter", "unknown"))
            );

            return bdiv;
        }


        public XElement GetLogs(BaseMetaObject obj)
        {
            if (!(obj is TestItem || obj is Step))
                return null;

            string name = (obj as TestItem)?.Name ?? (obj as Step)?.Name;
            List<LogMessage> messages = (obj as TestItem)?.LogMessages ?? (obj as Step)?.Messages;

            var main = new XElement("div", new XAttribute("class", "logPanel"));
            var elem = new XElement("div", new XAttribute("class", "logs"));

            if (messages.Count == 0)
            {
                elem.Add(new XElement("p", $"No logs for {name} item"));
                main.Add(elem);
                return main;
            }


            var logHeader = new XElement("div",
               new XAttribute("class", "logHeader"),
               new XElement("div", "Logs:", new XAttribute("class", "logHeaderName")),
               new XElement("div", new XAttribute("class", "log-fltr-btn-exp"),
                   new XElement("span", new XAttribute("class", "glyphicon glyphicon-chevron-right"))
               ),
               GetLogButtons()
            );


            if (messages.Count != 0)
            {
                foreach (var msg in messages)
                {
                    var tmp = new XElement("div",
                        new XAttribute("class", "log"),
                        new XElement("span", $"{msg.Level}", new XAttribute("class", $"log-level bg-{GetLogColor(msg.Level)}")),
                        new XElement("span", $" | {msg.DataStemp}", new XAttribute("class", "log-datastemp")),
                        GetMessage(msg),
                        GetException(msg)
                    );
                    elem.Add(tmp);
                }
            }

            main.Add(logHeader);
            main.Add(elem);
            return main;
        }
        public XElement GetTests(TestItem testItem)
        {
            if (testItem.Childs.Count != 0)
            {
                XElement acc = new XElement("div", new XAttribute("class", "tests"));
                foreach (var item in testItem.Childs)
                {
                    acc.Add(GetReport(item));
                }
                return acc;
            }
            return null;
        }
        public XElement GetSteps(TestItem testItem)
        {
            if (testItem.Steps.Count != 0)
            {
                XElement acc = new XElement("div", new XAttribute("class", "steps"));

                acc.Add(GetStepButtons(testItem));

                foreach (var step in testItem.Steps)
                {
                    acc.Add(
                        new XElement("div",
                            new XAttribute("class", "step"),
                            new XElement("div",
                                new XAttribute("class", $"panel panel-{GetContainerColor(step.Status)}"),
                                new XElement("div",
                                    new XAttribute("class", "panel-heading"),
                                    new XElement("p", $"{step.Name}"),
                                    new XElement("p", $"Status: {step.Status}", new XAttribute("class", $"status{step.Status}")),
                                    new XElement("p", $"Duration: {step.Duration}"),
                                    GetLogExpander(step)
                                ),
                                new XElement("div",
                                    new XAttribute("class", "panel-body"),
                                    new XElement("p", $"Description: {step.Description}"),
                                    GetLogs(step)
                                )
                            )
                        )
                    );
                }
                return acc;
            }

            return null;
        }

        public XElement GetReport(TestItem testItem)
        {
            XElement cont = new XElement("div",
                new XAttribute("class", "test"),
                new XElement("div",
                    new XAttribute("class", $"panel panel-{GetContainerColor(testItem.Status)}"),
                    new XElement("div",
                        new XAttribute("class", "panel-heading"),
                        new XElement("p", $"{testItem.Type}: {testItem.Name}"),
                        new XElement("p", $"Status: {testItem.Status}", new XAttribute("class", $"status{testItem.Status}")),
                        new XElement("p", $"Duration: {testItem.Duration} "),
                        GetTestExpander(testItem),
                        GetStepExpander(testItem),
                        GetLogExpander(testItem)
                    ),
                    new XElement("div",
                        new XAttribute("class", "panel-body"),
                        new XElement("p", $"Description: {testItem.Description}"),
                        GetOverall(testItem),
                        GetLogs(testItem)
                    )
                ),
                GetSteps(testItem),
                GetTests(testItem)
            );


            return cont;
        }


    }
}
