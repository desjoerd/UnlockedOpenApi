#!/bin/sh

# This script is run after the container is created and the workspace is mounted.
git config --global user.name 'demouser'

#sudo dotnet workload update
dotnet restore
dotnet tool restore