rem "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\xsd.exe"  /c /l:CS /n:onvif.types /nologo

SET files= include.xsd ws-addr.xsd bf-2.xsd b-2.xsd t-1.xsd xml.xsd xmlmime.xsd onvif.xsd
rem "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\xsd.exe" %files% /p:xsd-gen-config.xml /nologo


rem "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\svcutil.exe"  /noConfig /t:code /l:cs media.wsdl %files%
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\svcutil.exe"  /o:ref.cs /t:code /l:cs *.wsdl *.xsd

rem "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\svcutil.exe"  /noConfig /t:code /l:cs http://www.onvif.org/onvif/ver10/device/wsdl/devicemgmt.wsdl


pause