@echo off

set APP_PATH="../Valuator/"
set NGINX_PATH="../nginx/"

pushd %APP_PATH%

start dotnet run --urls "http://localhost:5001"
start dotnet run --urls "http://localhost:5002"

popd

pushd %NGINX_PATH%

start nginx

popd

