using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aspire.Git;

public static class ProjectResourceBuilderExtensions
{
    public static IResourceBuilder<GitRepositoryResource> AddGitRepository(
        this IDistributedApplicationBuilder builder,
        string gitUrl,
        string? name = null,
        string repositoryPath = ".",
        string relativeProjectPath = ".")
    {
        string gitProjectName = GetProjectNameFromGitUrl(gitUrl);
        string resolvedRepositoryPath = Path.GetFullPath(Path.Combine(Path.GetFullPath(repositoryPath), name ?? gitProjectName));

        string projectName = name ?? gitProjectName;

        string resolvedProjectPath = Path.GetFullPath(Path.Join(resolvedRepositoryPath, relativeProjectPath));

        GitRepositoryResource gitRepositoryResource = new(projectName, resolvedRepositoryPath, resolvedProjectPath);

        if (!Directory.Exists(gitRepositoryResource.RepositoryPath))
        {
            ProcessCommands.CloneGitRepository(gitUrl, gitRepositoryResource.RepositoryPath);
        }

        VerifyProjectLocation(resolvedProjectPath, gitRepositoryResource);

        return builder.CreateResourceBuilder(gitRepositoryResource);
    }

    private static void VerifyProjectLocation(string resolvedProjectPath, GitRepositoryResource gitRepositoryResource)
    {
        if (IsFolder(resolvedProjectPath))
        {
            if (Directory.Exists(gitRepositoryResource.ProjectPath))
            {
                return;
            }

            string message = string.Format("Project folder {0} not found", gitRepositoryResource.ProjectPath);
            throw new Exception(message);
        }
        else
        {
            if (File.Exists(gitRepositoryResource.ProjectPath))
            {
                return;
            }

            string message = string.Format("Project file {0} not found", gitRepositoryResource.ProjectPath);
            throw new Exception(message);
        }
    }

    private static bool IsFolder(string path)
    {
        return Directory.Exists(path);
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