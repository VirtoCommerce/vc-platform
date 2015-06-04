@echo off

REM echo robocopy %3\es "%1" /E
REM robocopy %3\es "%1" /E

echo xcopy /I /E /Y %3\es "%1"
xcopy /I /E /Y %3\es "%1"

rem %3\TomcatConfigManager.exe %es_home%\conf\server.xml %2


rem robocopy $(ProjectDir)es\jre7 $(OutDir)es /S
rem set rce=%errorlevel%
rem if not %rce%==1 exit %rce% else exit 0