using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using FTD2XX_NET;
using static FTD2XX_NET.FTDI;

namespace ScdFtd2xxLib
{
    public class ScdFtd2xx
    {
        const double FTDI_CLOCK_FRE = 60e6;
        const byte MSB_FALLING_EDGE_CLOCK_BYTE_OUT = 0x11;
        const byte MSB_FALLING_EDGE_CLOCK_BIT_OUT = 0x12;
        const byte MSB_RISING_EDGE_CLOCK_BYTE_IN = 0x20;
        const byte MSB_RISING_EDGE_CLOCK_BIT_IN  = 0x22;
        const byte MSB_FALLING_EDGE_CLOCK_BIT_IN = 0x26;
        FTDI Ftdi;
        uint RxByte;
        uint RxByteRead;
        uint TxByte;
        uint TxByteWrite;
        byte[] RxBuff = new byte[4096];
        byte[] TxBuff = new byte[4096];
        public ScdFtd2xx()
        {
            Ftdi = new FTDI();
        }
        public FT_STATUS GetNumberOfDevices(ref uint num)
        {
            return Ftdi.GetNumberOfDevices(ref num);
        }
        public FT_STATUS GetDeviceList(FT_DEVICE_INFO_NODE[] list)
        {
            return Ftdi.GetDeviceList(list);
        }
        public FT_DEVICE_INFO_NODE[] GetDeviceList()
        {
            FT_STATUS status;
            uint num = 0;
            FT_DEVICE_INFO_NODE[] list;
            Ftdi.GetNumberOfDevices(ref num);
            list = new FT_DEVICE_INFO_NODE[num];
            status = Ftdi.GetDeviceList(list);
            return status != FT_STATUS.FT_OK ? null : list;
        }
        public FT_STATUS Open(uint idx)
        {
            return Ftdi.OpenByIndex(idx);
        }
        public FT_STATUS Close()
        {
            return Ftdi.Close();
        }
        private FT_STATUS CheckMPSSE()
        {
            FT_STATUS Status = FT_STATUS.FT_OK;
            TxBuff[0] = 0xAA;
            Ftdi.Write(TxBuff, 1, ref TxByteWrite);
            do
            {
                Status = Ftdi.GetRxBytesAvailable(ref RxByte);
            } while (RxByte == 0 && Status == FT_STATUS.FT_OK);
            if (Status != FT_STATUS.FT_OK || RxByte < 2)
            {
                return FT_STATUS.FT_IO_ERROR;
            }
            Ftdi.Read(RxBuff, RxByte, ref RxByteRead);
            if (RxByte != RxByteRead)
            {
                return FT_STATUS.FT_IO_ERROR;
            }
            for (int i = 0; i < RxByte - 1; i++)
            {
                if (RxBuff[i] == 0xFA && RxBuff[i + 1] == 0xAA)
                {
                    return FT_STATUS.FT_OK;
                }
            }
            return FT_STATUS.FT_IO_ERROR;
        }
        public FT_STATUS MPSEEInit()
        {
            FT_STATUS Status = FT_STATUS.FT_OK;
            Status |= Ftdi.ResetDevice();
            Status |= Ftdi.GetRxBytesAvailable(ref RxByte);
            if (RxByte > 0)
            {
                Ftdi.Read(RxBuff, RxByte, ref RxByteRead);
            }
            Status |= Ftdi.SetCharacters(0, false, 0, false);
            Status |= Ftdi.SetTimeouts(0, 1000);
            Status |= Ftdi.SetLatency(16);
            Status |= Ftdi.SetBitMode(0x0, 0x00);
            Status |= Ftdi.SetBitMode(0x0, 0x02);
            if (Status != FT_STATUS.FT_OK)
            {
                return FT_STATUS.FT_IO_ERROR;
            }
            Thread.Sleep(50);
            Status = CheckMPSSE();
            if (Status != FT_STATUS.FT_OK)
            {
                return FT_STATUS.FT_IO_ERROR;
            }
            return FT_STATUS.FT_OK;
        }
        public FT_STATUS I2CInit(double Baudrate = 100e3)
        {
            UInt16 Div;
            FT_STATUS Status;
            Status = MPSEEInit();
            if (Status != FT_STATUS.FT_OK)
            {
                return FT_STATUS.FT_IO_ERROR;
            }
            TxByte = 0;
            TxByteWrite = 0;
            TxBuff[TxByte++] = 0x8A;/*disable divide by 5 from the 60 MHz internal clock*/
            TxBuff[TxByte++] = 0x97;
            TxBuff[TxByte++] = 0x8C;
            Status |= Ftdi.Write(TxBuff,TxByte,ref TxByteWrite);
            TxByte = 0;
            TxByteWrite = 0;
            TxBuff[TxByte++] = 0x80;
            TxBuff[TxByte++] = 0x03;
            TxBuff[TxByte++] = 0x03;
            Status |= Ftdi.Write(TxBuff, TxByte, ref TxByteWrite);
            TxByte = 0;
            TxByteWrite = 0;
            Div = (UInt16)(FTDI_CLOCK_FRE / (Baudrate*1.5 - 1) / 2);
            TxBuff[TxByte++] = 0x86;
            TxBuff[TxByte++] = (byte)((Div >> 0) & 0xFF);
            TxBuff[TxByte++] = (byte)((Div >> 8) & 0xFF);
            Status |= Ftdi.Write(TxBuff, TxByte, ref TxByteWrite);
            Thread.Sleep(50);
            TxByte = 0;
            TxByteWrite = 0;
            TxBuff[TxByte++] = 0x85;
            Status |= Ftdi.Write(TxBuff, TxByte, ref TxByteWrite);
            Thread.Sleep(50);
            if (Status != FT_STATUS.FT_OK)
            { 
                return FT_STATUS.FT_IO_ERROR;
            }
            return FT_STATUS.FT_OK;
        }
        public FT_STATUS I2CStart()
        {
            for (int i = 0; i < 4; i++)
            {
                TxBuff[TxByte++] = 0x80;
                TxBuff[TxByte++] = 0x03;
                TxBuff[TxByte++] = 0x03;
            }
            for (int i = 0; i < 4; i++)
            {
                TxBuff[TxByte++] = 0x80;
                TxBuff[TxByte++] = 0x01;
                TxBuff[TxByte++] = 0x03;
            }
            TxBuff[TxByte++] = 0x80;
            TxBuff[TxByte++] = 0x00;
            TxBuff[TxByte++] = 0x03;
            return FT_STATUS.FT_OK;
        }
        public FT_STATUS I2CStop()
        {
            for (int i = 0; i < 4; i++)
            {
                TxBuff[TxByte++] = 0x80;
                TxBuff[TxByte++] = 0x00;
                TxBuff[TxByte++] = 0x03;
            }
            for (int i = 0; i < 4; i++)
            {
                TxBuff[TxByte++] = 0x80;
                TxBuff[TxByte++] = 0x01;
                TxBuff[TxByte++] = 0x03;
            }
            for (int i = 0; i < 4; i++)
            {
                TxBuff[TxByte++] = 0x80;
                TxBuff[TxByte++] = 0x03;
                TxBuff[TxByte++] = 0x03;
            }
            TxBuff[TxByte++] = 0x80;
            TxBuff[TxByte++] = 0x03;
            TxBuff[TxByte++] = 0x00;
            return FT_STATUS.FT_OK;
        }
        public FT_STATUS I2CSendByteWithACK(byte[] TxBuff,int Length)
        {
            for (int i = 0; i < Length; i++)
            {
                this.TxBuff[TxByte++] = 0x80;
                this.TxBuff[TxByte++] = 0x00;
                this.TxBuff[TxByte++] = 0x03;
                this.TxBuff[TxByte++] = MSB_FALLING_EDGE_CLOCK_BYTE_OUT;
                this.TxBuff[TxByte++] = 0x00;
                this.TxBuff[TxByte++] = 0x00;
                this.TxBuff[TxByte++] = TxBuff[i];
                this.TxBuff[TxByte++] = 0x80;
                this.TxBuff[TxByte++] = 0x00;
                this.TxBuff[TxByte++] = 0x01;
                this.TxBuff[TxByte++] = MSB_RISING_EDGE_CLOCK_BIT_IN;
                this.TxBuff[TxByte++] = 0x00;
                this.TxBuff[TxByte++] = 0x87;
            }
            return FT_STATUS.FT_OK;
        }
        public FT_STATUS I2CRecvByteWithACK(int Length)
        {
            for (int i = 0; i < Length; i++)
            {
                this.TxBuff[TxByte++] = 0x80;
                this.TxBuff[TxByte++] = 0x00;
                this.TxBuff[TxByte++] = 0x01;
                this.TxBuff[TxByte++] = MSB_RISING_EDGE_CLOCK_BYTE_IN;
                this.TxBuff[TxByte++] = 0x00;
                this.TxBuff[TxByte++] = 0x00;
                this.TxBuff[TxByte++] = 0x87;
                this.TxBuff[TxByte++] = 0x80;
                this.TxBuff[TxByte++] = (byte)(i == (Length - 1) ? 0x02 : 0x00);
                this.TxBuff[TxByte++] = 0x03;
                this.TxBuff[TxByte++] = MSB_FALLING_EDGE_CLOCK_BIT_IN;
                this.TxBuff[TxByte++] = 0x00;
                this.TxBuff[TxByte++] = 0x87;
            }
            return FT_STATUS.FT_OK;
        }

