@echo off
%~d0
cd %~dp0
call esi_config.bat
elasticsearch.bat %*