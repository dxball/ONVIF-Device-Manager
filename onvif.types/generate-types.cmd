SET files= schemas\onvif.xsd schemas\include.xsd schemas\ws-addr.xsd schemas\bf-2.xsd schemas\b-2.xsd schemas\t-1.xsd schemas\xml.xsd schemas\xmlmime.xsd 
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\xsd.exe" /l:cs /c /u:http://www.onvif.org/ver10/schema %files% 
pause