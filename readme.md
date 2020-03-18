# .Net Core 3.0 Multilayer Api demo

## Description

It just have two REST actions:

1. Import from a football-data.org a full league from . Football-data.org has a limitation 10 calls/min in the free tier, so it's a bit slow.
2. Get the total players in the leagues imported on the DB.

## Instructions

1. Change the connectionstring on the appsettings.json and/or appsettings.Development.json
2. Compile and run

The DB migrations is done automatically

## Features

1. Swagger
2. Entity Framework
3. Unit testing with InMemory EntityFramework DB (xUnit)
4. HttpClient
5. Repository and Unit of Work

Complementary reading: https://medium.com/swlh/building-a-nice-multi-layer-net-core-3-api-c68a9ef16368
