@echo off

taskkill /f /im Valuator.exe
taskkill /f /im RankCalculatorService.exe
taskkill /f /im EventsLogger.exe

pushd %NGINX_PATH%

nginx -s stop

popd

reg delete "HKCU\Environment" /f /v NGINX_PATH

