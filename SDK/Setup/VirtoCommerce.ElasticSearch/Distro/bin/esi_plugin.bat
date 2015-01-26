@echo off
%~d0
cd %~dp0
call esi_config.bat
call plugin.bat %*
rem echo Errorlevel %errorlevel%
if %errorlevel% == 0 goto success

echo ------------------------------------------------------------------------------------------------------------------------------------------------- 1>&2
echo ### ERROR ### Cannot download plugin %2, maybe you are behind a proxy or firewall or plugin name/url is misspelled or plugin is already installed 1>&2
echo ------------------------------------------------------------------------------------------------------------------------------------------------- 1>&2
goto :eof

:success
echo Plugin installed succesfully
