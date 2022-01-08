#include "../interpreter.h"
#include "stdint.h"
#include "../scdlib/iic/iic.h"
#include "windows.h"

const char *StdIntDefs = "\
\n\
typedef signed char        int8_t;\n\
typedef short              int16_t;\n\
typedef int                int32_t;\n\
typedef unsigned char      uint8_t;\n\
typedef unsigned short     uint16_t;\n\
typedef unsigned int       uint32_t;\n\
\n\
typedef signed char        int_least8_t;\n\
typedef short              int_least16_t;\n\
typedef int                int_least32_t;\n\
typedef unsigned char      uint_least8_t;\n\
typedef unsigned short     uint_least16_t;\n\
typedef unsigned int       uint_least32_t;\n\
\n\
typedef signed char        int_fast8_t;\n\
typedef int                int_fast16_t;\n\
typedef int                int_fast32_t;\n\
typedef unsigned char      uint_fast8_t;\n\
typedef unsigned int       uint_fast16_t;\n\
typedef unsigned int       uint_fast32_t;\n\
\n\
#define INT8_MIN         (-127i8 - 1)\n\
#define INT16_MIN        (-32767i16 - 1)\n\
#define INT32_MIN        (-2147483647i32 - 1)\n\
#define INT8_MAX         127i8\n\
#define INT16_MAX        32767i16\n\
#define INT32_MAX        2147483647i32\n\
#define UINT8_MAX        0xffui8\n\
#define UINT16_MAX       0xffffui16\n\
#define UINT32_MAX       0xffffffffui32\n\
\n\
#define INT_LEAST8_MIN   INT8_MIN\n\
#define INT_LEAST16_MIN  INT16_MIN\n\
#define INT_LEAST32_MIN  INT32_MIN\n\
#define INT_LEAST8_MAX   INT8_MAX\n\
#define INT_LEAST16_MAX  INT16_MAX\n\
#define INT_LEAST32_MAX  INT32_MAX\n\
#define UINT_LEAST8_MAX  UINT8_MAX\n\
#define UINT_LEAST16_MAX UINT16_MAX\n\
#define UINT_LEAST32_MAX UINT32_MAX\n\
\n\
#define INT_FAST8_MIN    INT8_MIN\n\
#define INT_FAST16_MIN   INT32_MIN\n\
#define INT_FAST32_MIN   INT32_MIN\n\
#define INT_FAST8_MAX    INT8_MAX\n\
#define INT_FAST16_MAX   INT32_MAX\n\
#define INT_FAST32_MAX   INT32_MAX\n\
#define UINT_FAST8_MAX   UINT8_MAX\n\
#define UINT_FAST16_MAX  UINT32_MAX\n\
#define UINT_FAST32_MAX  UINT32_MAX\n\
\n\
#define INTPTR_MIN       INT32_MIN\n\
#define INTPTR_MAX       INT32_MAX\n\
#define UINTPTR_MAX      UINT32_MAX\n\
\n\
#define INTMAX_MIN       INT64_MIN\n\
#define INTMAX_MAX       INT64_MAX\n\
#define UINTMAX_MAX      UINT64_MAX\n\
\n\
#define PTRDIFF_MIN      INTPTR_MIN\n\
#define PTRDIFF_MAX      INTPTR_MAX\n\
\n\
#ifndef SIZE_MAX\n\
    #define SIZE_MAX     UINTPTR_MAX\n\
#endif\n\
\n\
#define SIG_ATOMIC_MIN   INT32_MIN\n\
#define SIG_ATOMIC_MAX   INT32_MAX\n\
\n\
#define WCHAR_MIN        0x0000\n\
#define WCHAR_MAX        0xffff\n\
\n\
#define WINT_MIN         0x0000\n\
#define WINT_MAX         0xffff\n\
\n\
";
const char* ScdToolDef = "#define IIC_DEVICE_NAME_MAX_LEN (512)";

void Scd_Sleep(struct ParseState* Parser, struct Value* ReturnValue, struct Value** Param, int NumArgs)
{
    Sleep(Param[0]->Val->Integer);
}

/* MsvcSetupFunc */
void MsvcSetupFunc(Picoc *pc)
{
    /*none*/
}

/* list of all library functions and their prototypes */
struct LibraryFunction MsvcFunctions[] =
{
    {IIC_CreateDeviceObject, "int IIC_CreateDeviceObject(void);"                                                                           },
    {IIC_SetContext,         "int IIC_SetContext(int DeviceObject);"                                                                       },
    {IIC_GetContext,         "int IIC_GetContext(void);"                                                                                   },
    {IIC_GetDeviceList,      "int IIC_GetDeviceList(void *DeviceList ,uint32_t MaxNumber);"                                                                                    },
    {IIC_OpenByIndex,        "int IIC_OpenByIndex(int Index);"                                                                             },
    {IIC_Open,               "int IIC_Open(char* Name);"                                                                             },
    {IIC_SetBaudrate,        "int IIC_SetBaudrate(double Baudrate);"                                                                       },
    {IIC_Close,              "int IIC_Close(void);"                                                                                        },
    {IIC_ReadByte,           "int IIC_ReadByte(uint8_t SlvAddr, uint8_t* Data);"                                                           },
    {IIC_Read,               "int IIC_Read(uint8_t SlvAddr, uint8_t* Buff, int Length);"                                                   },
    {IIC_WriteByte,          "int IIC_WriteByte(uint8_t SlvAddr, uint8_t Data);"                                                               },
    {IIC_Write,              "int IIC_Write(uint8_t SlvAddr, uint8_t* Buff, int Length);"                                            },
    {IIC_WriteWithRead,      "int IIC_WriteWithRead(uint8_t SlvAddr, uint8_t* TxBuff, int TxLength, uint8_t* RxBuff, int RxLength);" },
    {Scd_Sleep,"void Sleep(int);"},
    { NULL,                  NULL                                                                                                          }
};

void PlatformLibraryInit(Picoc *pc)
{
    IncludeRegister(pc, "scdtool.h", &MsvcSetupFunc, MsvcFunctions, ScdToolDef);
    IncludeRegister(pc, "stdint.h", NULL, NULL, StdIntDefs);
}

