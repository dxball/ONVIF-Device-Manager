
				set NET="%PROGRAMFILES%\Microsoft SDKs\Windows\v6.0A\bin"
				rem -- alternative location: set NET="%PROGRAMFILES%\Microsoft.NET\SDK\v2.0\Bin"
				%NET%\gacutil /if IKVM.Runtime.dll
				%NET%\gacutil /if IKVM.OpenJDK.ClassLibrary.dll
				%NET%\gacutil /if charsets.dll
				%NET%\gacutil /if saxon9.dll 
				%NET%\gacutil /if saxon9api.dll                
      