# Aspire.PolyRepo

> [!WARNING]  
> This package is still being developed and can change. If you are trying this package please share your experience and
> how it could improve.

Aspire.PolyRepo is a .NET Aspire package designed to simplify the process of cloning and managing Git repositories
within your .NET Aspire applications. This package allows you to configure and use Git repositories seamlessly,
integrating them into your cloud-native development workflow.

## Features

- Clone Git repositories directly into your .NET Aspire application.
- Configure repository URL, name, target path, default branch, and project path.
- Easy integration with .NET Aspire App Host.

## Installation

To install the Aspire.PolyRepo package, use the .NET CLI. Run the following command in your terminal:

```sh
dotnet add package Dutchskull.Aspire.PolyRepo
```

## Usage

To use Aspire.PolyRepo in your .NET Aspire application, follow these steps:

Add the configuration to your App Host project.

```csharp
var repository = builder.AddRepository(
    "repository",
    "https://github.com/Dutchskull/Aspire-Git.git",
    c => c.WithDefaultBranch("feature/rename_and_new_api")
        .WithTargetPath("../../repos"));

var dotnetProject = builder
    .AddProjectFromRepository("dotnetProject", repository,
        "src/Dutchskull.Aspire.PolyRepo.Web/Dutchskull.Aspire.PolyRepo.Web.csproj")
    .WithReference(cache)
    .WithReference(apiService);

var reactProject = builder
    .AddNpmAppFromRepository("reactProject", repository, "src/Dutchskull.Aspire.PolyRepo.React")
    .WithReference(cache)
    .WithReference(apiService)
    .WithHttpEndpoint(3000);

var nodeProject = builder
    .AddNodeAppFromRepository("nodeProject", repository, "src/Dutchskull.Aspire.PolyRepo.Node")
    .WithReference(cache)
    .WithReference(apiService)
    .WithHttpEndpoint(54622);
```

Navigate to your App Host project directory in the terminal.

Run the application using the .NET CLI or Visual Studio 2022.

```sh
dotnet run
```

## Example

Here is an example configuration for adding a Git repository to your .NET Aspire application:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

var repository = builder.AddRepository(
    "repository",
    "https://github.com/Dutchskull/Aspire-Git.git",
    c => c.WithDefaultBranch("feature/rename_and_new_api")
        .WithTargetPath("../../repos"));

var dotnetProject = builder
    .AddProjectFromRepository("dotnetProject", repository,
        "src/Dutchskull.Aspire.PolyRepo.Web/Dutchskull.Aspire.PolyRepo.Web.csproj")
    .WithReference(cache)
    .WithReference(apiService);

var reactProject = builder
    .AddNpmAppFromRepository("reactProject", repository, "src/Dutchskull.Aspire.PolyRepo.React")
    .WithReference(cache)
    .WithReference(apiService)
    .WithHttpEndpoint(3000);

var nodeProject = builder
    .AddNodeAppFromRepository("nodeProject", repository, "src/Dutchskull.Aspire.PolyRepo.Node")
    .WithReference(cache)
    .WithReference(apiService)
    .WithHttpEndpoint(54622);

builder.Build().Run();
```

This configuration clones the specified Git repository into your application, making it available for further
development and deployment.

## Contributing

Contributions are welcome! If you have any suggestions, bug reports, or feature requests, please open an issue or submit
a pull request on GitHub.

## License

This project is licensed under the MIT License. See the LICENSE file for details.