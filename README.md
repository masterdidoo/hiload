# Highload Cup 2017
https://highloadcup.ru/

## Stack
ASP.NET Core 2.0

## Run
```
dotnet publish -c Release -o out
docker build -t hiload .
docker run --rm -p 80:80 hiload
```
