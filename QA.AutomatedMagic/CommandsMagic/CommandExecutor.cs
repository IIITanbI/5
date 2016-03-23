namespace QA.AutomatedMagic.CommandsMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Reflection;

    public class CommandExecutionInfo
    {
        public MethodInfo Method { get; set; } = null;
        public object[] Arguments = null;
        public object ManagerObject = null;

        public object Execute()
        {
            if(Method.Name == "GetIssues")
            {

            }

            try
            {
                return Method.Invoke(ManagerObject, Arguments);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
