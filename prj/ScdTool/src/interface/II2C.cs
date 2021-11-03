using System;
using System.Collections.Generic;
using System.Text;

namespace ScdTool
{
    public interface IIIC
    {
        /*获取设备列表*/
        public string[] GetDeviceList();
        /*打开设备(通过设备序号)*/
        public bool Open(int Index);
        /*设置波特率*/
        public bool SetBaudrate(double Baudrate);
        /*关闭设备*/
        public bool Close();
        /*读一组byte数据*/
        public bool Read(byte SlvAddr, byte[] Buff, int Length);
        /*读一个byte数据*/
        public bool Read(byte SlvAddr, ref byte Data);
        /*写一组byte数据*/
        public bool Write(byte SlvAddr, byte[] Buff, int Length);
        /*写一个byte数据*/
        public bool Write(byte SlvAddr, byte Data);
        /*写一组数据后再读取一组数据*/
        public bool WriteWithRead(byte SlvAddr, byte[] TxBuff, int TxLength, byte[] RxBuff, int RxLength);
    }
}
