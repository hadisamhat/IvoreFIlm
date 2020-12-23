#!/bin/sh

cd ~
dotnet run  --project Desktop/IvoreFilm/IvoreFilm/ & ./keycloak-11.0.3/bin/standalone.sh
