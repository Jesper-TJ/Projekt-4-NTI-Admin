#!/bin/sh

cleanup() {
    echo "Cleaning up background processes..."
    if [ -n "$npm_pid" ]; then
        kill "$npm_pid" 2>/dev/null || true
    fi
    if [ -n "$dotnet_pid" ]; then
        kill "$dotnet_pid" 2>/dev/null || true
    fi
    sleep 2

    pkill -f 'npm run dev' 2>/dev/null || true
    pkill -f 'dotnet run' 2>/dev/null || true

    lsof -i :3002 | awk 'NR>1 {print $2}' | xargs -r kill -9 2>/dev/null || true
}

trap cleanup EXIT

cd frontend
npm install
npm run dev &
npm_pid=$!
echo "Started npm dev server with PID: $npm_pid"
cd ..

cd backend
dotnet restore
if [ $# -eq 0 ]; then
    dotnet run seed
fi
dotnet run dev --launch-profile development &
dotnet_pid=$!
echo "Started dotnet backend server with PID: $dotnet_pid"
cd ..

sleep 5

cd tests
cd frontend
npm install cypress --save-dev
npx cypress run
cd ..
cd ..


cleanup

echo "Script completed successfully. Exiting..."
exit 0