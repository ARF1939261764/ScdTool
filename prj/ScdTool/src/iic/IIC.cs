using System;
using System.Collections.Generic;
using System.Text;
using CH341Lib;
using Ftd2xx = ScdFtd2xxLib.ScdFtd2xx;
using System.Linq;

namespace ScdTool
{
    struct IICDeviceListItem
    {
        public Type DeviceObjectType;
        public string Name;
        public int Index;
    }
    public class IIC : IIIC
    {
        private List<IICDeviceListItem> IICDeviceList;
        Type[] DeviceObjectTypeList = new Type[] {
            typeof(Ftd2xx),
            typeof(CH341)
        };
        IIIC dev;
        public IIC()
        {
            IICDeviceList = new List<IICDeviceListItem>();
        }
        public bool Close()
        {
            return dev.Close();
        }

        public string[] GetDeviceList()
        {
            IICDeviceList.Clear();
            foreach (Type deviceObject in DeviceObjectTypeList)
            {
                IIIC iic = System.Activator.CreateInstance(deviceObject) as IIIC;
                string[] devList = iic.GetDeviceList();
                int index = 0;
                foreach (string item in devList)
                {
                    IICDeviceList.Add(new IICDeviceListItem()
                    {
                        DeviceObjectType = deviceObject,
                        Name = item,
                        Index = index
                    });
                }
            }
            return IICDeviceList.Select(u => u.Name).ToArray();
        }
        public bool Open(int Index)
        {
            if (Index >= IICDeviceList.Count)
            {
                return false;
            }
            dev = System.Activator.CreateInstance(IICDeviceList[Index].DeviceObjectType) as IIIC;
            bool status = dev.Open(IICDeviceList[Index].Index);
            return status;
        }
        public bool Open(string Name)
        {
            int index = IICDeviceList.FindIndex(item => item.Name == Name);
            if (index < 0)
            {
                return false;
            }
            return Open(index);
        }

        public bool Read(byte SlvAddr, byte[] Buff, int Length)
        {
            return dev.Read(SlvAddr, Buff, Length);
        }

        public bool Read(byte SlvAddr, ref byte Data)
        {
            return dev.Read(SlvAddr, ref Data);
        }

        public bool SetBaudrate(double Baudrate)
        {
            return dev.SetBaudrate(Baudrate);
        }

        public bool Write(byte SlvAddr, byte[] Buff, int Length)
        {
            return dev.Write(SlvAddr, Buff, Length);
        }

        public bool Write(byte SlvAddr, byte Data)
        {
            return dev.Write(SlvAddr, Data);
        }
        public bool WriteWithRead(byte SlvAddr, byte[] TxBuff, int TxLength, byte[] RxBuff, int RxLength)
        {
            return dev.WriteWithRead(SlvAddr, TxBuff, TxLength, RxBuff,RxLength);
        }
    }
}
