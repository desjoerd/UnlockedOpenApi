#!/bin/sh

# This script is run after the container is created and the workspace is mounted.
# git config --global user.name 'demouser'
# git config --global user.email 'alex.thissen@xebia.com'
# git config --global core.autocrlf input

sudo dotnet workload update
dotnet restore
dotnet tool restore

npm install