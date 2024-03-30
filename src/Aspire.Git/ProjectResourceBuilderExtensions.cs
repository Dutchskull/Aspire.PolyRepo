using System.Diagnostics;

namespace Aspire.Git;

public static class ProjectResourceBuilderExtensions
{
    public static IResourceBuilder<ProjectResource> AddGitProject(
        this IDistributedApplicationBuilder builder,
        string gitUrl,
        string repositoryPath,
        string relativeProjectPath,
        string? name = null)
    {
        string gitProjectName = GetProjectNameFromGitUrl(gitUrl);
        string projectName = name ?? gitProjectName;

        string resolvedRepositoryPath = Path.Combine(Path.GetFullPath(repositoryPath), projectName);

        bool hasRespository = Directory.Exists(resolvedRepositoryPath);
        if (!hasRespository)
        {
            CloneGitRepository(gitUrl, resolvedRepositoryPath);
        }

        string resolvedProjectPath = Path.Join(resolvedRepositoryPath, relativeProjectPath);

        bool hasProject = File.Exists(resolvedProjectPath);
        if (!hasProject)
        {
            string message = string.Format("Project folder {0} not found", resolvedProjectPath);
            throw new Exception(message);
        }

        BuildDotNetProject(resolvedProjectPath);

        return builder.AddProject(projectName, resolvedProjectPath);
    }

    private static void BuildDotNetProject(string resolvedProjectPath)
    {
        RunProcess("dotnet", $"build {resolvedProjectPath}");
    }

    private static void CloneGitRepository(string gitUrl, string resolvedRepositoryPath)
    {
        RunProcess("git", $"clone {gitUrl} {resolvedRepositoryPath}");
    }

    private static void RunProcess(string fileName, string arguments)
    {
        Process process = new()
        {
            StartInfo = new()
            {
                FileName = fileName,
                Arguments = arguments,
            }
        };

        process.Start();
        process.WaitForExit();
    }

    private static string GetProjectNameFromGitUrl(string gitUrl)
    {
        if (gitUrl.EndsWith(".git"))
        {
            gitUrl = gitUrl[..^4];
        }

        return gitUrl.Split('/')[^1];
    }
}