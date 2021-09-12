using System;
using System.Collections.Generic;
using System.Text;

namespace ScdTool
{
    public interface IIIC
    {
        public string[] GetDeviceList();
        public bool Open(uint idx);
        public bool Close();
        public bool Init(double Baudrate = 100e3);
        public bool SetBaudrate(double Baudrate);
        public bool Write(byte SlvAddr, byte[] TxBuff, int Length);
        public bool Write(byte SlvAddr, byte data);
        public byte[] Read(byte SlvAddr, int Length);
        public byte Read(byte SlvAddr);
        public byte[] WriteWithRead(byte SlvAddr, byte[] TxBuff, int TxLength, int RxLength);
    }
}
