#!/bin/bash
set -e
trap 'echo "❌ Error occurred at line $LINENO. Exiting."; exit 1' ERR

echo "Checking for mono..."
if command -v mono > /dev/null; then
  echo "Mono is already installed."
  exit 0
else
  echo "Mono is not installed, installing..."
  
  sudo apt-get update
  sudo apt-get install -y dotnet-sdk-8.0
  sudo apt-get install -y aspnetcore-runtime-8.0
  sudo apt-get install -y dotnet-runtime-8.0

  # install dependencies:
  sudo apt-get install -y dirmngr gnupg apt-transport-https ca-certificates software-properties-common

  # import mono GPG key:
  sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF

  # add mono repository:
  sudo apt-add-repository 'deb https://download.mono-project.com/repo/ubuntu stable-focal main'

  # install mono:
  sudo apt-get update
  sudo apt-get install -y mono-complete
fi