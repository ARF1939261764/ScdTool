Copy /y msstdfmt.dll %windir%\System32\
Copy /y msstdfmt.dll %windir%\SysWOW64\
regsvr32 %windir%\SysWOW64\msstdfmt.dll
regsvr32 %windir%\System32\msstdfmt.dll