        public FT_STATUS I2CSetBaudrate(double Baudrate)
        {
            return FT_STATUS.FT_OK;
        }
        private FT_STATUS I2CWrite(bool SlvAddrValid,bool StopValid,byte SlvAddr, byte[] TxBuff, int Length)
        {
            FT_STATUS Status;
            byte[] Addr = new byte[1] { (byte)(SlvAddr << 1) };
            int AckNum = 0;
            TxByte = 0;
            I2CStart();
            if (SlvAddrValid)
            {
                I2CSendByteWithACK(Addr, 1);
                AckNum++;
            }
            I2CSendByteWithACK(TxBuff, Length);
            AckNum += Length;
            if (StopValid)
            {
                I2CStop();
            }
            Ftdi.Write(this.TxBuff, TxByte, ref TxByteWrite);
            do
            {
                Status = Ftdi.GetRxBytesAvailable(ref RxByte);
            } while (RxByte != AckNum && Status == FT_STATUS.FT_OK);
            Ftdi.Read(RxBuff, RxByte, ref RxByteRead);
            return FT_STATUS.FT_OK;
        }
        public FT_STATUS I2CWrite(byte[] TxBuff, int Length)
        {
            return I2CWrite(false,true,0, TxBuff,Length) ;
        }
        public FT_STATUS I2CWrite(byte SlvAddr, byte[] TxBuff, int Length)
        {
            return I2CWrite(true, true, SlvAddr, TxBuff, Length);
        }
        public FT_STATUS I2CWriteWitoutStop(byte[] TxBuff, int Length)
        {
            return I2CWrite(false, false, 0, TxBuff, Length);
        }
        public FT_STATUS I2CWriteWitoutStop(byte SlvAddr, byte[] TxBuff, int Length)
        {
            return I2CWrite(true, false, SlvAddr, TxBuff, Length);
        }
        public byte[] I2CRead(byte SlvAddr, int Length)
        {
            FT_STATUS Status;
            TxByte = 0;
            byte[] Buff;
            Status = Ftdi.GetRxBytesAvailable(ref RxByte);
            I2CStart();
            I2CSendByteWithACK(new byte[] { (byte)((SlvAddr << 1) + 1) }, 1);
            I2CRecvByteWithACK(Length);
            I2CStop();
            Ftdi.Write(this.TxBuff, TxByte, ref TxByteWrite);
            do
            {
                Status = Ftdi.GetRxBytesAvailable(ref RxByte);
            } while (RxByte != Length*2 + 1 && Status == FT_STATUS.FT_OK);
            Ftdi.Read(RxBuff, RxByte, ref RxByteRead);
            Buff = new byte[Length];
            for (int i = 0; i < Length; i++)
            {
                Buff[i] = RxBuff[i * 2 + 1];
            }
            return Buff;
        }
    }
}
