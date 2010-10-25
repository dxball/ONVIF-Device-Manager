rem "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\xsd.exe"  /c /l:CS /n:onvif.types /nologo

SET files= schemas\onvif.xsd schemas\include.xsd schemas\ws-addr.xsd schemas\bf-2.xsd schemas\b-2.xsd schemas\t-1.xsd schemas\xml.xsd schemas\xmlmime.xsd 
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\xsd.exe" /p:xsd-gen-config.xml /fields /order %files% 

rem "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\svcutil.exe" /r:onvif.types.dll /n:"*,onvif.types" /n:"http://www.onvif.org/ver10/media/wsdl,onvif.media" /o:media.generated.cs /noConfig /t:code /l:cs schemas\media.wsdl
rem "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\svcutil.exe" /dconly /r:onvif.types.dll /n:http://www.onvif.org/ver10/schema,qqq /n:*,rrrr /n:http://www.onvif.org/ver10/media/wsdl,onvif.media /o:media.generated.cs /t:code /l:cs schemas\media.wsdl  %files%
rem "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\svcutil.exe" /t:XmlSerializer onvif.types.dll
rem "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\svcutil.exe"  /o:ref.cs /t:code /l:cs *.wsdl *.xsd

rem "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\svcutil.exe"  /noConfig /t:code /l:cs http://www.onvif.org/onvif/ver10/device/wsdl/devicemgmt.wsdl


pause