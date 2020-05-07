Set WshShell = CreateObject("WScript.Shell") 
WshShell.Run chr(34) & "c:\ORFarm\BatFiles\WebService.bat" & Chr(34), 0
Set WshShell = Nothing
