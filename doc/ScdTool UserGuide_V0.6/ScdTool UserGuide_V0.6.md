```c#
class IIC
{
    /*获取设备列表*/
    public string[] GetDeviceList();
	/*打开设备(通过设备序号)*/
    public bool Open(int Index);
	/*打开设备(通过设备名)*/
    public bool Open(string Name);
    /*设置波特率*/
    public bool SetBaudrate(double Baudrate);
    /*关闭设备*/
    public bool Close();
	/*读一组byte数据*/
    public bool Read(byte SlvAddr, byte[] Buff, int Length);
	/*读一个byte数据*/
    public bool Read(byte SlvAddr,ref byte Data);
	/*写一组byte数据*/
    public bool Write(byte SlvAddr, byte[] Buff, int Length);
	/*写一个byte数据*/
    public bool Write(byte SlvAddr, byte Data);
    /*写一组数据后再读取一组数据*/
    public bool WriteWithRead(byte SlvAddr, byte[] TxBuff, int TxLength, byte[] RxBuff, int RxLength);
}
```

```c
/*生成设备对象*/
void* IIC_CreateDeviceObject(void);
/*设置上下文*/
void* IIC_SetContext(void* DeviceObject);
void* IIC_GetContext(void);
/*获取设备列表*/
int IIC_GetDeviceList(char[][IIC_DEVICE_NAME_MAX_LEN] DeviceList,uint32_t MaxNumber);
/*打开设备(通过设备序号)*/
int IIC_Open(int Index);
/*打开设备(通过设备名)*/
int IIC_Open(const char* Name);
/*设置波特率*/
int IIC_SetBaudrate(double Baudrate);
/*关闭设备*/
int IIC_Close();
/*读一组byte数据*/
int IIC_Read(uint8_t SlvAddr, uint8_t* Buff, int Length);
/*读一个byte数据*/
int IIC_Read(uint8_t SlvAddr, uint8_t* Data);
/*写一组byte数据*/
int IIC_Write(uint8_t SlvAddr, const uint8_t* Buff, int Length);
/*写一个byte数据*/
int IIC_Write(uint8_t SlvAddr, uint8_t Data);
/*写一组数据后再读取一组数据*/
int IIC_WriteWithRead(uint8_t SlvAddr, const uint8_t* TxBuff, int TxLength, uint8_t* RxBuff, int RxLength);
```

