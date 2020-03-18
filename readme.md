# .Net Core 3.0 Multilayer Api demo

## Description

It just have two REST actions:

1. Import from a football-data.org a full league from . [Football-data.org](https://www.football-data.org/) has a limitation 10 calls/min in the free tier, so it's a bit slow.
2. Get the total players in the leagues imported on the DB.

## Instructions

1. Change the connectionstring on the appsettings.json and/or appsettings.Development.json
2. Compile and run

The DB migrations is done automatically

## Features
1. Async Calls
2. Swagger
3. Entity Framework Core
4. Unit testing with InMemory EntityFramework DB (xUnit)
5. HttpClient
6. Repository and Unit of Work
7. DI

## Api Responses by definition:
 
 1. The API responses for /import-league/{leagueCode} are:
 
    HttpCode 201, {"message": "Successfully imported"} --> When the leagueCode was successfully imported.
 
    HttpCode 409, {"message": "League already imported"} --> If the given leagueCode was already imported into the DB (and in this case, it doesn't need to be imported again).

    HttpCode 404, {"message": "Not found" } --> if the leagueCode was not found.
 
    HttpCode 504, {"message": "Server Error" } --> If there is any connectivity issue either with the football API or the DB server.
 
 2. HTTP GET in URI /total-players/{leagueCode}: 
    
    {"total" : N } and HTTP Code 200. Where N is the total amount of players belonging to all teams that participate in the given league (leagueCode).
 
    If the given leagueCode is not present into the DB, it should respond an HTTP Code 404.
 

Complementary reading: https://medium.com/swlh/building-a-nice-multi-layer-net-core-3-api-c68a9ef16368
