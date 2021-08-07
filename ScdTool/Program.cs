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

namespace ScdTool
{
    class Program
    {
        static string ScriptLib = "using ScdFtd2xxLib;using static FTD2XX_NET.FTDI;";
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
            CSharpScript.RunAsync(str, options);
        }
    }
}
