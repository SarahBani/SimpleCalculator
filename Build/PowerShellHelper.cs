using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Build
{
    public static class PowerShellHelper
    {

        public static IList<string> GetExecutionResult(string command)
        {
            IList<string> list = new List<string>();
            using (var ps = PowerShell.Create())
            {
                var results = ps.AddScript(command).Invoke();
                foreach (var result in results)
                {
                    string output = result?.ToString() ?? null;
                    list.Add(output);
                   Console.Write(output);
                }
            }
            return list;
        }

    }
}
