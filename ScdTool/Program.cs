using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using FTD2XX_NET;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using ScdFtd2xxLib;
using static FTD2XX_NET.FTDI;
using System.Threading.Tasks;

namespace ScdTool
{
    class Program
    {
        static string ScriptLib = "using ScdTool;using CH341Lib;using ScdFtd2xxLib;using static FTD2XX_NET.FTDI;";
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("please enter script file");
                Console.ReadKey();
                return;
            }
            string str = ScriptLib + File.ReadAllText(args[0]);
            ScriptOptions options = ScriptOptions.Default.AddReferences(typeof(ScdFtd2xxLib.ScdFtd2xx).Assembly);
            try
            {
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
        }
    }
}
