@echo off

echo robocopy %3\es "%1" /E
robocopy %3\es "%1" /E

rem %3\TomcatConfigManager.exe %es_home%\conf\server.xml %2


rem robocopy $(ProjectDir)es\jre7 $(OutDir)es /S
rem set rce=%errorlevel%
rem if not %rce%==1 exit %rce% else exit 0