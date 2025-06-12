#!/bin/sh

brew install docker && echo "Installed Docker"
brew install dotnet@9 && echo "Installed Dotnet-sdk"
brew install node && echo "Installed Node"
brew install mono-libgdiplus && echo "Installed Gdiplus"

open -a docker && echo "Started Docker"
docker pull postgres && echo "Pulled Postgres"
docker ps --filter "name=pg-docker" --format "{{.Names}}" | grep -q "^pg-docker$" && docker stop pg-docker && echo "Shut down all running pg-docker images"
rm -rf $HOME/docker/volumes/postgres && echo "Removed directory $HOME/docker/volumes/postgres"
mkdir -p $HOME/docker/volumes/postgres && echo "Created new directory $HOME/docker/volumes/postgres"
docker run --rm --name pg-docker -e POSTGRES_PASSWORD=postgres -d -p 5432:5432 -v $HOME/docker/volumes/postgres:/var/lib/postgresql/data postgres && echo "Started a new pg-docker image"

if [ ! -f ./backend/.env ]; then
  touch ./backend/.env && echo "Created the ./backend/.env file"
  echo "DEV_DB_HOST=localhost
DEV_DB_PORT=5432
DEV_DB_USERNAME=postgres
DEV_DB_PASSWORD=postgres
DEV_DB_NAME=NtiAdminDev

TEST_DB_HOST=localhost
TEST_DB_PORT=5432
TEST_DB_USERNAME=postgres
TEST_DB_PASSWORD=postgres
TEST_DB_NAME=NtiAdminTest

PROD_DB_HOST=localhost
PROD_DB_PORT=5432
PROD_DB_USERNAME=postgres
PROD_DB_PASSWORD=postgres
PROD_DB_NAME=NtiAdminProd

ADMIN_DB_HOST=localhost
ADMIN_DB_PORT=5432
ADMIN_DB_USERNAME=postgres
ADMIN_DB_PASSWORD=postgres
ADMIN_DB_NAME=postgres

GOOGLE_API_SERVICE_ACCOUNT=[email adress]
GOOGLE_API_ORGUNITPATH=[origin path]" > ./backend/.env && echo "Wrote the default content of the ./backend/.env file"
else
  echo "./backend/.env file already exists, skipping creation"
fi

cd backend
dotnet dev-certs https --trust && echo "Install https cert on backend"

echo "installing dotnet"

dotnet tool install --global dotnet-ef && echo "Install dotnet-ef"

echo "seeding db"

echo -ne '\n' | dotnet run completeSeed && echo "Seed database"