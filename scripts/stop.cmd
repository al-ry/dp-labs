@echo off

set NGINX_PATH="../nginx/"

taskkill /f /im Valuator.exe

pushd %NGINX_PATH%

nginx -s stop

popd

