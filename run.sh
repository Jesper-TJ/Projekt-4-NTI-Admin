#!/bin/sh

. coconutChecker.sh

cd frontend
npm run
npm run build

cd ../backend
dotnet restore
dotnet run migrate

export 'FONTCONFIG_PATH=/opt/homebrew/etc/fonts'

dotnet run --launch-profile https
