#!/bin/sh

cd tests
cd backend
dotnet build
dotnet test
cd ..
cd ..