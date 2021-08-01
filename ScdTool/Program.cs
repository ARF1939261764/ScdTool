using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using FTD2XX_NET;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace ScdTool
{
    class Program
    {
        static string ScriptLib = "using ScdFtd2xxLib;using static FTD2XX_NET.FTDI;";
        static async void test()
        {
            ScriptOptions options = ScriptOptions.Default.AddReferences(typeof(ScdFtd2xxLib.ScdFtd2xx).Assembly);
            string str1 = ScriptLib + File.ReadAllText("main.cs");
            var state = await CSharpScript.RunAsync(str1, options);
            string str2 = ScriptLib + File.ReadAllText("test.cs");
            state = await state.ContinueWithAsync(str2);
            string str3 = "int b = 2+2;";
            state = await state.ContinueWithAsync(str3);
            Console.WriteLine(state.ReturnValue);
        }
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("please enter script file");
                return;
            }
            string str = ScriptLib + File.ReadAllText(args[0]);
            var syntaxTree = CSharpSyntaxTree.ParseText(str);
            Type type = CompileType("Program", syntaxTree);
            MethodInfo methodInfo = type.GetMethod("Main");
            var inst = Activator.CreateInstance(type);
            methodInfo.Invoke(inst, new object[] { new string[] { "hello world" } });
        }
        private static Type CompileType(string originalClassName, SyntaxTree syntaxTree)
        {
            MetadataReference[] metadataReference = new MetadataReference[] {
                MetadataReference.CreateFromFile(typeof(FTDI).Assembly.Location)
            };
            string test = typeof(object).Assembly.Location;
            var assemblyName = $"{originalClassName}.g";
            var compilation = CSharpCompilation.Create(assemblyName, new[] { syntaxTree },
                references: metadataReference,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)).AddReferences(
                AppDomain.CurrentDomain.GetAssemblies().Select(x => MetadataReference.CreateFromFile(x.Location))
                ); ;
            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);
                if (result.Success)
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(ms.ToArray());
                    return assembly.GetTypes().First(x => x.Name == originalClassName);
                }
                else 
                {
                    foreach (var item in result.Diagnostics)
                    {
                        Console.WriteLine(item);
                    }
                    Environment.Exit(0x1);
                    return null;
                }
            }
        }
    }
}
