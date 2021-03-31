@echo off

set APP_PATH="../Valuator/"
set RANK_CALCULATOR_PATH="../RankCalculatorService/"
set EVENTS_LOGGER="../EventsLogger/"


pushd %APP_PATH%

start dotnet build

popd

pushd %EVENTS_LOGGER%

start dotnet build 

popd

pushd %RANK_CALCULATOR_PATH%

start dotnet build

popd
