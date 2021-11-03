using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Threading.Tasks;
using System.Reflection;

namespace ScdTool
{
    class CsScript
    {
        static string ScriptLib = "" +
       "using System;" +
       "using System.IO;" +
       "using static System.Threading.Thread;" +
       "using System.Threading.Tasks;" +
       "using ScdTool;";
        public bool run(List<string> filelist, List<string> argv)
        {
            try
            {
                string str = ScriptLib + File.ReadAllText(filelist[0]);
                ScriptOptions options = ScriptOptions.Default.AddReferences(Assembly.GetExecutingAssembly());
                Task<ScriptState<object>> State = CSharpScript.RunAsync(str, options);
                if (!State.IsCompletedSuccessfully)
                {
                    Console.WriteLine("Error:" + State.Exception.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return true;
        }
    }
}
