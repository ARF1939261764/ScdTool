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

        bool IIIC.Read(byte SlvAddr, byte[] Buff, int Length)
        {
            byte[] rxbuff = WriteWithRead(SlvAddr, null, 0, Length);
            if (rxbuff == null || rxbuff.Length != Length)
            {
                return false;
            }
            for (int i = 0; i < Length; i++)
            {
                Buff[i] = rxbuff[i];
            }
            return true;
        }

        bool IIIC.Read(byte SlvAddr, ref byte Data)
        {
            byte[] buff;
            buff = WriteWithRead(SlvAddr,null,0,1);
            if (buff == null)
            {
                return false;
            }
            Data = buff[0];
            return true;
        }

        bool IIIC.SetBaudrate(double Baudrate)
        {
            /*none*/
            return true;
        }
        bool IIIC.Write(byte SlvAddr, byte[] Buff, int Length)
        {
            WriteWithRead(SlvAddr, Buff, Length, 0);
            return true;
        }
        bool IIIC.Write(byte SlvAddr, byte Data)
        {
            WriteWithRead(SlvAddr, new byte[] { Data }, 1, 0);
            return true;
        }
        bool IIIC.WriteWithRead(byte SlvAddr, byte[] TxBuff, int TxLength, byte[] RxBuff, int RxLength)
        {
            return CH341DLL.StreamI2C(Index, (uint)(TxLength + 1), TxBuff, (uint)RxLength, RxBuff);
        }

        bool IIIC.Open(int Index)
        {
            this.Index = (uint)Index;
            return CH341DLL.OpenDevice((uint)Index) == IntPtr.Zero ? false : true;
        }
    }
}
