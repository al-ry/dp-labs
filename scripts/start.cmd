@echo off

set APP_PATH="../Valuator/"
setx NGINX_PATH "../nginx/"
set RANK_CALCULATOR_PATH="../RankCalculatorService/"
set EVENTS_LOGGER="../EventsLogger/"

pushd %APP_PATH%

start "Valuator1" dotnet run --no-build --urls "http://localhost:5001"
start "Valuator2" dotnet run --no-build --urls "http://localhost:5002"

popd

pushd %RANK_CALCULATOR_PATH%

start "RankCalculator1" dotnet run
start "RankCalculator1" dotnet run

popd

pushd %EVENTS_LOGGER%

start "EventsLogger1" dotnet run
start "EventsLogger2" dotnet run

popd

pushd %NGINX_PATH%

start nginx

popd

