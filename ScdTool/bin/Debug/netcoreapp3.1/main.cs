using System;
using System.IO;
using static System.Threading.Thread;

class Program
{
    public void Main(string[] args)
    {
        ScdFtd2xx Ftdi = new ScdFtd2xx();
        FT_DEVICE_INFO_NODE[] list;
        FT_STATUS Status;
        byte[] Buff;
        list = Ftdi.GetDeviceList();
        for(int i=0;i<list.Length;i++)
        {
            Console.WriteLine("{0:D}:{1:S}",i,list[i].Description);
        }
        Status = Ftdi.Open(0);
        if(Status == FT_STATUS.FT_OK)
        {
            Console.WriteLine("打开设备成功");
        }
        Ftdi.I2CInit();
        Ftdi.I2CWrite(0x50,new byte[] {0x01,0x00,0x55,0xAA,0x12,0x62 },6);
        Sleep(10);
        Ftdi.I2CWriteWitoutStop(0x50,new byte[] { 0x01, 0x00 },2);
        Buff = Ftdi.I2CRead(0x50, 4);
        for(int i=0;i<4;i++)
        {
            Console.WriteLine("{0:X}",Buff[i]);
        }
    }
    
}
