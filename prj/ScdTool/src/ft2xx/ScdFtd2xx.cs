using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using FTD2XX_NET;
using static FTD2XX_NET.FTDI;
using ScdTool;

namespace ScdFtd2xxLib
{
    public class ScdFtd2xx: IIIC
    {
        const double FTDI_CLOCK_FRE = 60e6;
        const byte MSB_FALLING_EDGE_CLOCK_BYTE_OUT = 0x11;
        const byte MSB_FALLING_EDGE_CLOCK_BIT_OUT = 0x12;
        const byte MSB_RISING_EDGE_CLOCK_BYTE_IN = 0x20;
        const byte MSB_RISING_EDGE_CLOCK_BIT_IN = 0x22;
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
        public string[] GetDeviceList()
        {
            FT_STATUS status;
            string[] devlist;
            uint num = 0;
            FT_DEVICE_INFO_NODE[] list;
            Ftdi.GetNumberOfDevices(ref num);
            list = new FT_DEVICE_INFO_NODE[num];
            status = Ftdi.GetDeviceList(list);
            devlist = list.Select(x => x.SerialNumber).ToArray();
            return devlist;
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
            Status |= Ftdi.Write(TxBuff, TxByte, ref TxByteWrite);
            TxByte = 0;
            TxByteWrite = 0;
            TxBuff[TxByte++] = 0x80;
            TxBuff[TxByte++] = 0x03;
            TxBuff[TxByte++] = 0x03;
            Status |= Ftdi.Write(TxBuff, TxByte, ref TxByteWrite);
            TxByte = 0;
            TxByteWrite = 0;
            Div = (UInt16)(FTDI_CLOCK_FRE / (Baudrate * 1.5 - 1) / 2);
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
        public FT_STATUS I2CSendByteWithACK(byte[] TxBuff, int Length)
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
        private FT_STATUS I2CWrite(bool SlvAddrValid, bool StopValid, byte SlvAddr, byte[] TxBuff, int Length)
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
            return I2CWrite(false, true, 0, TxBuff, Length);
        }
        public FT_STATUS I2CWrite(byte SlvAddr, byte[] TxBuff, int Length)
        {
            return I2CWrite(true, true, SlvAddr, TxBuff, Length);
        }
        public FT_STATUS I2CWriteWithoutStop(byte[] TxBuff, int Length)
        {
            return I2CWrite(false, false, 0, TxBuff, Length);
        }
        public FT_STATUS I2CWriteWithoutStop(byte SlvAddr, byte[] TxBuff, int Length)
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
            } while (RxByte != Length * 2 + 1 && Status == FT_STATUS.FT_OK);
            Ftdi.Read(RxBuff, RxByte, ref RxByteRead);
            Buff = new byte[Length];
            for (int i = 0; i < Length; i++)
            {
                Buff[i] = RxBuff[i * 2 + 1];
            }
            return Buff;
        }
        public FT_STATUS JTAGInit(double Baudrate = 100e3)
        {
            UInt16 Div;
            FT_STATUS status = FT_STATUS.FT_OK;
            MPSEEInit();
            TxByte = 0;
            TxBuff[TxByte++] = 0x8A;/*disable div  by 5*/
            TxBuff[TxByte++] = 0x97;
            TxBuff[TxByte++] = 0x8D;
            status |= Ftdi.Write(TxBuff, TxByte, ref TxByteWrite);
            TxByte = 0;
            TxBuff[TxByte++] = 0x80;
            TxBuff[TxByte++] = 0x00;
            TxBuff[TxByte++] = 0x0B;
            status |= Ftdi.Write(TxBuff, TxByte, ref TxByteWrite);
            TxByte = 0x00;
            Div = (UInt16)(FTDI_CLOCK_FRE / (Baudrate - 1) / 2);
            TxBuff[TxByte++] = 0x86;
            TxBuff[TxByte++] = (byte)((Div >> 0) & 0xFF);
            TxBuff[TxByte++] = (byte)((Div >> 8) & 0xFF);
            status |= Ftdi.Write(TxBuff, TxByte, ref TxByteWrite);
            TxByte = 0x00;
            TxBuff[TxByte++] = 0x85;
            status |= Ftdi.Write(TxBuff, TxByte, ref TxByteWrite);
            TxByte = 0x00;
            TxBuff[TxByte++] = 0x4B;
            TxBuff[TxByte++] = 0x07;
            TxBuff[TxByte++] = 0xFF;
            status |= Ftdi.Write(TxBuff, TxByte, ref TxByteWrite);
            return status;
        }
        public FT_STATUS JTAGReset()
        {
            FT_STATUS status = FT_STATUS.FT_OK;
            TxByte = 0x00;
            TxBuff[TxByte++] = 0x4B;
            TxBuff[TxByte++] = 0x07;
            TxBuff[TxByte++] = 0xFF;
            status |= Ftdi.Write(TxBuff, TxByte, ref TxByteWrite);
            return status;
        }
        private byte[] JTAGWriteReadShiftReg(int Type,int Length, byte[] TxBuff)
        {
            int i, t, byte_num, s1_bits;
            FT_STATUS status = FT_STATUS.FT_OK;
            Ftdi.GetRxBytesAvailable(ref RxByte);
            if (RxByte > 0)
            {
                Ftdi.Read(RxBuff, RxByte, ref RxByteRead);
            }
            TxByte = 0x00;
            if (Type == 0)
            {
                this.TxBuff[TxByte++] = 0x4B;
                this.TxBuff[TxByte++] = 0x05;
                this.TxBuff[TxByte++] = 0x0D;
            }
            else
            {
                this.TxBuff[TxByte++] = 0x4B;
                this.TxBuff[TxByte++] = 0x04;
                this.TxBuff[TxByte++] = 0x05;
            }
            byte_num = (Length + 7 - 1) / 8;
            s1_bits = Length - 1;
            for (i = 0; i < byte_num; i++)
            {
                this.TxBuff[TxByte++] = 0x3B;
                this.TxBuff[TxByte++] = (byte)(i == byte_num - 1 ? ((byte)(s1_bits % 8) - 1) : 7);
                this.TxBuff[TxByte++] = TxBuff[i];
            }

            t = (((1 << (s1_bits % 8)) & TxBuff[(Length + 7) / 8 - 1]) == 0x00) ? 0x00 : 0x1;
            this.TxBuff[TxByte++] = 0x6B;
            this.TxBuff[TxByte++] = 0x03;
            this.TxBuff[TxByte++] = (byte)(0x03 | (t << 7));
            this.TxBuff[TxByte++] = 0x87;
            status |= Ftdi.Write(this.TxBuff, TxByte, ref TxByteWrite);
            do
            {
                status = Ftdi.GetRxBytesAvailable(ref RxByte);
            } while (RxByte != byte_num + 1 && status == FT_STATUS.FT_OK);
            Ftdi.Read(RxBuff, RxByte, ref RxByteRead);
            byte[] Buff = new byte[(Length + 7) / 8];
            for (i = 0; i < Buff.Length; i++)
            {
                Buff[i] = RxBuff[i];
            }
            Buff[i - 1] >>= 8 - (s1_bits % 8);
            Buff[i - 1] |= (byte)(((RxBuff[byte_num] & 0x80) == 0x00 ? 0x00 : 0x01) << (s1_bits % 8));
            return Buff;
        }
        public byte[] JTAGWriteReadIR(int IRLength,byte[] TxBuff)
        {
            return JTAGWriteReadShiftReg(0, IRLength, TxBuff);
        }
        public byte[] JTAGWriteReadDR(int DRLength, byte[] TxBuff)
        {
            return JTAGWriteReadShiftReg(0, DRLength, TxBuff);
        }
        bool IIIC.Open(int Index)
        {
            FT_STATUS Status;
            Status = Open((uint)Index);
            I2CInit(400e3);
            return Status == FT_STATUS.FT_OK ? true : false;
        }
        bool IIIC.Close()
        {
            FT_STATUS Status;
            Status = Close();
            return Status == FT_STATUS.FT_OK ? true : false;
        }
        bool IIIC.SetBaudrate(double Baudrate)
        {
            FT_STATUS Status;
            Status = I2CSetBaudrate(Baudrate);
            return Status == FT_STATUS.FT_OK ? true : false;
        }
        bool IIIC.Write(byte SlvAddr, byte[] Buff, int Length)
        {
            FT_STATUS Status;
            Status = I2CWrite(SlvAddr, TxBuff, Length);
            return Status == FT_STATUS.FT_OK ? true : false;
        }
        bool IIIC.Write(byte SlvAddr, byte Data)
        {
            FT_STATUS Status;
            Status = I2CWrite(SlvAddr, new byte[1] { Data }, 1);
            return Status == FT_STATUS.FT_OK ? true : false;
        }
        bool IIIC.Read(byte SlvAddr, byte[] Buff, int Length)
        {
            byte[] rxbuff = I2CRead(SlvAddr, Length);
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
            buff = I2CRead(SlvAddr, 1);
            if (buff == null)
            {
                return false;
            }
            Data = buff[0];
            return true;
        }
        string[] IIIC.GetDeviceList()
        {
            return GetDeviceList();
        }
        bool IIIC.WriteWithRead(byte SlvAddr, byte[] TxBuff, int TxLength, byte[] RxBuff, int RxLength)
        {
            I2CWriteWithoutStop(SlvAddr, TxBuff, TxLength);
            byte[] rxbuff = I2CRead(SlvAddr, RxLength);
            if (rxbuff == null || rxbuff.Length != RxLength)
            {
                return false;
            }
            for (int i = 0; i < RxLength; i++)
            {
                RxBuff[i] = rxbuff[i];
            }
            return true;
        }
    }
}
