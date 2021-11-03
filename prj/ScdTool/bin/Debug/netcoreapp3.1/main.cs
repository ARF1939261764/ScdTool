//IIIC i2c = new Ftd2xx();
//string[] list = i2c.GetDeviceList();
//
//foreach (string item in list)
//{
//    Console.WriteLine(item);
//}
//
//i2c.Open(0);
//i2c.Init();
//
//i2c.Write(0x50,0x57);
//i2c.Write(0x50,new byte[]{0x00,0x00,0x56},0x03);
//Sleep(100);
//i2c.Write(0x50,new byte[]{0x00,0x00},0x02);
//Console.WriteLine(i2c.Read(0x50).ToString());
//
//byte[] buff = i2c.WriteWithRead(0x50,new byte[]{0x00,0x00},0x02,0x01);
//Console.WriteLine(buff[0].ToString());

//IIC iic = new IIC();
//
//string[] devList = iic.GetDeviceList();
//
//for(int i=0;i<devList.Length;i++)
//{
//    Console.WriteLine("{0:D2}:{1:S}",i,devList[i]);
//}
//
//bool status = iic.Open("\\\\?\\usb#vid_1a86&pid_5512#7&3a944ce5&0&4#{5446f048-98b4-4ef0-96e8-27994bac0d00}");
//if(status == false)
//{
//    throw new System.Exception("打开设备失败");
//}


Task.Run(
    () =>{
        long i = 0;
        while (true)
        {
            i++;
            if(i%100000000 == 0)
            {
                Console.WriteLine("a,{0:D}",i);
            }
        }
    }
);

Task.Run(
    () =>{
        long i = 0;
        while (true)
        {
            i++;
            if(i%100000000 == 0)
            {
                Console.WriteLine("b,{0:D}",i);
            }
        }
    }
);
Task.Run(
    () =>{
        long i = 0;
        while (true)
        {
            i++;
            if(i%100000000 == 0)
            {
                Console.WriteLine("c,{0:D}",i);
            }
        }
    }
);
Task.Run(
    () =>{
        long i = 0;
        while (true)
        {
            i++;
            if(i%100000000 == 0)
            {
                Console.WriteLine("d,{0:D}",i);
            }
        }
    }
);
Task.Run(
    () =>{
        long i = 0;
        while (true)
        {
            i++;
            if(i%100000000 == 0)
            {
                Console.WriteLine("e,{0:D}",i);
            }
        }
    }
);
Task.Run(
    () =>{
        long i = 0;
        while (true)
        {
            i++;
            if(i%100000000 == 0)
            {
                Console.WriteLine("f,{0:D}",i);
            }
        }
    }
);
Task.Run(
    () =>{
        long i = 0;
        while (true)
        {
            i++;
            if(i%100000000 == 0)
            {
                Console.WriteLine("f,{0:D}",i);
            }
        }
    }
);Task.Run(
    () =>{
        long i = 0;
        while (true)
        {
            i++;
            if(i%100000000 == 0)
            {
                Console.WriteLine("f,{0:D}",i);
            }
        }
    }
);Task.Run(
    () =>{
        long i = 0;
        while (true)
        {
            i++;
            if(i%100000000 == 0)
            {
                Console.WriteLine("f,{0:D}",i);
            }
        }
    }
);Task.Run(
    () =>{
        long i = 0;
        while (true)
        {
            i++;
            if(i%100000000 == 0)
            {
                Console.WriteLine("f,{0:D}",i);
            }
        }
    }
);Task.Run(
    () =>{
        long i = 0;
        while (true)
        {
            i++;
            if(i%100000000 == 0)
            {
                Console.WriteLine("f,{0:D}",i);
            }
        }
    }
);Task.Run(
    () =>{
        long i = 0;
        while (true)
        {
            i++;
            if(i%100000000 == 0)
            {
                Console.WriteLine("f,{0:D}",i);
            }
        }
    }
);Task.Run(
    () =>{
        long i = 0;
        while (true)
        {
            i++;
            if(i%100000000 == 0)
            {
                Console.WriteLine("f,{0:D}",i);
            }
        }
    }
);Task.Run(
    () =>{
        long i = 0;
        while (true)
        {
            i++;
            if(i%100000000 == 0)
            {
                Console.WriteLine("f,{0:D}",i);
            }
        }
    }
);
while (true);

