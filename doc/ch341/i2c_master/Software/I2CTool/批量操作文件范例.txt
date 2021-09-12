rem ****************************************
rem 批处理文件格式，可以批量给芯片下setting
rem Batch file for download setting to I2C chip
rem 第一列为i2c读写命令
rem 1st row is i2c tool r/w command
rem 第二列为芯片i2c写地址，7bit+R/W, 例如4a
rem 2nd row is chip i2c slave address
rem 第三列为寄存器地址，第四列为寄存器值
rem 3rd row register address(sub address) and 4th row value.
rem 涉及数字均为16进制格式
rem The Numbers are in hexadecimal format
rem *****************************************

i2crw 4a 01 08 
i2crw 4a 02 C7 
i2crw 4a 03 33 
i2crw 4a 04 00 
i2crw 4a 05 00 
i2crw 4a 06 E9 
i2crw 4a 07 0D 
i2crw 4a 08 98 
i2crw 4a 09 b1 
i2crw 4a 0A 80 
i2crw 4a 0B 47 
i2crw 4a 0C 40 
i2crw 4a 0D 00 
i2crw 4a 0E 81 
i2crw 4a 0F 2A 
i2crw 4a 10 00 
i2crw 4a 11 0C 
i2crw 4a 12 11 
i2crw 4a 13 00 
i2crw 4a 14 00 
i2crw 4a 15 00 
i2crw 4a 16 00 
i2crw 4a 17 00 

