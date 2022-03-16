@echo off

cls


:TMfaceop

	echo Kopiere Svox dll
	mkdir C:\projekte\TMtextng_dir\TMtextng\TMtextng\bin\Debug
	cd C:\projekte\TMpicto\Debug_Environment
	copy svoxdll.dll C:\projekte\TMtextng_dir\TMtextng\TMtextng\bin\Debug
	copy TMspeak_DLL.dll C:\projekte\TMtextng_dir\TMtextng\TMtextng\bin\Debug
	echo Kopiert svoxdll.dll
	echo Kopiert TMspeak_DLL.dll

pause
exit