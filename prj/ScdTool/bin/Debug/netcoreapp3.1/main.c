#include "stdio.h"
#include "stdint.h"
#include "time.h"
#include "scdtool.h"

 char a = 0;

int main(int argn,char **argv)
{
    int i = 0;
    uint8_t hello;
    int Obj[4];
    int temp;
    hello = 0;
    time_t t;

    //for(i = 0;i<4;i++)
    //{
    //    Obj[i] = IIC_CreateDeviceObject();
    //}
    //IIC_GetDeviceList();
    //IIC_SetContext(Obj[0]);
    //IIC_GetDeviceList();
    //IIC_SetContext(Obj[1]);
    //IIC_GetDeviceList();
    //IIC_SetContext(Obj[2]);
    //IIC_GetDeviceList();
    //temp = IIC_SetContext(Obj[3]);
    //IIC_GetDeviceList();
    //IIC_SetContext(temp);
    //IIC_GetDeviceList();
    //IIC_SetContext(Obj[2]);
    //IIC_GetDeviceList();
    //IIC_Open("hello world");
    //printf("Obj[2] = %08X,temp = %08X\r\n",Obj[2],temp);

    while(1)
    {
        i++;
        
        if(i%10000 == 0)
        {
            for(int j=0;j<argn;j++)
            {
                printf("%s ",argv[j]);
            }
            time(&t);
            printf("%ld\n",t);
        }
    }
    return 0;
}

