using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ScdTool
{
    
    class IICWrapperForC
    {
        [System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public delegate int CallbackDelegate(CCmd Cmd, IntPtr Arg);
        public enum CCmd
        {
            CreateDeviceObject = 0,
            SetContext,
            GetContext,
            GetDeviceList,
            OpenByIndex,
            Open,
            SetBaudrate,
            Close,
            ReadByte,
            Read,
            WriteByte,
            Write,
            WriteWithRead
        }
        IIC IICInst;
        public int CHandler(CCmd Cmd, IntPtr Arg)
        {
            switch (Cmd)
            {   
                case CCmd.CreateDeviceObject:
                    Marshal.WriteInt32(Arg, CreateDeviceObject().ToInt32());
                    break;
                case CCmd.SetContext:
                    int NowPtrInt32 = Marshal.ReadInt32(Arg, 4);
                    Marshal.WriteInt32(Arg, SetContext(new IntPtr(NowPtrInt32)).ToInt32());
                    break;
                case CCmd.GetContext:
                    Marshal.WriteInt32(Arg, GetContext().ToInt32());
                    break;
                case CCmd.GetDeviceList:

                    break;
                default:
                    Console.WriteLine("unsupport cmd:{0:s}",Cmd.ToString());
                    break;
            }
            return 0;
        }
        public IntPtr CreateDeviceObject()
        {
            IIC IICObject;
            IICObject = new IIC();
            if (IICInst == null)
            {
                IICInst = IICObject;
            }
            GCHandle IICHandle = GCHandle.Alloc(IICObject);
            return GCHandle.ToIntPtr(IICHandle);
        }
        public IntPtr SetContext(IntPtr DeviceObject)
        {
            GCHandle IICHandle = GCHandle.Alloc(IICInst);
            IntPtr IICPtr = GCHandle.ToIntPtr(IICHandle);
            if (DeviceObject != IntPtr.Zero)
            {
                GCHandle NowIICHandle = GCHandle.FromIntPtr(DeviceObject);
                IICInst = NowIICHandle.Target as IIC;
            }
            return IICPtr;
        }
        public IntPtr GetContext()
        {
            GCHandle IICHandle = GCHandle.Alloc(IICInst);
            IntPtr IICPtr = GCHandle.ToIntPtr(IICHandle);
            return IICPtr;
        }
    }
}
