#include "iic.h"
#include "../../interpreter.h"

static CCmdHanlder ContextHandler;

int IIC_SetCSCallback(CCmdHanlder Handler)
{
	ContextHandler = Handler;
	return 0x00;
}

void IIC_CreateDeviceObject(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs)
{
	int DeviceObject;
	ContextHandler(CCmd_CreateDeviceObject,&DeviceObject);
	ReturnValue->Val->Integer = DeviceObject;
}
void IIC_SetContext(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs)
{
	int DeviceObject[2];/*0:return,1:arg*/
	DeviceObject[0] = 0;
	DeviceObject[1] = Param[0]->Val->Integer;
	ContextHandler(CCmd_SetContext, DeviceObject);
	ReturnValue->Val->Integer = DeviceObject[0];
}
void IIC_GetContext(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs)
{
	int DeviceObject;
	ContextHandler(CCmd_GetContext, &DeviceObject);
	ReturnValue->Val->Integer = DeviceObject;
}
void IIC_GetDeviceList(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs)
{
	int DeviceObject;
	ContextHandler(CCmd_GetDeviceList, &DeviceObject);
	ReturnValue->Val->Integer = DeviceObject;
}

void IIC_OpenByIndex(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs)
{
	int argv[2];
	argv[0] = 0;
	argv[1] = (int)&Param[0]->Val->Integer;
	ContextHandler(CCmd_OpenByIndex, &argv);
	ReturnValue->Val->Integer = argv[0];
}

void IIC_Open(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs)
{
	int argv[2];
	argv[0] = 0;
	argv[1] = (int)&Param[0]->Val->Character;
	ContextHandler(CCmd_Open, &argv);
	ReturnValue->Val->Integer = argv[0];
}

void IIC_SetBaudrate(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs)
{
	int argv[2];
	argv[0] = 0;
	argv[1] = (int)&Param[0]->Val->FP;
	ContextHandler(CCmd_SetBaudrate, &argv);
	ReturnValue->Val->Integer = argv[0];
}

void IIC_Close(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs)
{
	int argv[1];
	argv[0] = 0;
	ContextHandler(CCmd_Close, &argv);
	ReturnValue->Val->Integer = argv[0];
}

void IIC_ReadByte(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs)
{
	int argv[3];
	argv[0] = 0;
	argv[1] = (int)&Param[0]->Val->UnsignedCharacter;
	argv[2] = (int)&Param[1]->Val->UnsignedCharacter;
	ContextHandler(CCmd_ReadByte, &argv);
	ReturnValue->Val->Integer = argv[0];
}

void IIC_Read(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs)
{
	int argv[4];
	argv[0] = 0;
	argv[1] = (int)&Param[0]->Val->UnsignedCharacter;
	argv[2] = (int)&Param[1]->Val->UnsignedCharacter;
	argv[3] = (int)&Param[2]->Val->Integer;
	ContextHandler(CCmd_Read, &argv);
	ReturnValue->Val->Integer = argv[0];
}

void IIC_WriteByte(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs)
{
	int argv[3];
	argv[0] = 0;
	argv[1] = (int)&Param[0]->Val->UnsignedCharacter;
	argv[2] = (int)&Param[1]->Val->UnsignedCharacter;
	ContextHandler(CCmd_WriteByte, &argv);
	ReturnValue->Val->Integer = argv[0];
}

void IIC_Write(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs)
{
	int argv[4];
	argv[0] = 0;
	argv[1] = (int)&Param[0]->Val->UnsignedCharacter;
	argv[2] = (int)&Param[1]->Val->UnsignedCharacter;
	argv[3] = (int)&Param[2]->Val->Integer;
	ContextHandler(CCmd_Write, &argv);
	ReturnValue->Val->Integer = argv[0];
}

void IIC_WriteWithRead(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs)
{
	int argv[6];
	argv[0] = 0;
	argv[1] = (int)&Param[0]->Val->UnsignedCharacter;
	argv[2] = (int)&Param[1]->Val->UnsignedCharacter;
	argv[3] = (int)&Param[2]->Val->Integer;
	argv[4] = (int)&Param[3]->Val->UnsignedCharacter;
	argv[5] = (int)&Param[4]->Val->Integer;
	ContextHandler(CCmd_WriteWithRead, &argv);
	ReturnValue->Val->Integer = argv[0];
}
