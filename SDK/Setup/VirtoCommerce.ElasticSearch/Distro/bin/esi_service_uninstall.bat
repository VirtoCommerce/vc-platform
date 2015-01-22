@echo off
%~d0
cd %~dp0
call esi_config.bat
service.bat stop & service.bat remove
