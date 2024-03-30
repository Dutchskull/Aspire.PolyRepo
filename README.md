# Aspire.Git

## Installation

Get the package from nuget.

## How to use

Add this configuration to you apphost.

```csharp
builder.AddGitProject(
    "URL TO YOUR GIT REPO",
    "WHERE YOU WANT TO CLONE THE REPO",
    "PATH RELATIVE TO THE CLONED REPO TO THE CSPROJ")
        .WithReference(cache)
        .WithReference(apiService);
```

Go into a terminal and navigate to your apphost project and run this command.

```powershell
$env:DOTNET_DASHBOARD_UNSECURED_ALLOW_ANONYMOUS = 'true'; dotnet run
```

## Development

Go into a terminal and navigate to this projects apphost project and run this command.

```powershell
$env:DOTNET_DASHBOARD_UNSECURED_ALLOW_ANONYMOUS = 'true'; dotnet run .\Aspire.Git.AppHost.csproj
```