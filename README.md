# Some Weather App
Yet another weather app

## Features
This app will do the following, each feature updating whatever was previously there:

### 1. Farenheit temperature for a US zipcode
Copilot Prompt:
```
Log the current temperature for a given zipcode using Serilog. Results will be in imperieal units. Zip code will be supplied as an argument in the CLI using System.CommandLine.
```
Note that packages need to be added - ask Copilot.

Note the code needs to be fixed - Copilot doesn't know it. We need to use 
```
using System.CommandLine.NamingConventionBinder;
```
instead of `System.CommandLine.Invocation`.

Add the package:
```
dotnet add App/ package System.CommandLine.DragonFruit --prerelease
```
fix Serilog code: add `using Serilog.Formatting.Compactor`.

Run `dotnet add App/ package Serilog.Formatting.Compact` from the CLI.

### 2. Farenheit temperature for a US city and state combo.
Copilot Prompt:
```
Display the current temperature for a given city and state. Results will be in imperieal units. City and state will be supplied as an argument in the CLI.
```

### 3. Make sure that either the zipcode or the city and state are provided, but not both.
Copilot Prompt:
```
Add a validator to make sure that either the zipcode or the city and state are provided, but not both.
```
