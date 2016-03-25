namespace QA.AutomatedMagic.Reports.HtmlReport
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using TestInfo;
    using MetaMagic;
    using System.IO;

    public class HtmlReportGenerator : IReportGenerator
    {
        private string Path;

        public bool BuildInOneFile { get; set; } = true;

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

            if (BuildInOneFile)
            {
                //&gt; >
                //&lt; <
                string final = html.ToString();
                final = final.Replace("&gt;", ">");
                final = final.Replace("&lt;", "<");
                File.WriteAllText(Path, final);
            }
            else
            {
                html.Save(Path);
            }
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


            head.Add(charset, httpEquiv, name, title);

            head.Add(GetCss("https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css", true));
            head.Add(GetCss(@"css/css.css"));

            return head;
        }

        public XElement GetCss(string name, bool isLink = false)
        {
            XElement customCss = null;
            if (isLink)
            {
                customCss = new XElement("link",
                    new XAttribute("rel", "stylesheet"),
                    new XAttribute("href", name)
                );
                return customCss;
            }

            if (BuildInOneFile)
            {
                string res;
                res = File.ReadAllText(name);
                customCss = new XElement("style",
                        new XAttribute("type", "text/css"),
                        res
                );
            }
            else
            {
                customCss = new XElement("link",
                    new XAttribute("rel", "stylesheet"),
                    new XAttribute("href", name)
                );
            }

            return customCss;
        }
        public XElement GetJS(string name, bool IsLink = false)
        {
            XElement script = null;
            if (IsLink)
            {
                script = new XElement("script", "", new XAttribute("src", name));
                return script;
            }

            if (BuildInOneFile)
            {
                script = new XElement("script");
                string res = File.ReadAllText(name);
                script.Add(res);
            }
            else
            {
                script = new XElement("script", "", new XAttribute("src", name));
            }

            return script;
        }

        public XElement GetBody(TestItem testItem, TestEnvironmentInfo testEnvironmentInfo)
        {
            var body = new XElement("body");

            var container = new XElement("div", new XAttribute("class", "container"),
                GetEnvironment(testEnvironmentInfo),
                GetReport(testItem)
            );

            body.Add(container);

            body.Add(GetJS("https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js", true));
            body.Add(GetJS("https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js", true));


            body.Add(GetJS(@"filter js/init.js"));
            body.Add(GetJS(@"filter js/filter.js"));
            body.Add(GetJS(@"filter js/testFilter.js"));
            body.Add(GetJS(@"filter js/logFilter.js"));
            body.Add(GetJS(@"filter js/stepFilter.js"));


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
                    new XElement("td", testEnvironmentInfo.OSVersion),
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

        public XElement GetOverallMagic(TestItem testItem)
        {
            if (testItem.Type == TestItemType.Test) return null;

            var mainContainer = new XElement("div", new XAttribute("class", "checkboxes overall test-fltr-btns"));

            mainContainer.Add(GetOverallCheckBox("Total",       testItem.GetTotal(), "passed failed skipped notexecuted"));
            mainContainer.Add(GetOverallCheckBox("NotExecuted",       testItem.GetTotal(), "notexecuted"));
            mainContainer.Add(GetOverallCheckBox("Passed",      testItem.GetWithStatus(TestItemStatus.Passed) , "passed"));
            mainContainer.Add(GetOverallCheckBox("Failed",      testItem.GetWithStatus(TestItemStatus.Failed) , "failed"));
            mainContainer.Add(GetOverallCheckBox("Skipped",     testItem.GetWithStatus(TestItemStatus.Skipped), "skipped"));

          

            return mainContainer;
        }

        
        public XElement GetOverallCheckBox(string text, int count, string filters)
        {
            var checkBox = new XElement("div", 
                new XAttribute("class", "checkbox")
            );

            var input = new XElement("input", new XAttribute("type", "checkbox"), new XAttribute("filter", filters));
            var label = new XElement("label", text);
            var labelCount = new XElement("label", count);

            checkBox.Add(input);
            checkBox.Add(label);
            checkBox.Add(labelCount);

            return checkBox;
        }

        
        public XElement GetOverall(TestItem testItem)
        {
            return GetOverallMagic(testItem);
            if (testItem.Type == TestItemType.Test)
                return null;

            var table = new XElement("table", new XAttribute("class", "table overall"));

            var thead = new XElement("thead",
                new XElement("tr",
                    new XAttribute("class", "test-fltr-btns"),
                    new XElement("th", new XElement("button", "Total",          new XAttribute("class", "btn test-fltr-btn-all"),       new XAttribute("filter", "passed failed skipped notexecuted unknown"))),
                    new XElement("th", new XElement("button", "NotExecuted",    new XAttribute("class", "btn test-fltr-btn-notexctd"),  new XAttribute("filter", "notexecuted"))),
                    new XElement("th", new XElement("button", "Passed",         new XAttribute("class", "btn test-fltr-btn-psd"),       new XAttribute("filter", "passed"))),
                    new XElement("th", new XElement("button", "Failed",         new XAttribute("class", "btn test-fltr-btn-fld"),       new XAttribute("filter", "с"))),
                    new XElement("th", new XElement("button", "Skipped",        new XAttribute("class", "btn test-fltr-btn-skpd"),      new XAttribute("filter", "skipped"))),
                    new XElement("th", new XElement("button", "Unknown",        new XAttribute("class", "btn test-fltr-btn-unkwn"),     new XAttribute("filter", "unknown")))
                )
            );

            var tbody = new XElement("tbody",
                new XElement("tr",
                    new XElement("td", testItem.GetTotal()),
                    new XElement("td", testItem.GetWithStatus(TestItemStatus.NotExecuted)),
                    new XElement("td", testItem.GetWithStatus(TestItemStatus.Passed)),
                    new XElement("td", testItem.GetWithStatus(TestItemStatus.Failed)),
                    new XElement("td", testItem.GetWithStatus(TestItemStatus.Skipped)),
                    new XElement("td", testItem.GetWithStatus(TestItemStatus.Unknown))
                )
            );

            table.Add(thead, tbody);

            return table;
        }
        public XElement GetTestInfoTable(TestItem testItem)
        {
            XElement infoTable = new XElement("table",
                                    new XAttribute("class", "itemInfo"),
                                    new XElement("tbody",
                                        new XElement("tr",
                                            new XElement("td",
                                                new XAttribute("colspan", "3"),
                                                $"{testItem.Type}: {testItem.Description}"
                                            )
                                        ),
                                        new XElement("tr",
                                            new XElement("td", $"Status: {testItem.Status}", new XAttribute("class", $"status{testItem.Status}")),
                                            new XElement("td", $"Duration: {testItem.Duration}"),
                                            new XElement("td", $"Name: {testItem.Name}")
                                        )
                                     )
                                 );
            return infoTable;

        }
        public XElement GetStepInfoTable(Step step)
        {
            XElement infoTable = new XElement("table",
                                    new XAttribute("class", "itemInfo"),
                                    new XElement("tbody",
                                        new XElement("tr",
                                            new XElement("td",
                                                new XAttribute("colspan", "3"),
                                                $"{step.Description}  ",
                                                GetLogExpander(step)
                                            )
                                        ),
                                        new XElement("tr",
                                            new XElement("td", $"Status: {step.Status}", new XAttribute("class", $"status{step.Status}")),
                                            new XElement("td", $"Duration: {step.Duration}"),
                                            new XElement("td", $"Name: {step.Name}")
                                        )
                                     )
                                 );
            return infoTable;

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
        public XElement GetStepExpander(BaseMetaObject obj)
        {
            var steps = obj is TestItem ? ((TestItem)obj).Steps : obj is Step ? ((Step)obj).Steps : null;
            if (steps == null)
                return null;

            XElement btn = null;
            if (steps.Count != 0)
            {
                btn = new XElement("button", new XAttribute("class", "btn btnstep btn-warning"), "Steps");
            }

            return btn;
        }
        public XElement GetLogExpander(BaseMetaObject obj)
        {
            if (!(obj is TestItem || obj is Step))
                return null;
            var logs = obj is TestItem ? ((TestItem)obj).LogMessages : obj is Step ? ((Step)obj).Messages : null;
            if (logs == null || logs.Count == 0)
                return null;


            XElement btn = new XElement("button", new XAttribute("class", "btn btnlog btn-info"), "Logs");
            return btn;
        }

        public XElement GetException(LogMessage logMessage)
        {
            XElement ex = null;
            if (logMessage.ExceptionString != null)
            {
                var messageLines = logMessage.ExceptionString.Split(new string[] {"\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                ex = new XElement("div", new XAttribute("class", "log-exception"));
                foreach (var line in messageLines)
                {
                    ex.Add(new XElement("p", line));
                }
            }
            
            return ex;
        }
        public XElement GetMessage(LogItem logMessage)
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
        public XElement GetStepButtons(BaseMetaObject obj)
        {
            var steps = obj is TestItem ? ((TestItem)obj).Steps : obj is Step ? ((Step)obj).Steps : null;
            if (steps?.Count == null)
                return null;

            //NotExecuted, Unknown, Passed, Failed, Skipped
            var bdiv = new XElement("div",
                new XAttribute("class", "step-fltr-btns"),
                new XElement("button", "All", new XAttribute("class", "btn step-fltr-btn-all"), new XAttribute("filter", "passed failed skipped unknown notexecuted")),
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
            var messages = (obj as TestItem)?.LogMessages ?? (obj as Step)?.Messages;

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
                foreach (var logItem in messages)
                {
                    var msg = logItem as LogMessage;
                    if (msg != null)
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

                    var att = logItem as LogFile;
                    if (att != null)
                    {
                        XElement tmp = null;
                        switch (att.FileType)
                        {
                            case LoggedFileType.JPG:
                            case LoggedFileType.PNG:
                            case LoggedFileType.BMP:
                                tmp = new XElement("div",
                                    new XAttribute("class", "log"),
                                    new XElement("span", $"{att.Level}", new XAttribute("class", $"log-level bg-{GetLogColor(att.Level)}")),
                                    new XElement("span", $" | {att.DataStemp}", new XAttribute("class", "log-datastemp")),
                                    GetMessage(att),
                                    new XElement("div", new XAttribute("class", "img-btn-exp"),
                                       new XElement("span", "Screenshot"),
                                       new XElement("span", new XAttribute("class", "glyphicon glyphicon-triangle-bottom"))
                                    ),
                                    new XElement("div", new XAttribute("class", "image"),
                                        new XElement("img", new XAttribute("src", att.FilePath))
                                    )
                                );
                                break;
                            case LoggedFileType.ZIP:
                                tmp = new XElement("div",
                                    new XAttribute("class", "log"),
                                    new XElement("span", $"{att.Level}", new XAttribute("class", $"log-level bg-{GetLogColor(att.Level)}")),
                                    new XElement("span", $" | {att.DataStemp}", new XAttribute("class", "log-datastemp")),
                                    GetMessage(att),
                                    new XElement("div", new XAttribute("class", "Link"),
                                       new XElement("span", "ZIP"),
                                       new XElement("a",
                                            new XAttribute("href", att.FilePath),
                                            new XAttribute("target", "_blank"),
                                            "Link"
                                        )
                                    )
                                );
                                break;
                            case LoggedFileType.TXT:
                                tmp = new XElement("div",
                                    new XAttribute("class", "log"),
                                    new XElement("span", $"{att.Level}", new XAttribute("class", $"log-level bg-{GetLogColor(att.Level)}")),
                                    new XElement("span", $" | {att.DataStemp}", new XAttribute("class", "log-datastemp")),
                                    GetMessage(att),
                                    new XElement("div", new XAttribute("class", "Link"),
                                       new XElement("span", "TXT"),
                                       new XElement("a",
                                            new XAttribute("href", att.FilePath),
                                            new XAttribute("target", "_blank"),
                                            "Link"
                                        )
                                    )
                                );
                                break;
                            default:
                                break;
                        }
                        elem.Add(tmp);
                    }
                }
            }


            var up = new XElement("div",
                new XAttribute("class", "log-slideup"),
                new XElement("span",
                    new XAttribute("class", "glyphicon glyphicon-menu-up"),
                    ""
                )
             );
           

            var table = new XElement("table", new XAttribute("class", "logsContainer"));
            var tbody = new XElement("tbody");
            var row = new XElement("tr");
            row.Add(new XElement("td", new XAttribute("class", "logTD"), elem));
            row.Add(new XElement("td", new XAttribute("class", "slideupTD"), up));
            tbody.Add(row);
            table.Add(tbody);

            main.Add(logHeader);
            main.Add(table);

            


            
            //main.Add(elem);
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
                var up = new XElement("div",
                   new XAttribute("class", "test-slideup"),
                   new XElement("span",
                       new XAttribute("class", "glyphicon glyphicon-menu-up"),
                       ""
                   )
                );

                var table = new XElement("table", new XAttribute("class", "testContainer"));
                var tbody = new XElement("tbody");
                var row = new XElement("tr");
                row.Add(new XElement("td", new XAttribute("class", "slideupTD"), up));
                row.Add(new XElement("td", new XAttribute("class", "testTD"), acc));

                tbody.Add(row);
                table.Add(tbody);

                //return acc;
                return table;
            }

            

            return null;
        }


        public XElement GetSteps(BaseMetaObject obj)
        {
            var steps = obj is TestItem ? ((TestItem)obj).Steps : (obj is Step ? ((Step)obj).Steps : null);
            string name = obj is Step ? ((Step)obj).Description : null;

           
            if (steps?.Count == null)
                return null;

            XElement acc = new XElement("div", new XAttribute("class", "steps"));

            acc.Add(GetStepButtons(obj));

            foreach (var step in steps)
            {
                acc.Add(
                    new XElement("div",
                        new XAttribute("class", "step"),
                        new XElement("div",
                            new XAttribute("class", $"panel panel-{GetContainerColor(step.Status)}"),
                            new XElement("div",
                                new XAttribute("class", "panel-heading"),
                                GetStepInfoTable(step),
                                GetStepExpander(step)
                            ),
                            GetLogs(step)
                        ),
                        GetSteps(step)
                    )
                );
            }

            var up = new XElement("div",
                   new XAttribute("class", "step-slideup"),
                   new XElement("span",
                       new XAttribute("class", "glyphicon glyphicon-menu-up"),
                       ""
                   )
                );

            var table = new XElement("table", new XAttribute("class", "stepContainer"));
            var tbody = new XElement("tbody");
            var row = new XElement("tr");
            row.Add(new XElement("td", new XAttribute("class", "slideupTD"), up));
            row.Add(new XElement("td", new XAttribute("class", "testTD"), acc));

            tbody.Add(row);
            table.Add(tbody);

            return table;
           // return acc;
        }

        public XElement GetReport(TestItem testItem)
        {
            XElement overall = GetOverall(testItem);
            XElement div = null;
            if (overall != null)
            {
                div = new XElement("div",
                        new XAttribute("class", "panel-body"),
                        overall
                );
            }

           

            XElement cont = new XElement("div",
                new XAttribute("class", "test"),
                new XElement("div",
                    new XAttribute("class", $"panel panel-{GetContainerColor(testItem.Status)}"),
                    new XElement("div",
                        new XAttribute("class", "panel-heading"),
                        GetTestInfoTable(testItem),
                        GetTestExpander(testItem),
                        GetStepExpander(testItem),
                        GetLogExpander(testItem)
                    ),
                    div,
                    GetLogs(testItem)
                ),
                GetSteps(testItem),
               GetTests(testItem)
            );


            

            return cont;
        }
    }
}
