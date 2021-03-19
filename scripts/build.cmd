@echo off

set APP_PATH="../Valuator/"
set RANK_CALCULATOR_PATH="../RankCalculatorService/"
set LIB_PATH="../Lib"

pushd %LIB_PATH%

start dotnet build

popd


pushd %APP_PATH%

start dotnet build

popd

pushd %RANK_CALCULATOR_PATH%

start dotnet build

popd
