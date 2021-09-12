// 2005.03.25, 2005.04.28, 2005.07.18, 
// 2005.07.28, 2005.09.19, 2006.06.08
//****************************************
//**  Copyright  (C)  USBIO  1999-2005  **
//**  WEB:  http://WWW.USB-I2C-SPI.COM  **
//****************************************
//**  DLL for USB interface chip USB2I2C**
//**  C, VC5.0                          **
//****************************************
//
// USB接口芯片USB2I2C并口应用层接口库 V1.9
// USBIO TECH.       作者: USBIO  2006.08
// USBIOX.DLL  V1.9
// 运行环境: Windows 98/ME, Windows 2000/XP
// USB <=> I2C @40KHZ,100KHZ,400KHZ,75KHZ.
//

#ifndef		_mUSBIOX_DLL_H
#define		_mUSBIOX_DLL_H

#ifdef __cplusplus
extern "C" {
#endif

#define		mOFFSET( s, m )			( (ULONG) & ( ( ( s * ) 0 ) -> m ) )	// 定义获取结构成员相对偏移地址的宏

#ifndef		max
#define		max( a, b )				( ( ( a ) > ( b ) ) ? ( a ) : ( b ) )	// 较大值
#endif

#ifndef		min
#define		min( a, b )				( ( ( a ) < ( b ) ) ? ( a ) : ( b ) )	// 较小值
#endif

#ifdef		ExAllocatePool
#undef		ExAllocatePool						// 删除带TAG的内存分配
#endif

#ifndef		NTSTATUS
typedef		LONG	NTSTATUS;					// 返回状态
#endif


typedef	struct	_USB_SETUP_PKT {				// USB控制传输的建立阶段的数据请求包结构
	UCHAR			mUspReqType;				// 00H 请求类型
	UCHAR			mUspRequest;				// 01H 请求代码
	union	{
		struct	{
			UCHAR	mUspValueLow;				// 02H 值参数低字节
			UCHAR	mUspValueHigh;				// 03H 值参数高字节
		};
		USHORT		mUspValue;					// 02H-03H 值参数
	};
	union	{
		struct	{
			UCHAR	mUspIndexLow;				// 04H 索引参数低字节
			UCHAR	mUspIndexHigh;				// 05H 索引参数高字节
		};
		USHORT		mUspIndex;					// 04H-05H 索引参数
	};
	USHORT			mLength;					// 06H-07H 数据阶段的数据长度
} mUSB_SETUP_PKT, *mPUSB_SETUP_PKT;


//#define		mmUSBIO_PACKET_LENGTH_PACKET_LENGTH	32			// mUSBIO_PACKET_LENGTH支持的数据包的长度
//#define		mmUSBIO_PACKET_LENGTH_PKT_LEN_SHORT	8			// mUSBIO_PACKET_LENGTH支持的短数据包的长度
#define		mUSBIO_PACKET_LENGTH	32			// USB2I2C支持的数据包的长度
#define		mUSBIO_PKT_LEN_SHORT	8			// USB2I2C支持的短数据包的长度


typedef	struct	_WIN32_COMMAND {				// 定义WIN32命令接口结构
	union	{
		ULONG		mFunction;					// 输入时指定功能代码或者管道号
		NTSTATUS	mStatus;					// 输出时返回操作状态
	};
	ULONG			mLength;					// 存取长度,返回后续数据的长度
	union	{
		mUSB_SETUP_PKT	mSetupPkt;				// USB控制传输的建立阶段的数据请求
		UCHAR			mBuffer[ mUSBIO_PACKET_LENGTH ];	// 数据缓冲区,长度为0至255B
	};
} mWIN32_COMMAND, *mPWIN32_COMMAND;


// WIN32应用层接口命令
#define		IOCTL_mUSBIO_PACKET_LENGTH_COMMAND		( FILE_DEVICE_UNKNOWN << 16 | FILE_ANY_ACCESS << 14 | 0x0f34 << 2 | METHOD_BUFFERED )	// 专用接口

#define		mWIN32_COMMAND_HEAD		mOFFSET( mWIN32_COMMAND, mBuffer )	// WIN32命令接口的头长度

#define		mmUSBIO_PACKET_LENGTH_MAX_NUMBER		16			// 最多同时连接的mUSBIO_PACKET_LENGTH数

#define		mMAX_BUFFER_LENGTH		0x1000		// 数据缓冲区最大长度4096

#define		mMAX_COMMAND_LENGTH		( mWIN32_COMMAND_HEAD + mMAX_BUFFER_LENGTH )	// 最大数据长度加上命令结构头的长度

#define		mDEFAULT_BUFFER_LEN		0x0400		// 数据缓冲区默认长度1024

#define		mDEFAULT_COMMAND_LEN	( mWIN32_COMMAND_HEAD + mDEFAULT_BUFFER_LEN )	// 默认数据长度加上命令结构头的长度


// mUSBIO_PACKET_LENGTH端点地址
#define		mmUSBIO_PACKET_LENGTH_ENDP_INTER_UP	0x81		// mUSBIO_PACKET_LENGTH的中断数据上传端点的地址
#define		mmUSBIO_PACKET_LENGTH_ENDP_INTER_DOWN	0x01		// mUSBIO_PACKET_LENGTH的中断数据下传端点的地址
#define		mmUSBIO_PACKET_LENGTH_ENDP_DATA_UP		0x82		// mUSBIO_PACKET_LENGTH的数据块上传端点的地址
#define		mmUSBIO_PACKET_LENGTH_ENDP_DATA_DOWN	0x02		// mUSBIO_PACKET_LENGTH的数据块下传端点的地址


// 设备层接口提供的管道操作命令
#define		mPipeDeviceCtrl			0x00000004	// mUSBIO_PACKET_LENGTH的综合控制管道
#define		mPipeInterUp			0x00000005	// mUSBIO_PACKET_LENGTH的中断数据上传管道
#define		mPipeDataUp				0x00000006	// mUSBIO_PACKET_LENGTH的数据块上传管道
#define		mPipeDataDown			0x00000007	// mUSBIO_PACKET_LENGTH的数据块下传管道

// 应用层接口的功能代码
#define		mFuncNoOperation		0x00000000	// 无操作
#define		mFuncGetVersion			0x00000001	// 获取驱动程序版本号
#define		mFuncGetConfig			0x00000002	// 获取USB设备配置描述符
#define		mFuncSetTimeout			0x00000009	// 设置USB通讯超时
#define		mFuncSetExclusive		0x0000000b	// 设置独占使用
#define		mFuncResetDevice		0x0000000c	// 复位USB设备
#define		mFuncResetPipe			0x0000000d	// 复位USB管道
#define		mFuncAbortPipe			0x0000000e	// 取消USB管道的数据请求

// mUSBIO_PACKET_LENGTH并口专用的功能代码
#define		mFuncSetParaMode		0x0000000f	// 设置并口模式
#define		mFuncReadData0			0x00000010	// 从并口读取数据块0
#define		mFuncReadData1			0x00000011	// 从并口读取数据块1
#define		mFuncWriteData0			0x00000012	// 向并口写入数据块0
#define		mFuncWriteData1			0x00000013	// 向并口写入数据块1
#define		mFuncWriteRead			0x00000014	// 先输出再输入
#define		mFuncBufferMode			0x00000020	// 设定缓冲上传模式及查询缓冲区中的数据长度
#define		mFuncBufferModeDn		0x00000021	// 设定缓冲下传模式及查询缓冲区中的数据长度


// USB设备标准请求代码
#define		mUSB_CLR_FEATURE		0x01
#define		mUSB_SET_FEATURE		0x03
#define		mUSB_GET_STATUS			0x00
#define		mUSB_SET_ADDRESS		0x05
#define		mUSB_GET_DESCR			0x06
#define		mUSB_SET_DESCR			0x07
#define		mUSB_GET_CONFIG			0x08
#define		mUSB_SET_CONFIG			0x09
#define		mUSB_GET_INTERF			0x0a
#define		mUSB_SET_INTERF			0x0b
#define		mUSB_SYNC_FRAME			0x0c

// mUSBIO_PACKET_LENGTH控制传输的厂商专用请求类型
#define		mmUSBIO_PACKET_LENGTH_VENDOR_READ		0xC0		// 通过控制传输实现的mUSBIO_PACKET_LENGTH厂商专用读操作
#define		mmUSBIO_PACKET_LENGTH_VENDOR_WRITE		0x40		// 通过控制传输实现的mUSBIO_PACKET_LENGTH厂商专用写操作

// mUSBIO_PACKET_LENGTH控制传输的厂商专用请求代码
#define		mmUSBIO_PACKET_LENGTH_PARA_INIT		0xB1		// 初始化并口
#define		mmUSBIO_PACKET_LENGTH_I2C_STATUS		0x52		// 获取I2C接口的状态
#define		mmUSBIO_PACKET_LENGTH_I2C_COMMAND		0x53		// 发出I2C接口的命令

// mUSBIO_PACKET_LENGTH并口操作命令代码
#define		mmUSBIO_PACKET_LENGTH_PARA_CMD_R0		0xAC		// 从并口读数据0
#define		mmUSBIO_PACKET_LENGTH_PARA_CMD_R1		0xAD		// 从并口读数据1
#define		mmUSBIO_PACKET_LENGTH_PARA_CMD_W0		0xA6		// 向并口写数据0
#define		mmUSBIO_PACKET_LENGTH_PARA_CMD_W1		0xA7		// 向并口写数据1
#define		mmUSBIO_PACKET_LENGTH_PARA_CMD_STS		0xA0		// 获取并口状态

// mUSBIO_PACKET_LENGTHA并口操作命令代码
#define		mmUSBIO_PACKET_LENGTHA_CMD_SET_OUTPUT	0xA1		// 设置并口输出
#define		mmUSBIO_PACKET_LENGTHA_CMD_IO_ADDR		0xA2		// MEM带地址读写/输入输出,从次字节开始为命令流
#define		mmUSBIO_PACKET_LENGTHA_CMD_PRINT_OUT	0xA3		// PRINT兼容打印方式输出,从次字节开始为数据流
#define		mmUSBIO_PACKET_LENGTHA_CMD_SPI_STREAM	0xA8		// SPI接口的命令包,从次字节开始为数据流
#define		mmUSBIO_PACKET_LENGTHA_CMD_SIO_STREAM	0xA9		// SIO接口的命令包,从次字节开始为数据流
#define		mmUSBIO_PACKET_LENGTHA_CMD_I2C_STREAM	0xAA		// I2C接口的命令包,从次字节开始为I2C命令流
#define		mmUSBIO_PACKET_LENGTHA_CMD_UIO_STREAM	0xAB		// UIO接口的命令包,从次字节开始为命令流

// mUSBIO_PACKET_LENGTHA控制传输的厂商专用请求代码
#define		mmUSBIO_PACKET_LENGTHA_BUF_CLEAR		0xB2		// 清除未完成的数据
#define		mmUSBIO_PACKET_LENGTHA_I2C_CMD_X		0x54		// 发出I2C接口的命令,立即执行
#define		mmUSBIO_PACKET_LENGTHA_DELAY_MS		0x5E		// 以亳秒为单位延时指定时间
#define		mmUSBIO_PACKET_LENGTHA_GET_VER			0x5F		// 获取芯片版本

#define		mmUSBIO_PACKET_LENGTH_EPP_IO_MAX		( mUSBIO_PACKET_LENGTH - 1 )	// mUSBIO_PACKET_LENGTH在EPP/MEM方式下单次读写数据块的最大长度
#define		mmUSBIO_PACKET_LENGTHA_EPP_IO_MAX		0xFF		// mUSBIO_PACKET_LENGTHA在EPP/MEM方式下单次读写数据块的最大长度

#define		mmUSBIO_PACKET_LENGTHA_CMD_IO_ADDR_W	0x00		// MEM带地址读写/输入输出的命令流:写数据,位6-位0为地址,下一个字节为待写数据
#define		mmUSBIO_PACKET_LENGTHA_CMD_IO_ADDR_R	0x80		// MEM带地址读写/输入输出的命令流:读数据,位6-位0为地址,读出数据一起返回

#define		mmUSBIO_PACKET_LENGTHA_CMD_I2C_STM_STA	0x74		// I2C接口的命令流:产生起始位
#define		mmUSBIO_PACKET_LENGTHA_CMD_I2C_STM_STO	0x75		// I2C接口的命令流:产生停止位
#define		mmUSBIO_PACKET_LENGTHA_CMD_I2C_STM_OUT	0x80		// I2C接口的命令流:输出数据,位5-位0为长度,后续字节为数据,0长度则只发送一个字节并返回应答
#define		mmUSBIO_PACKET_LENGTHA_CMD_I2C_STM_IN	0xC0		// I2C接口的命令流:输入数据,位5-位0为长度,0长度则只接收一个字节并发送无应答
#define		mmUSBIO_PACKET_LENGTHA_CMD_I2C_STM_MAX	( min( 0x3F, mUSBIO_PACKET_LENGTH ) )	// I2C接口的命令流单个命令输入输出数据的最大长度
#define		mmUSBIO_PACKET_LENGTHA_CMD_I2C_STM_SET	0x60		// I2C接口的命令流:设置参数,位2=SPI的I/O数(0=单入单出,1=双入双出),位1位0=I2C速度(00=低速,01=标准,10=快速,11=高速)
#define		mmUSBIO_PACKET_LENGTHA_CMD_I2C_STM_US	0x40		// I2C接口的命令流:以微秒为单位延时,位3-位0为延时值
#define		mmUSBIO_PACKET_LENGTHA_CMD_I2C_STM_MS	0x50		// I2C接口的命令流:以亳秒为单位延时,位3-位0为延时值
#define		mmUSBIO_PACKET_LENGTHA_CMD_I2C_STM_DLY	0x0F		// I2C接口的命令流单个命令延时的最大值
#define		mmUSBIO_PACKET_LENGTHA_CMD_I2C_STM_END	0x00		// I2C接口的命令流:命令包提前结束

#define		mmUSBIO_PACKET_LENGTHA_CMD_UIO_STM_IN	0x00		// UIO接口的命令流:输入数据D7-D0
#define		mmUSBIO_PACKET_LENGTHA_CMD_UIO_STM_DIR	0x40		// UIO接口的命令流:设定I/O方向D5-D0,位5-位0为方向数据
#define		mmUSBIO_PACKET_LENGTHA_CMD_UIO_STM_OUT	0x80		// UIO接口的命令流:输出数据D5-D0,位5-位0为数据
#define		mmUSBIO_PACKET_LENGTHA_CMD_UIO_STM_US	0xC0		// UIO接口的命令流:以微秒为单位延时,位5-位0为延时值
#define		mmUSBIO_PACKET_LENGTHA_CMD_UIO_STM_END	0x20		// UIO接口的命令流:命令包提前结束


// mUSBIO_PACKET_LENGTH并口工作模式
#define		mmUSBIO_PACKET_LENGTH_PARA_MODE_EPP	0x00		// mUSBIO_PACKET_LENGTH并口工作模式为EPP方式
#define		mmUSBIO_PACKET_LENGTH_PARA_MODE_EPP17	0x00		// mUSBIO_PACKET_LENGTHA并口工作模式为EPP方式V1.7
#define		mmUSBIO_PACKET_LENGTH_PARA_MODE_EPP19	0x01		// mUSBIO_PACKET_LENGTHA并口工作模式为EPP方式V1.9
#define		mmUSBIO_PACKET_LENGTH_PARA_MODE_MEM	0x02		// mUSBIO_PACKET_LENGTH并口工作模式为MEM方式


// I/O方向设置位定义,直接输入的状态信号的位定义,直接输出的位数据定义
#define		mStateBitERR			0x00000100	// 只读可写,ERR#引脚输入状态,1:高电平,0:低电平
#define		mStateBitPEMP			0x00000200	// 只读可写,PEMP引脚输入状态,1:高电平,0:低电平
#define		mStateBitINT			0x00000400	// 只读可写,INT#引脚输入状态,1:高电平,0:低电平
#define		mStateBitSLCT			0x00000800	// 只读可写,SLCT引脚输入状态,1:高电平,0:低电平
#define		mStateBitWAIT			0x00002000	// 只读可写,WAIT#引脚输入状态,1:高电平,0:低电平
#define		mStateBitDATAS			0x00004000	// 只写可读,DATAS#/READ#引脚输入状态,1:高电平,0:低电平
#define		mStateBitADDRS			0x00008000	// 只写可读,ADDRS#/ADDR/ALE引脚输入状态,1:高电平,0:低电平
#define		mStateBitRESET			0x00010000	// 只写,RESET#引脚输入状态,1:高电平,0:低电平
#define		mStateBitWRITE			0x00020000	// 只写,WRITE#引脚输入状态,1:高电平,0:低电平
#define		mStateBitSDA			0x00800000	// 只读,SDA引脚输入状态,1:高电平,0:低电平


#define		MAX_DEVICE_PATH_SIZE	128			// 设备名称的最大字符数
#define		MAX_DEVICE_ID_SIZE		64			// 设备ID的最大字符数


typedef		VOID	( CALLBACK	* mPmUSBIO_PACKET_LENGTH_INT_ROUTINE ) (  // 中断服务程序
	ULONG			iStatus );  // 中断状态数据,参考下面的位说明
// 位7-位0对应mUSBIO_PACKET_LENGTH的D7-D0引脚
// 位8对应mUSBIO_PACKET_LENGTH的ERR#引脚, 位9对应mUSBIO_PACKET_LENGTH的PEMP引脚, 位10对应mUSBIO_PACKET_LENGTH的INT#引脚, 位11对应mUSBIO_PACKET_LENGTH的SLCT引脚


HANDLE	WINAPI	USBIO_OpenDevice(  // 打开mUSBIO_PACKET_LENGTH设备,返回句柄,出错则无效
	ULONG			iIndex );  // 指定mUSBIO_PACKET_LENGTH设备序号,0对应第一个设备


VOID	WINAPI	USBIO_CloseDevice(  // 关闭mUSBIO_PACKET_LENGTH设备
	ULONG			iIndex );  // 指定mUSBIO_PACKET_LENGTH设备序号


ULONG	WINAPI	USBIO_GetVersion( );  // 获得DLL版本号,返回版本号


ULONG	WINAPI	USBIO_DriverCommand(  // 直接传递命令给驱动程序,出错则返回0,否则返回数据长度
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号,V1.6以上DLL也可以是设备打开后的句柄
	mPWIN32_COMMAND	ioCommand );  // 命令结构的指针
// 该程序在调用后返回数据长度,并且仍然返回命令结构,如果是读操作,则数据返回在命令结构中,
// 返回的数据长度在操作失败时为0,操作成功时为整个命令结构的长度,例如读一个字节,则返回mWIN32_COMMAND_HEAD+1,
// 命令结构在调用前,分别提供:管道号或者命令功能代码,存取数据的长度(可选),数据(可选)
// 命令结构在调用后,分别返回:操作状态代码,后续数据的长度(可选),
//   操作状态代码是由WINDOWS定义的代码,可以参考NTSTATUS.H,
//   后续数据的长度是指读操作返回的数据长度,数据存放在随后的缓冲区中,对于写操作一般为0


ULONG	WINAPI	USBIO_GetDrvVersion( );  // 获得驱动程序版本号,返回版本号,出错则返回0


BOOL	WINAPI	USBIO_ResetDevice(  // 复位USB设备
	ULONG			iIndex );  // 指定mUSBIO_PACKET_LENGTH设备序号


BOOL	WINAPI	USBIO_GetDeviceDescr(  // 读取设备描述符
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	PVOID			oBuffer,  // 指向一个足够大的缓冲区,用于保存描述符
	PULONG			ioLength );  // 指向长度单元,输入时为准备读取的长度,返回后为实际读取的长度


BOOL	WINAPI	USBIO_GetConfigDescr(  // 读取配置描述符
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	PVOID			oBuffer,  // 指向一个足够大的缓冲区,用于保存描述符
	PULONG			ioLength );  // 指向长度单元,输入时为准备读取的长度,返回后为实际读取的长度


BOOL	WINAPI	USBIO_SetIntRoutine(  // 设定中断服务程序
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	mPmUSBIO_PACKET_LENGTH_INT_ROUTINE	iIntRoutine );  // 指定中断服务程序,为NULL则取消中断服务,否则在中断时调用该程序


BOOL	WINAPI	USBIO_ReadInter(  // 读取中断数据
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	PULONG			iStatus );  // 指向一个双字单元,用于保存读取的中断状态数据,见下行
// 位7-位0对应mUSBIO_PACKET_LENGTH的D7-D0引脚
// 位8对应mUSBIO_PACKET_LENGTH的ERR#引脚, 位9对应mUSBIO_PACKET_LENGTH的PEMP引脚, 位10对应mUSBIO_PACKET_LENGTH的INT#引脚, 位11对应mUSBIO_PACKET_LENGTH的SLCT引脚


BOOL	WINAPI	USBIO_AbortInter(  // 放弃中断数据读操作
	ULONG			iIndex );  // 指定mUSBIO_PACKET_LENGTH设备序号


BOOL	WINAPI	USBIO_SetParaMode(  // 设置并口模式
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	ULONG			iMode );  // 指定并口模式: 0为EPP模式/EPP模式V1.7, 1为EPP模式V1.9, 2为MEM模式


BOOL	WINAPI	USBIO_InitParallel(  // 复位并初始化并口,RST#输出低电平脉冲
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	ULONG			iMode );  // 指定并口模式: 0为EPP模式/EPP模式V1.7, 1为EPP模式V1.9, 2为MEM模式, >= 0x00000100 保持当前模式


BOOL	WINAPI	USBIO_ReadData0(  // 从0#端口读取数据块
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	PVOID			oBuffer,  // 指向一个足够大的缓冲区,用于保存读取的数据
	PULONG			ioLength );  // 指向长度单元,输入时为准备读取的长度,返回后为实际读取的长度


BOOL	WINAPI	USBIO_ReadData1(  // 从1#端口读取数据块
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	PVOID			oBuffer,  // 指向一个足够大的缓冲区,用于保存读取的数据
	PULONG			ioLength );  // 指向长度单元,输入时为准备读取的长度,返回后为实际读取的长度


BOOL	WINAPI	USBIO_AbortRead(  // 放弃数据块读操作
	ULONG			iIndex );  // 指定mUSBIO_PACKET_LENGTH设备序号


BOOL	WINAPI	USBIO_WriteData0(  // 向0#端口写出数据块
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	PVOID			iBuffer,  // 指向一个缓冲区,放置准备写出的数据
	PULONG			ioLength );  // 指向长度单元,输入时为准备写出的长度,返回后为实际写出的长度


BOOL	WINAPI	USBIO_WriteData1(  // 向1#端口写出数据块
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	PVOID			iBuffer,  // 指向一个缓冲区,放置准备写出的数据
	PULONG			ioLength );  // 指向长度单元,输入时为准备写出的长度,返回后为实际写出的长度


BOOL	WINAPI	USBIO_AbortWrite(  // 放弃数据块写操作
	ULONG			iIndex );  // 指定mUSBIO_PACKET_LENGTH设备序号


BOOL	WINAPI	USBIO_GetStatus(  // 通过mUSBIO_PACKET_LENGTH直接输入数据和状态
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	PULONG			iStatus );  // 指向一个双字单元,用于保存状态数据,参考下面的位说明
// 位7-位0对应mUSBIO_PACKET_LENGTH的D7-D0引脚
// 位8对应mUSBIO_PACKET_LENGTH的ERR#引脚, 位9对应mUSBIO_PACKET_LENGTH的PEMP引脚, 位10对应mUSBIO_PACKET_LENGTH的INT#引脚, 位11对应mUSBIO_PACKET_LENGTH的SLCT引脚, 位23对应mUSBIO_PACKET_LENGTH的SDA引脚
// 位13对应mUSBIO_PACKET_LENGTH的BUSY/WAIT#引脚, 位14对应mUSBIO_PACKET_LENGTH的AUTOFD#/DATAS#引脚,位15对应mUSBIO_PACKET_LENGTH的SLCTIN#/ADDRS#引脚


BOOL	WINAPI	USBIO_ReadI2C(  // 从I2C接口读取一个字节数据
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	UCHAR			iDevice,  // 低7位指定I2C设备地址
	UCHAR			iAddr,  // 指定数据单元的地址
	PUCHAR			oByte );  // 指向一个字节单元,用于保存读取的字节数据


BOOL	WINAPI	USBIO_WriteI2C(  // 向I2C接口写入一个字节数据
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	UCHAR			iDevice,  // 低7位指定I2C设备地址
	UCHAR			iAddr,  // 指定数据单元的地址
	UCHAR			iByte );  // 待写入的字节数据


BOOL	WINAPI	USBIO_SetExclusive(  // 设置独占使用当前mUSBIO_PACKET_LENGTH设备
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	ULONG			iExclusive );  // 为0则设备可以共享使用,非0则独占使用


BOOL	WINAPI	USBIO_SetTimeout(  // 设置USB数据读写的超时
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	ULONG			iWriteTimeout,  // 指定USB写出数据块的超时时间,以毫秒mS为单位,0xFFFFFFFF指定不超时(默认值)
	ULONG			iReadTimeout );  // 指定USB读取数据块的超时时间,以毫秒mS为单位,0xFFFFFFFF指定不超时(默认值)


BOOL	WINAPI	USBIO_ReadData(  // 读取数据块
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	PVOID			oBuffer,  // 指向一个足够大的缓冲区,用于保存读取的数据
	PULONG			ioLength );  // 指向长度单元,输入时为准备读取的长度,返回后为实际读取的长度


BOOL	WINAPI	USBIO_WriteData(  // 写出数据块
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	PVOID			iBuffer,  // 指向一个缓冲区,放置准备写出的数据
	PULONG			ioLength );  // 指向长度单元,输入时为准备写出的长度,返回后为实际写出的长度


PVOID	WINAPI	USBIO_GetDeviceName(  // 返回指向mUSBIO_PACKET_LENGTH设备名称的缓冲区,出错则返回NULL
	ULONG			iIndex );  // 指定mUSBIO_PACKET_LENGTH设备序号,0对应第一个设备


ULONG	WINAPI	USBIO_GetVerIC(  // 获取mUSBIO_PACKET_LENGTH芯片的版本,返回:0=设备无效,0x10=mUSBIO_PACKET_LENGTH,0x20=mUSBIO_PACKET_LENGTHA
	ULONG			iIndex );  // 指定mUSBIO_PACKET_LENGTH设备序号


BOOL	WINAPI	USBIO_FlushBuffer(  // 清空mUSBIO_PACKET_LENGTH的缓冲区
	ULONG			iIndex );  // 指定mUSBIO_PACKET_LENGTH设备序号


BOOL	WINAPI	USBIO_WriteRead(  // 执行数据流命令,先输出再输入
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	ULONG			iWriteLength,  // 写长度,准备写出的长度
	PVOID			iWriteBuffer,  // 指向一个缓冲区,放置准备写出的数据
	ULONG			iReadStep,  // 准备读取的单个块的长度, 准备读取的总长度为(iReadStep*iReadTimes)
	ULONG			iReadTimes,  // 准备读取的次数
	PULONG			oReadLength,  // 指向长度单元,返回后为实际读取的长度
	PVOID			oReadBuffer );  // 指向一个足够大的缓冲区,用于保存读取的数据


BOOL	WINAPI	USBIO_SetStream(  // 设置串口流模式
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	ULONG			iMode );  // 指定模式,见下行
// 位1-位0: I2C接口速度/SCL频率, 00=低速/20KHz,01=标准/100KHz(默认值),10=快速/400KHz,11=高速/750KHz
// 位2:     SPI的I/O数/IO引脚, 0=单入单出(D3时钟/D5出/D7入)(默认值),1=双入双出(D3时钟/D5出D4出/D7入D6入)
// 位7:     SPI字节中的位顺序, 0=低位在前, 1=高位在前
// 其它保留,必须为0


BOOL	WINAPI	USBIO_SetDelaymS(  // 设置硬件异步延时,调用后很快返回,而在下一个流操作之前延时指定毫秒数
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	ULONG			iDelay );  // 指定延时的毫秒数


BOOL	WINAPI	USBIO_StreamI2C(  // 处理I2C数据流,2线接口,时钟线为SCL引脚,数据线为SDA引脚(准双向I/O),速度约56K字节
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	ULONG			iWriteLength,  // 准备写出的数据字节数
	PVOID			iWriteBuffer,  // 指向一个缓冲区,放置准备写出的数据,首字节通常是I2C设备地址及读写方向位
	ULONG			iReadLength,  // 准备读取的数据字节数
	PVOID			oReadBuffer );  // 指向一个缓冲区,返回后是读入的数据


typedef	enum	_EEPROM_TYPE {					// EEPROM型号
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
} EEPROM_TYPE;


BOOL	WINAPI	USBIO_ReadEEPROM(  // 从EEPROM中读取数据块,速度约56K字节
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	EEPROM_TYPE		iEepromID,  // 指定EEPROM型号
	ULONG			iAddr,  // 指定数据单元的地址
	ULONG			iLength,  // 准备读取的数据字节数
	PUCHAR			oBuffer );  // 指向一个缓冲区,返回后是读入的数据


BOOL	WINAPI	USBIO_WriteEEPROM(  // 向EEPROM中写入数据块
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	EEPROM_TYPE		iEepromID,  // 指定EEPROM型号
	ULONG			iAddr,  // 指定数据单元的地址
	ULONG			iLength,  // 准备写出的数据字节数
	PUCHAR			iBuffer );  // 指向一个缓冲区,放置准备写出的数据


BOOL	WINAPI	USBIO_GetInput(  // 通过mUSBIO_PACKET_LENGTH直接输入数据和状态,效率比mUSBIO_PACKET_LENGTHGetStatus更高
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	PULONG			iStatus );  // 指向一个双字单元,用于保存状态数据,参考下面的位说明
// 位7-位0对应mUSBIO_PACKET_LENGTH的D7-D0引脚
// 位8对应mUSBIO_PACKET_LENGTH的ERR#引脚, 位9对应mUSBIO_PACKET_LENGTH的PEMP引脚, 位10对应mUSBIO_PACKET_LENGTH的INT#引脚, 位11对应mUSBIO_PACKET_LENGTH的SLCT引脚, 位23对应mUSBIO_PACKET_LENGTH的SDA引脚
// 位13对应mUSBIO_PACKET_LENGTH的BUSY/WAIT#引脚, 位14对应mUSBIO_PACKET_LENGTH的AUTOFD#/DATAS#引脚,位15对应mUSBIO_PACKET_LENGTH的SLCTIN#/ADDRS#引脚


BOOL	WINAPI	USBIO_SetOutput(  // 设置mUSBIO_PACKET_LENGTH的I/O方向,并通过mUSBIO_PACKET_LENGTH直接输出数据
/* ***** 谨慎使用该API, 防止修改I/O方向使输入引脚变为输出引脚导致与其它输出引脚之间短路而损坏芯片 ***** */
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	ULONG			iEnable,  // 数据有效标志,参考下面的位说明
// 位0为1说明iSetDataOut的位15-位8有效,否则忽略
// 位1为1说明iSetDirOut的位15-位8有效,否则忽略
// 位2为1说明iSetDataOut的7-位0有效,否则忽略
// 位3为1说明iSetDirOut的位7-位0有效,否则忽略
// 位4为1说明iSetDataOut的位23-位16有效,否则忽略
	ULONG			iSetDirOut,  // 设置I/O方向,某位清0则对应引脚为输入,某位置1则对应引脚为输出,并口方式下默认值为0x000FC000,参考下面的位说明
	ULONG			iSetDataOut );  // 输出数据,如果I/O方向为输出,那么某位清0时对应引脚输出低电平,某位置1时对应引脚输出高电平,参考下面的位说明
// 位7-位0对应mUSBIO_PACKET_LENGTH的D7-D0引脚
// 位8对应mUSBIO_PACKET_LENGTH的ERR#引脚, 位9对应mUSBIO_PACKET_LENGTH的PEMP引脚, 位10对应mUSBIO_PACKET_LENGTH的INT#引脚, 位11对应mUSBIO_PACKET_LENGTH的SLCT引脚
// 位13对应mUSBIO_PACKET_LENGTH的WAIT#引脚, 位14对应mUSBIO_PACKET_LENGTH的DATAS#/READ#引脚,位15对应mUSBIO_PACKET_LENGTH的ADDRS#/ADDR/ALE引脚
// 以下引脚只能输出,不考虑I/O方向: 位16对应mUSBIO_PACKET_LENGTH的RESET#引脚, 位17对应mUSBIO_PACKET_LENGTH的WRITE#引脚, 位18对应mUSBIO_PACKET_LENGTH的SCL引脚, 位29对应mUSBIO_PACKET_LENGTH的SDA引脚


BOOL	WINAPI	USBIO_Set_D5_D0(  // 设置mUSBIO_PACKET_LENGTH的D5-D0引脚的I/O方向,并通过mUSBIO_PACKET_LENGTH的D5-D0引脚直接输出数据,效率比mUSBIO_PACKET_LENGTHSetOutput更高
/* ***** 谨慎使用该API, 防止修改I/O方向使输入引脚变为输出引脚导致与其它输出引脚之间短路而损坏芯片 ***** */
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号
	ULONG			iSetDirOut,  // 设置D5-D0各引脚的I/O方向,某位清0则对应引脚为输入,某位置1则对应引脚为输出,并口方式下默认值为0x00全部输入
	ULONG			iSetDataOut );  // 设置D5-D0各引脚的输出数据,如果I/O方向为输出,那么某位清0时对应引脚输出低电平,某位置1时对应引脚输出高电平
// 以上数据的位5-位0分别对应mUSBIO_PACKET_LENGTH的D5-D0引脚


BOOL	WINAPI	USBIO_SetBufUpload(  // 设定内部缓冲上传模式
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号,0对应第一个设备
	ULONG			iEnableOrClear );  // 为0则禁止内部缓冲上传模式,使用直接上传,非0则启用内部缓冲上传模式并清除缓冲区中的已有数据
// 如果启用内部缓冲上传模式,那么mUSBIO_PACKET_LENGTH驱动程序创建线程自动接收USB上传数据到内部缓冲区,同时清除缓冲区中的已有数据,当应用程序调用mUSBIO_PACKET_LENGTHReadData后将立即返回缓冲区中的已有数据


LONG	WINAPI	USBIO_QueryBufUpload(  // 查询内部上传缓冲区中的已有数据包个数,成功返回数据包个数,出错返回-1
	ULONG			iIndex );  // 指定mUSBIO_PACKET_LENGTH设备序号,0对应第一个设备


BOOL	WINAPI	USBIO_SetBufDownload(  // 设定内部缓冲下传模式
	ULONG			iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号,0对应第一个设备
	ULONG			iEnableOrClear );  // 为0则禁止内部缓冲下传模式,使用直接下传,非0则启用内部缓冲下传模式并清除缓冲区中的已有数据
// 如果启用内部缓冲下传模式,那么当应用程序调用mUSBIO_PACKET_LENGTHWriteData后将仅仅是将USB下传数据放到内部缓冲区并立即返回,而由mUSBIO_PACKET_LENGTH驱动程序创建的线程自动发送直到完毕


LONG	WINAPI	USBIO_QueryBufDownload(  // 查询内部下传缓冲区中的剩余数据包个数(尚未发送),成功返回数据包个数,出错返回-1
	ULONG			iIndex );  // 指定mUSBIO_PACKET_LENGTH设备序号,0对应第一个设备


BOOL	WINAPI	USBIO_ResetInter(  // 复位中断数据读操作
	ULONG			iIndex );  // 指定mUSBIO_PACKET_LENGTH设备序号


BOOL	WINAPI	USBIO_ResetRead(  // 复位数据块读操作
	ULONG			iIndex );  // 指定mUSBIO_PACKET_LENGTH设备序号


BOOL	WINAPI	USBIO_ResetWrite(  // 复位数据块写操作
	ULONG			iIndex );  // 指定mUSBIO_PACKET_LENGTH设备序号


typedef		VOID	( CALLBACK	* mPmUSBIO_PACKET_LENGTH_NOTIFY_ROUTINE ) (  // 设备事件通知回调程序
	ULONG			iEventStatus );  // 设备事件和当前状态(在下行定义): 0=设备拔出事件, 3=设备插入事件
/*
#define		mUSBIO_PACKET_LENGTH_DEVICE_ARRIVAL		3		// 设备插入事件,已经插入
#define		mUSBIO_PACKET_LENGTH_DEVICE_REMOVE_PEND	1		// 设备将要拔出
#define		mUSBIO_PACKET_LENGTH_DEVICE_REMOVE			0		// 设备拔出事件,已经拔出
*/
#define		USBIO_DEVICE_ARRIVAL		3		// 设备插入事件,已经插入
#define		USBIO_DEVICE_REMOVE_PEND	1		// 设备将要拔出
#define		USBIO_DEVICE_REMOVE			0		// 设备拔出事件,已经拔出


BOOL	WINAPI	USBIO_SetDeviceNotify(  // 设定设备事件通知程序
	ULONG					iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号,0对应第一个设备
	PCHAR					iDeviceID,  // 可选参数,指向字符串,指定被监控的设备的ID,字符串以\0终止
	mPmUSBIO_PACKET_LENGTH_NOTIFY_ROUTINE	iNotifyRoutine );  // 指定设备事件回调程序,为NULL则取消事件通知,否则在检测到事件时调用该程序


BOOL	WINAPI	USBIO_SetupSerial(  // 设定mUSBIO_PACKET_LENGTH的串口特性,该API只能用于工作于串口方式的mUSBIO_PACKET_LENGTH芯片
	ULONG					iIndex,  // 指定mUSBIO_PACKET_LENGTH设备序号,0对应第一个设备
	ULONG					iParityMode,  // 指定mUSBIO_PACKET_LENGTH串口的数据校验模式: NOPARITY/ODDPARITY/EVENPARITY/MARKPARITY/SPACEPARITY
	ULONG					iBaudRate );  // 指定mUSBIO_PACKET_LENGTH串口的通讯波特率值,可以是50至3000000之间的任意值



#ifdef __cplusplus
}
#endif

#endif		// _mUSBIO_PACKET_LENGTH_DLL_H
