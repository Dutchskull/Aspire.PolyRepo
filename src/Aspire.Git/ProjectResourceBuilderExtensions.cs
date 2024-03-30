using System.Diagnostics;

namespace Aspire.Git;

public static class ProjectResourceBuilderExtensions
{
    public static IResourceBuilder<ProjectResource> AddGitProject(
        this IDistributedApplicationBuilder builder,
        string gitUrl,
        string? name = null,
        string repositoryPath = ".",
        string relativeProjectPath = ".")
    {
        string gitProjectName = GetProjectNameFromGitUrl(gitUrl);
        string resolvedRepositoryPath = Path.Combine(Path.GetFullPath(repositoryPath), gitProjectName);

        if (!Directory.Exists(resolvedRepositoryPath))
        {
            CloneGitRepository(gitUrl, resolvedRepositoryPath);
        }

        string projectName = name ?? gitProjectName;
        string resolvedProjectPath = Path.Join(resolvedRepositoryPath, relativeProjectPath);

        if (!File.Exists(resolvedProjectPath))
        {
            string message = string.Format("Project folder {0} not found", resolvedProjectPath);
            throw new Exception(message);
        }

        return builder.AddProject(projectName, resolvedProjectPath);
    }

    private static void CloneGitRepository(string gitUrl, string resolvedRepositoryPath)
    {
        Process process = new()
        {
            StartInfo = new()
            {
                FileName = "git",
                Arguments = $"clone {gitUrl} {resolvedRepositoryPath}",
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