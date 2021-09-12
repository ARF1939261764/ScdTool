using ScdTool;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CH341Lib
{
    public class CH341 : IIIC
    {
        UInt32 Index;
        byte[] WriteWithRead(byte SlvAddr, byte[] TxBuff, int TxLength, int RxLength)
        {
            byte[] txbuff;
            byte[] rxbuff = new byte[RxLength];
            txbuff = new byte[TxLength + 1];
            txbuff[0] = (byte)((SlvAddr << 1) | 0x00);
            for (int i = 0; i < TxLength; i++)
            {
                txbuff[i + 1] = TxBuff[i];
            }
            CH341DLL.StreamI2C(Index, (uint)(TxLength + 1), txbuff, (uint)RxLength, rxbuff);
            return rxbuff;
        }
        bool IIIC.Close()
        {
            CH341DLL.CloseDevice(Index);
            return true;
        }

        string[] IIIC.GetDeviceList()
        {
            string[] list;
            IntPtr DevName;
            uint i;
            for (i = 0; ; i++)
            {
                if (CH341DLL.GetDeviceName(i) == IntPtr.Zero)
                {
                    break;
                }
            }
            list = new string[i];
            for (i = 0; i < list.Length; i++)
            {
                DevName = CH341DLL.GetDeviceName(i);
                if (DevName == IntPtr.Zero)
                {
                    break;
                }
                list[i] = Marshal.PtrToStringAnsi(DevName);
            }
            return list;
        }

        bool IIIC.Init(double Baudrate)
        {
            return true;
        }

        byte[] IIIC.Read(byte SlvAddr, int Length)
        {
            byte[] buff = new byte[Length];
            buff = WriteWithRead(SlvAddr, null, 0, Length);
            return buff;
        }

        byte IIIC.Read(byte SlvAddr)
        {
            byte[] buff;
            buff = WriteWithRead(SlvAddr,null,0,1);
            if (buff == null)
            {
                return 0xCC;
            }
            return buff[0];
        }

        bool IIIC.SetBaudrate(double Baudrate)
        {
            /*none*/
            return true;
        }
        bool IIIC.Write(byte SlvAddr, byte[] TxBuff, int Length)
        {
            WriteWithRead(SlvAddr, TxBuff, Length, 0);
            return true;
        }
        bool IIIC.Write(byte SlvAddr, byte data)
        {
            WriteWithRead(SlvAddr, new byte[] { data }, 1, 0);
            return true;
        }
        byte[] IIIC.WriteWithRead(byte SlvAddr, byte[] TxBuff, int TxLength, int RxLength)
        {
            return WriteWithRead(SlvAddr, TxBuff, TxLength, RxLength);
        }

        bool IIIC.Open(uint idx)
        {
            Index = idx;
            return CH341DLL.OpenDevice(Index) == IntPtr.Zero ? true : false;
        }
    }
}
