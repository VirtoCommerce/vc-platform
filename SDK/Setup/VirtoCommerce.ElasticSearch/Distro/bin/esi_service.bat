@echo off
%~d0
cd %~dp0
call esi_config.bat %1
service.bat install && service.bat start
