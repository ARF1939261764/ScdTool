namespace ScdTool
{
    public interface IJTAG
    {
        /*��ȡ�豸�б�*/
        public string[] GetDeviceList();
        /*���豸(ͨ���豸���)*/
        public void Reset();
        public byte[] WriteWithReadIR(byte[] Buff, int Length);
        public byte[] WriteWithReadDR(byte[] Buff,int Length);
    }
}