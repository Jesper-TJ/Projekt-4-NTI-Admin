#!/bin/sh

docker ps --format "{{.Names}}" | xargs -r docker stop
echo "Stopped all running Docker containers."

rm -rf $HOME/docker/volumes/postgres && echo "Removed directory $HOME/docker/volumes/postgres"

osascript -e 'quit app "Docker"'
echo "Docker Desktop application closed."

if pgrep -f "Docker"; then
    echo "Some Docker processes are still running. Attempting to kill them..."
    pkill -f "Docker" && echo "All Docker processes terminated."
else
    echo "No Docker processes found."
fi