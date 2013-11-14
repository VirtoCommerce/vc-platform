@echo off
setlocal

set SERVICE_NAME=virtocommerce.elasticsearch
for %%I in ("%~dp0..") do set ES_HOME=%%~dpfI
set PRUNSRV=%ES_HOME%\bin\elasticsearchw

"%PRUNSRV%" //DS//%SERVICE_NAME%
