@echo off

set APP_PATH="../Valuator/"
set NGINX_PATH="../nginx/"
set RANK_CALCULATOR_PATH="../RankCalculatorService/"
set EVENTS_LOGGER="../EventsLogger/"

pushd %APP_PATH%

start dotnet run --no-build --urls "http://localhost:5001"
start dotnet run --no-build --urls "http://localhost:5002"

popd

pushd %RANK_CALCULATOR_PATH%

start dotnet run
start dotnet run

popd

pushd %EVENTS_LOGGER%

start dotnet run
start dotnet run

popd

pushd %NGINX_PATH%

start nginx

popd

