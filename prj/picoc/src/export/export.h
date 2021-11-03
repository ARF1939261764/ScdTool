#ifndef  __EXPORT_H
#define __EXPORT_H
#include "../picoc/scdlib/iic/iic.h"
__declspec(dllexport) int picoc(int argc, char** argv);
__declspec(dllexport) int IIC_SetCSCallback(CCmdHanlder Hanlder);

#endif
