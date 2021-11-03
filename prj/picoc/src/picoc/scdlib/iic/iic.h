
#include "stdint.h"
#define IIC_DEVICE_NAME_MAX_LEN 256

typedef enum
{
    CCmd_CreateDeviceObject = 0,
    CCmd_SetContext,
    CCmd_GetContext,
    CCmd_GetDeviceList,
    CCmd_OpenByIndex,
    CCmd_Open,
    CCmd_SetBaudrate,
    CCmd_Close,
    CCmd_ReadByte,
    CCmd_Read,
    CCmd_WriteByte,
    CCmd_Write,
    CCmd_WriteWithRead
}CCmd;

typedef int (*CCmdHanlder)(CCmd Cmd, void* Arg);
__declspec(dllexport) int IIC_SetCSCallback(CCmdHanlder Hanlder);

/*生成设备对象*/
void IIC_CreateDeviceObject(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs);
void IIC_SetContext(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs);
void IIC_GetContext(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs);
void IIC_GetDeviceList(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs);
void IIC_OpenByIndex(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs);
void IIC_Open(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs);
void IIC_SetBaudrate(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs);
void IIC_Close(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs);
void IIC_ReadByte(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs);
void IIC_Read(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs);
void IIC_WriteByte(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs);
void IIC_Write(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs);
void IIC_WriteWithRead(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs);
