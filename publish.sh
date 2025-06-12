#!/bin/bash

# Configuration 
BACKEND_PROJECT_PATH="./backend"
FRONTEND_PROJECT_PATH="./frontend"
PUBLISH_OUTPUT_PATH="./publish"
DOTNET_VERSION="9.0"
SEED=false
PORT=5000
RELEASE=false

# Check for required .NET SDK version
echo "Checking .NET version..."
INSTALLED_DOTNET_VERSION=$(dotnet --version)
if [[ ! "$INSTALLED_DOTNET_VERSION" =~ ^$DOTNET_VERSION ]]; then
  echo "Error: .NET version $DOTNET_VERSION is not installed. Installed version: $INSTALLED_DOTNET_VERSION"
  exit 1
fi

# Check arguments
while [[ $# -gt 0 ]]; do
  case $1 in
    --release)
      RELEASE=true
      shift
      ;;
    --debug)
      RELEASE=false
      shift
      ;;
    --seed)
      SEED=true
      shift
      ;;
    --port)
      PORT=$2
      shift
      shift
      ;;
    *)
      echo "Unknown option: $1"
      exit 1
      ;;
  esac
done

# Display large green text for the mode using repeated characters
if $RELEASE; then
  echo -e "\033[32m#########################################"
  echo -e "\033[32m#                                       #"
  echo -e "\033[32m#           RELEASE MODE                #"
  echo -e "\033[32m#                                       #"
  echo -e "\033[32m#########################################"
else
  echo -e "\033[32m#########################################"
  echo -e "\033[32m#                                       #"
  echo -e "\033[32m#           DEBUG MODE                  #"
  echo -e "\033[32m#                                       #"
  echo -e "\033[32m#########################################"
fi

# Build the frontend (Vuetify)
echo "Building frontend..."
cd $FRONTEND_PROJECT_PATH
npm install
npm run build
if [[ $? -ne 0 ]]; then
  echo "Frontend build failed!"
  exit 1
fi
echo "Frontend built successfully."

# Copy built frontend to backend's wwwroot
echo "Copying frontend build to backend's wwwroot..."
mkdir -p $BACKEND_PROJECT_PATH/wwwroot
cp -r build/* ../$BACKEND_PROJECT_PATH/wwwroot/

# Restore backend dependencies (just to be safe)
echo "Restoring backend dependencies..."
cd ../$BACKEND_PROJECT_PATH
dotnet restore
if [[ $? -ne 0 ]]; then
  echo "Backend restore failed!"
  exit 1
fi

# Build and publish the backend (Dotnet MVC with EF)
echo "Publishing backend..."
if $RELEASE; then
  dotnet publish --configuration Release --output $PUBLISH_OUTPUT_PATH
else
  dotnet publish --configuration Debug --output $PUBLISH_OUTPUT_PATH
fi

if [[ $? -ne 0 ]]; then
  echo "Backend publish failed!"
  exit 1
fi
echo "Backend published successfully."

# Update Program.cs to handle --seed and --port arguments
echo "Updating Program.cs for --seed and --port arguments..."
sed -i "s|var host = CreateHostBuilder(args).Build();|var host = CreateHostBuilder(args).ConfigureServices((context, services) => { services.AddSingleton(new AppSettings { SeedDatabase = $SEED, Port = $PORT }); }).Build();" $BACKEND_PROJECT_PATH/Program.cs

# Handle seed logic in Program.cs (if applicable)
if $SEED; then
  echo "Seeding database..."
  dotnet ef database update --project $BACKEND_PROJECT_PATH
fi

# Final message
echo "Publish process completed successfully."
echo "Backend executable is ready in $PUBLISH_OUTPUT_PATH"
echo "You can now deploy to your server with dotnet $DOTNET_VERSION installed."
