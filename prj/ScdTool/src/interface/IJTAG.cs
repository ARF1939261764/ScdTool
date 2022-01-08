namespace ScdTool
{
    public interface IJTAG
    {
        /*获取设备列表*/
        public string[] GetDeviceList();
        /*打开设备(通过设备序号)*/
        public void Reset();
        public byte[] WriteWithReadIR(byte[] Buff, int Length);
        public byte[] WriteWithReadDR(byte[] Buff,int Length);
    }
}