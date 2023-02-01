@ECHO OFF
start cmd.exe /k "cd ./server/Api & dotnet run"
start cmd.exe /k "cd client & npm i & npm start"