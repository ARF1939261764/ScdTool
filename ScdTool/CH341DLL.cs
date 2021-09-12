using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CH341Lib
{
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 8)]
    public struct mUSB_SETUP_PKT
    {
        [FieldOffset(0)]
        public byte mUspReqType;
        [FieldOffset(1)]
        public byte mUspRequest;
        [FieldOffset(2)]
        public byte mUspValueLow;
        [FieldOffset(3)]
        public byte mUspValueHigh;
        [FieldOffset(2)]
        public UInt16 mUspValue;
        [FieldOffset(4)]
        public byte mUspIndexLow;
        [FieldOffset(5)]
        public byte mUspIndexHigh;
        [FieldOffset(4)]
        public UInt16 mUspIndex;
        [FieldOffset(6)]
        public UInt16 mLength;
    };

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
    public struct mWIN32_COMMAND
    {
        [FieldOffset(0)]
        public UInt32 mFunction;
        [FieldOffset(0)]
        public Int32 mStatus;
        [FieldOffset(4)]
        public UInt32 mLength;
        [FieldOffset(8)]
        mUSB_SETUP_PKT mSetupPkt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = CH341DLL.mCH341_PACKET_LENGTH),FieldOffset(8)]
        public byte[] mBuffer;
    }
    enum EEPROM_TYPE
    {
        ID_24C01,
        ID_24C02,
        ID_24C04,
        ID_24C08,
        ID_24C16,
        ID_24C32,
        ID_24C64,
        ID_24C128,
        ID_24C256,
        ID_24C512,
        ID_24C1024,
        ID_24C2048,
        ID_24C4096
    }
    class CH341DLL
    {
        public const int mCH341_PACKET_LENGTH = 32;

        public delegate void mPCH341_INT_ROUTINE(UInt32 iStatus);

        public delegate void mPCH341_NOTIFY_ROUTINE(UInt32 iEventStatus);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341OpenDevice")]
        extern public static IntPtr OpenDevice(UInt32 iIndex);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341CloseDevice")]
        extern public static void CloseDevice(UInt32 iIndex);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341GetVersion")]
        extern public static UInt32 GetVersion();

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341DriverCommand")]
        extern public static UInt32 DriverCommand(UInt32 iIndex, ref mWIN32_COMMAND ioCommand);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341GetDrvVersion")]
        extern public static UInt32 GetDrvVersion();

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341ResetDevice")]
        extern public static bool ResetDevice(UInt32 iIndex);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341GetDeviceDescr")]
        extern public static bool GetDeviceDescr(UInt32 iIndex, byte[] oBuffer,ref UInt32 ioLength);
        
        [DllImport("CH341DLL.DLL", EntryPoint = "CH341GetConfigDescr")]
        extern public static bool GetConfigDescr(UInt32 iIndex, byte[] oBuffer, ref UInt32 ioLength);
        
        [DllImport("CH341DLL.DLL", EntryPoint = "CH341SetIntRoutine")]
        extern public static bool SetIntRoutine(UInt32 iIndex, mPCH341_INT_ROUTINE iIntRoutine);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341ReadInter")]
        extern public static bool CH341ReadInter(UInt32 iIndex, ref UInt32 iStatus);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341AbortInter")]
        extern public static bool AbortInter(UInt32 iIndex);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341SetParaMode")]
        extern public static bool SetParaMode(UInt32 iIndex,UInt32 iMode);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341InitParallel")]
        extern public static bool InitParallel(UInt32 iIndex, UInt32 iMode);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341ReadData0")]
        extern public static bool ReadData0(UInt32 iIndex,byte[] oBuffer,ref UInt32 ioLength);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341ReadData1")]
        extern public static bool ReadData1(UInt32 iIndex, byte[] oBuffer, ref UInt32 ioLength);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341AbortRead")]
        extern public static bool AbortRead(UInt32 iIndex);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341WriteData0")]
        extern public static bool WriteData0(UInt32 iIndex, byte[] iBuffer, ref UInt32 ioLength);
        
        [DllImport("CH341DLL.DLL", EntryPoint = "CH341WriteData1")]
        extern public static bool WriteData1(UInt32 iIndex, byte[] iBuffer, ref UInt32 ioLength);
        
        [DllImport("CH341DLL.DLL", EntryPoint = "CH341AbortWrite")]
        extern public static bool AbortWrite(UInt32 iIndex);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341GetStatus")]
        extern public static bool GetStatus(UInt32 iIndex,ref UInt32 iStatus);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341ReadI2C")]
        extern public static bool ReadI2C(UInt32 iIndex,byte iDevice,byte iAddr,ref byte oByte);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341WriteI2C")]
        extern public static bool WriteI2C(UInt32 iIndex, byte iDevice, byte iAddr,byte iByte);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341EppReadData")]
        extern public static bool EppReadData(UInt32 iIndex,byte[] oBuffer,ref UInt32 ioLength);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341EppReadAddr")]
        extern public static bool EppReadAddr(UInt32 iIndex, byte[] oBuffer, ref UInt32 ioLength);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341EppWriteData")]
        extern public static bool EppWriteData(UInt32 iIndex, byte[] iBuffer, ref UInt32 ioLength);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341EppWriteAddr")]
        extern public static bool EppWriteAddr(UInt32 iIndex, byte[] iBuffer, ref UInt32 ioLength);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341EppSetAddr")]
        extern public static bool EppSetAddr(UInt32 iIndex, byte iAddr);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341MemReadAddr0")]
        extern public static bool MemReadAddr0(UInt32 iIndex, byte[] oBuffer, ref UInt32 ioLength);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341MemReadAddr1")]
        extern public static bool MemReadAddr1(UInt32 iIndex, byte[] oBuffer, ref UInt32 ioLength);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341MemWriteAddr0")]
        extern public static bool MemWriteAddr0(UInt32 iIndex, byte[] iBuffer, ref UInt32 ioLength);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341MemWriteAddr1")]
        extern public static bool MemWriteAddr1(UInt32 iIndex, byte[] iBuffer, ref UInt32 ioLength);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341SetExclusive")]
        extern public static bool SetExclusive(UInt32 iIndex, UInt32 iExclusive);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341SetTimeout")]
        extern public static bool SetTimeout(UInt32 iIndex,UInt32 iWriteTimeout,UInt32 iReadTimeout);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341ReadData")]
        extern public static bool ReadData(UInt32 iIndex,byte[] oBuffer,ref UInt32 ioLength);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341WriteData")]
        extern public static bool WriteData(UInt32 iIndex, byte[] iBuffer, ref UInt32 ioLength);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341GetDeviceName")]
        extern public static IntPtr GetDeviceName(UInt32 iIndex);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341GetVerIC")]
        extern public static UInt32 GetVerIC(UInt32 iIndex);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341FlushBuffer")]
        extern public static bool FlushBuffer(UInt32 iIndex);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341WriteRead")]
        extern public static bool WriteRead(
            UInt32 iIndex, 
            UInt32 iWriteLength,
            byte[] iWriteBuffer, 
            UInt32 iReadStep, 
            UInt32 iReadTimes,
            ref UInt32 oReadLength,
            byte[] oReadBuffer);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341SetStream")]
        extern public static bool SetStream(UInt32 iIndex, UInt32 iMode);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341SetDelaymS")]
        extern public static bool SetDelaymS(UInt32 iIndex, UInt32 iDelay);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341StreamI2C")]
        extern public static bool StreamI2C(UInt32 iIndex, UInt32 iWriteLength,byte[] iWriteBuffer,UInt32 iReadLength,byte[] oReadBuffer);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341ReadEEPROM")]
        extern public static bool ReadEEPROM(
            UInt32 iIndex, 
            EEPROM_TYPE iEepromID, 
            UInt32 iAddr, 
            UInt32 iLength,
            byte[] oBuffer);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341WriteEEPROM")]
        extern public static bool WriteEEPROM(
            UInt32 iIndex,
            EEPROM_TYPE iEepromID,
            UInt32 iAddr,
            UInt32 iLength,
            byte[] iBuffer);
        [DllImport("CH341DLL.DLL", EntryPoint = "CH341GetInput")]
        extern public static bool GetInput(UInt32 iIndex,ref UInt32 iStatus);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341SetOutput")]
        extern public static bool SetOutput(UInt32 iIndex, UInt32 iEnable, UInt32 iSetDirOut, UInt32 iSetDataOut);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341Set_D5_D0")]
        extern public static bool Set_D5_D0(UInt32 iIndex,UInt32 iSetDirOut,UInt32 iSetDataOut);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341StreamSPI3")]
        extern public static bool StreamSPI3(UInt32 iIndex, UInt32 iChipSelect, UInt32 iLength ,byte[] ioBuffer);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341StreamSPI4")]
        extern public static bool StreamSPI4(UInt32 iIndex, UInt32 iChipSelect, UInt32 iLength, byte[] ioBuffer);
        
        [DllImport("CH341DLL.DLL", EntryPoint = "CH341StreamSPI5")]
        extern public static bool StreamSPI5(UInt32 iIndex, UInt32 iChipSelect, UInt32 iLength, byte[] ioBuffer);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341BitStreamSPI")]
        extern public static bool BitStreamSPI(UInt32 iIndex,UInt32 iLength,byte[] ioBuffer);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341SetBufUpload")]
        extern public static bool SetBufUpload(UInt32 iIndex, UInt32 iEnableOrClear);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341QueryBufUpload")]
        extern public static Int32 QueryBufUpload(UInt32 iIndex);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341SetBufDownload")]
        extern public static bool SetBufDownload(UInt32 iIndex,UInt32 iEnableOrClear);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341QueryBufDownload")]
        extern public static Int32 QueryBufDownload(UInt32 iIndex);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341ResetInter")]
        extern public static bool ResetInter(UInt32 iIndex);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341ResetRead")]
        extern public static bool ResetRead(UInt32 iIndex);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341ResetWrite")]
        extern public static bool ResetWrite(UInt32 iIndex);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341SetDeviceNotify")]
        extern public static bool SetDeviceNotify(
            UInt32 iIndex,
            byte[] iDeviceID, 
            mPCH341_NOTIFY_ROUTINE iNotifyRoutine);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341SetupSerial")]
        extern public static bool SetupSerial(
            UInt32 iIndex,
            UInt32 iParityMode,
            UInt32 iBaudRate);
        [DllImport("CH341DLL.DLL", EntryPoint = "CH341OpenDeviceEx")]
        extern public static IntPtr OpenDeviceEx(UInt32 iIndex);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341CloseDeviceEx")]
        extern public static void CloseDeviceEx(UInt32 iIndex);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341GetDeviceNameEx")]
        extern public static IntPtr GetDeviceNameEx(UInt32 iIndex);

        [DllImport("CH341DLL.DLL", EntryPoint = "CH341SetDeviceNotifyEx")]
        extern public static bool SetDeviceNotifyEx(UInt32 iIndex,byte[] iDeviceID, mPCH341_NOTIFY_ROUTINE iNotifyRoutine);
    }
}
