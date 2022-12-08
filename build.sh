#!/bin/bash
( cd Web/ClientApp || exit ; npm cache verify )
dotnet restore
dotnet build
./coverage.sh

