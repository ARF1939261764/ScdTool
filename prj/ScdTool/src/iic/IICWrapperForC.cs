using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ScdTool
{

    class IICWrapperForC
    {
        const int IIC_DEVICE_NAME_MAX_LEN = 512;
        [System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public delegate int CallbackDelegate(CCmd Cmd, IntPtr Arg);
        public delegate void CCmdHandler(IntPtr Arg);
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
        public IICWrapperForC()
        {
            IICInst = new IIC();
            IICInst.GetDeviceList();
        }
        public int CHandler(CCmd Cmd, IntPtr Arg)
        {
            Dictionary<CCmd, CCmdHandler> CCmdHandlerList = new Dictionary<CCmd, CCmdHandler> {
                {CCmd.CreateDeviceObject,CreateDeviceObject },
                {CCmd.SetContext,SetContext },
                {CCmd.GetContext,GetContext },
                {CCmd.GetDeviceList,GetDeviceList },
                {CCmd.OpenByIndex,OpenByIndex },
                {CCmd.Open,Open},
                {CCmd.SetBaudrate,SetBaudrate },
                {CCmd.Close,Close },
                {CCmd.ReadByte,ReadByte },
                {CCmd.Read,Read },
                {CCmd.WriteByte,WriteByte },
                {CCmd.Write,Write },
                {CCmd.WriteWithRead,WriteWithRead }
            };
            CCmdHandlerList[Cmd]?.Invoke(Arg);
            return 0;
        }
        void WriteWithRead(IntPtr Arg)
        {
            byte SlvAddr = Marshal.ReadByte(Arg, 4);
            int WriteDataAddr = Marshal.ReadInt32(Arg, 8);
            int WriteLength = Marshal.ReadInt32(Arg, 12);
            int ReadDataAddr = Marshal.ReadInt32(Arg, 16);
            int ReadLength = Marshal.ReadInt32(Arg, 20);
            IntPtr WriteDataBuff = new IntPtr(WriteDataAddr);
            IntPtr ReadDataBuff = new IntPtr(ReadDataAddr);
            byte[] WriteData = new byte[WriteLength];
            byte[] ReadData = new byte[ReadLength];
            Marshal.Copy(WriteDataBuff, WriteData,0, WriteLength);
            bool Status = IICInst.WriteWithRead(SlvAddr, WriteData, WriteLength, ReadData, ReadLength);
            Marshal.Copy(ReadData, 0, ReadDataBuff, ReadLength);
            Marshal.WriteInt32(Arg, Status == false ? -1 : 0);
        }
        void Write(IntPtr Arg)
        {
            byte SlvAddr = Marshal.ReadByte(Arg, 4);
            int DataAddr = Marshal.ReadInt32(Arg, 8);
            int Length = Marshal.ReadInt32(Arg, 12);
            IntPtr DataBuff = new IntPtr(DataAddr);
            byte[] Data = new byte[Length];
            Marshal.Copy(DataBuff, Data, 0, Length);
            bool Status = IICInst.Write(SlvAddr, Data, Length);
            Marshal.WriteInt32(Arg, Status == false ? -1 : 0);
        }
        void WriteByte(IntPtr Arg)
        {
            byte SlvAddr = Marshal.ReadByte(Arg, 4);
            byte Data = Marshal.ReadByte(Arg, 8);
            bool Status = IICInst.Write(SlvAddr, Data);
            Marshal.WriteInt32(Arg, Status == false ? -1 : 0);
        }
        void Read(IntPtr Arg)
        {
            byte SlvAddr = Marshal.ReadByte(Arg, 4);
            int DataAddr = Marshal.ReadInt32(Arg, 8);
            int Length   = Marshal.ReadInt32(Arg,12);
            byte[] Data = new byte[Length];
            bool Status = IICInst.Read(SlvAddr, Data, Length);
            IntPtr DataBuff = new IntPtr(DataAddr);
            for (int i = 0; i < Length; i++)
            {
                Marshal.WriteByte(DataBuff, i, Data[i]);
            }
            Marshal.WriteInt32(Arg, Status == false ? -1 : 0);
        }
        void ReadByte(IntPtr Arg)
        {
            byte SlvAddr = Marshal.ReadByte(Arg, 4);
            int DataAddr = Marshal.ReadInt32(Arg, 8);
            byte Data = 0;
            bool Status = IICInst.Read(SlvAddr, ref Data);
            Marshal.WriteByte(new IntPtr(DataAddr), Data);
            Marshal.WriteInt32(Arg, Status == false ? -1 : 0);
        }
        void Close(IntPtr Arg)
        {
            bool Status = IICInst.Close();
            Marshal.WriteInt32(Arg, Status == false ? -1 : 0);
        }
        void SetBaudrate(IntPtr Arg)
        {
            byte[] BaudrateArray = new byte[8];
            double Baudrate;
            for (int i = 0; i < 8; i++)
            {
                BaudrateArray[i] = Marshal.ReadByte(Arg, 8 + i);
            }
            Baudrate = BitConverter.ToDouble(BaudrateArray);
            bool Status = IICInst.SetBaudrate(Baudrate);
            Marshal.WriteInt32(Arg, Status == false ? -1 : 0);
        }
        void Open(IntPtr Arg)
        {
            int Addr = Marshal.ReadInt32(Arg, 4);
            string Name = Marshal.PtrToStringAnsi(new IntPtr(Addr));
            bool Status = IICInst.Open(Name);
            Marshal.WriteInt32(Arg, Status == false ? -1 : 0);
        }
        void OpenByIndex(IntPtr Arg)
        {
            int Index = Marshal.ReadInt32(Arg, 4);
            bool Status = IICInst.Open(Index);
            Marshal.WriteInt32(Arg, Status == false ? -1 : 0);
        }
        void CreateDeviceObject(IntPtr Arg)
        {
            Marshal.WriteInt32(Arg, IIC_CreateDeviceObject().ToInt32());
        }
        void SetContext(IntPtr Arg)
        {
            int NowPtrInt32 = Marshal.ReadInt32(Arg, 4);
            Marshal.WriteInt32(Arg, IIC_SetContext(new IntPtr(NowPtrInt32)).ToInt32());
        }
        void GetContext(IntPtr Arg)
        {
            Marshal.WriteInt32(Arg, IIC_GetContext().ToInt32());
        }
        void GetDeviceList(IntPtr Arg)
        {
            int i;
            int BuffAddr = Marshal.ReadInt32(Arg, 4);
            int MaxDeviceNum = Marshal.ReadInt32(Arg, 8);
            string[] DeviceList = IICInst.GetDeviceList();
            for (i = 0; i < MaxDeviceNum && i < DeviceList.Length; i++)
            {
                IntPtr Buff = new IntPtr(BuffAddr + i*IIC_DEVICE_NAME_MAX_LEN);
                byte[] Array = System.Text.Encoding.ASCII.GetBytes(DeviceList[i]);
                Marshal.Copy(Array, 0, Buff, Array.Length);
                Marshal.WriteByte(Buff, Array.Length,0x00);
            }
            Marshal.WriteInt32(Arg,0, i);
        }
        public IntPtr IIC_CreateDeviceObject()
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
        public IntPtr IIC_SetContext(IntPtr DeviceObject)
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
        public IntPtr IIC_GetContext()
        {
            GCHandle IICHandle = GCHandle.Alloc(IICInst);
            IntPtr IICPtr = GCHandle.ToIntPtr(IICHandle);
            return IICPtr;
        }
    }
}
