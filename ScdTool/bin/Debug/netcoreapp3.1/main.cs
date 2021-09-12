using System;
using System.IO;
using static System.Threading.Thread;

IIIC i2c = new ScdFtd2xx();

string[] list = i2c.GetDeviceList();

foreach (string item in list)
{
    Console.WriteLine(item);
}



i2c.Open(0);
i2c.Init();

i2c.Write(0x50,0x57);
i2c.Write(0x50,new byte[]{0x00,0x00,0x56},0x03);
Sleep(100);
i2c.Write(0x50,new byte[]{0x00,0x00},0x02);
Console.WriteLine(i2c.Read(0x50).ToString());

byte[] buff = i2c.WriteWithRead(0x50,new byte[]{0x00,0x00},0x02,0x01);
Console.WriteLine(buff[0].ToString());
