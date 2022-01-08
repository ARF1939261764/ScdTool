#include "stdio.h"
#include "stdint.h"
#include "scdtool.h"
#include "string.h"
/*eeprom_write*/
void eeprom_write(uint16_t addr,uint8_t *data,uint32_t length)
{
    uint32_t i;
    uint8_t buff[2048];
    buff[0] = (addr >> 8) & 0xFF;
    buff[1] = (addr >> 0) & 0xFF;
    memcpy(&buff[2],data,length);
    IIC_Write(0x50,buff,length + 2);
}
/*eeprom_read*/
void eeprom_read(uint16_t addr,uint8_t *data,uint32_t length)
{
    uint8_t buff[2];
    buff[0] = (addr >> 8) & 0xFF;
    buff[1] = (addr >> 0) & 0xFF;
    IIC_WriteWithRead(0x50,buff,2,data,length);
}
/*main*/
int main(void)
{
    int i;
    uint8_t txbuff[4] = {0xA0,0xA1,0xA2,0xA3};
    uint8_t rxbuff[4];
    IIC_Open("\\\\?\\usb#vid_1a86&pid_5512#8&240e69e3&0&4#{5446f048-98b4-4ef0-96e8-27994bac0d00}");
    eeprom_write(0x00,txbuff,4);
    Sleep(100);
    eeprom_read(0x00,rxbuff,4);
    for(i=0;i<4;i++)
    {
        printf("rxbuff[%d] = %02x\r\n",i,rxbuff[i]);
    }
    return 0;
}
