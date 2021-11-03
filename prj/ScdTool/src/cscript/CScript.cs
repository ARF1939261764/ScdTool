using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Linq;

namespace ScdTool
{
    class CScript
    {
        [DllImport("picoc.dll", EntryPoint = "picoc", CallingConvention = CallingConvention.Cdecl)]
        extern static int picoc(int argn, IntPtr[] argv);
        [DllImport("picoc.dll", EntryPoint = "IIC_SetCSCallback", CallingConvention = CallingConvention.StdCall)]
        extern static void IIC_SetCSCallback(IICWrapperForC.CallbackDelegate callbackDelegate);

        public CScript()
        {
            IICWrapperForC IICWrapper = new IICWrapperForC();
            IIC_SetCSCallback(IICWrapper.CHandler);
        }
        public bool run(List<string> filelist, List<string> argv)
        {
            int i;
            string[] c_argv = new string[1 + 1 + 1 + argv.Count];
            c_argv[0] = "picoc";
            c_argv[1] = filelist[0];
            c_argv[2] = "-";
            for (i = 0; i < argv.Count; i++)
            {
                c_argv[3 + i] = argv[i];
            }
            picoc(c_argv.Length, c_argv.Select(u => Marshal.StringToHGlobalAnsi(u)).ToArray());
            return true;
        }
    }
}
