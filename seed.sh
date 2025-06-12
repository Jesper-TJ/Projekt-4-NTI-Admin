#!/bin/sh
cd ./backend || exit 1
echo "What type of seed do you want?"
# Use 'select' to create a menu
options=("Standard" "Complete (Only if really needed)")
PS3="Enter the number corresponding to your choice: "
select opt in "${options[@]}"; do
    if [ -n "$opt" ]; then
        selected_option=$REPLY
        echo "You selected: $opt"
        break
    else
        echo "Invalid selection. Please try again."
    fi
done
# Perform actions based on the selected option
if [ "$selected_option" = "1" ]; then
    dotnet run seed
elif [ "$selected_option" = "2" ]; then
    dotnet run completeSeed
else
    echo "Unexpected error."
fi
