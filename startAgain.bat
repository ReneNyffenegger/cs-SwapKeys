@cd %~dp0

:loop
   echo %time% >> started.at
   powershell -c . .\load-SwapKeys.ps1
goto loop
