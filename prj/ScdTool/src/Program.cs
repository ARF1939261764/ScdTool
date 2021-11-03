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
using static System.Threading.Thread;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ScdTool
{
    class Program
    {
        static void CMDHandle_DisplayVersion()
        {
            Console.WriteLine("版本信息:");
            Console.WriteLine("    Version:0.6");
            Console.WriteLine("    Compile time:{0:s}\n", System.IO.File.GetLastWriteTime(typeof(Program).Assembly.Location).ToString());
        }
        static void CMD_Handle_ListIICDevice()
        {
            IIC iic = new IIC();
            string[] devList = iic.GetDeviceList();
            if (devList == null || devList.Length == 0)
            {
                Console.WriteLine("没有找到设备\n");
                return;
            }
            Console.WriteLine("IIC设备列表:");
            Console.WriteLine("-------|-------------------------------------------");
            Console.WriteLine("Number | Name                                      ");
            Console.WriteLine("-------|-------------------------------------------");
            for (int i = 0; i < devList.Length; i++)
            {
                Console.WriteLine(" {0:D4}  | {1:S96}", i,devList[i]);
            }
            Console.WriteLine("");
        }
        static void MainArgParse(string[] args)
        {
            int i,state = 0;
            List<string> argv = new List<string>();
            List<string> filelist = new List<string>();
            for (i = 0; i < args.Length && state == 0; i++)
            {
                switch (args[i])
                {
                    case "-v":
                        CMDHandle_DisplayVersion();
                        break;
                    case "-lsiic":
                        CMD_Handle_ListIICDevice();
                        break;
                    default:
                        state = 1;
                        break;
                }
            }
            if (state == 0)
            {
                return;
            }
            for (i--; i < args.Length; i++)
            {
                if (Regex.IsMatch(args[i], @".*\.c$",RegexOptions.IgnoreCase))
                {
                    state = 2;
                    break;
                }
                else if (Regex.IsMatch(args[i], @".*\.cs$", RegexOptions.IgnoreCase))
                {
                    state = 3;
                    break;
                }
                else
                {
                    Console.WriteLine("WARNING:{0:s}为未定义的参数,将会被忽略",args[i]);
                }
            }
            if (i == args.Length)
            {
                return;
            }
            filelist.Add(args[i]);
            for (i++; i < args.Length; i++)
            {
                argv.Add(args[i]);
            }
            if (state == 2)
            {
                CScript Parser = new CScript();
                Parser.run(filelist,argv);
            }
            if (state == 3)
            {
                CsScript Parser = new CsScript();
                Parser.run(filelist, argv);
            }
        }
        static void Main(string[] args)
        {
            MainArgParse(args);
        }
    }
}
