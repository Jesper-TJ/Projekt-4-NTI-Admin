#!/bin/sh

open -a docker && echo "Started Docker"
docker pull postgres && echo "Pulled Postgres"
docker ps --filter "name=pg-docker" --format "{{.Names}}" | grep -q "^pg-docker$" && docker stop pg-docker && echo "Shut down all running pg-docker images"
docker run --rm --name pg-docker -e POSTGRES_PASSWORD=postgres -d -p 5432:5432 -v $HOME/docker/volumes/postgres:/var/lib/postgresql/data postgres && echo "Started a new pg-docker image"

./seed.sh

trap cleanup EXIT

function cleanup {
    ps aux | grep vite | awk '{print $2}' | xargs kill -1
}

. coconutChecker.sh

cd frontend
npm install
npm run dev &

cd ../backend
dotnet restore

export 'FONTCONFIG_PATH=/opt/homebrew/etc/fonts'

dotnet run dev --launch-profile development
