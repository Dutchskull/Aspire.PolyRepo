using Dutchskull.Aspire.Git.Interfaces;

namespace Dutchskull.Aspire.Git;

public class GitRepositoryConfigBuilder
{
    private string _branch = "main";
    private string _cloneTargetPath = ".";
    private IFileSystem? _fileSystem;
    private string _gitUrl = string.Empty;
    private string? _name;
    private IProcessCommandsExecutor? _processCommandsExecutor;
    private string _projectPath = ".";

    public GitRepositoryConfig Build()
    {
        if (string.IsNullOrEmpty(_gitUrl))
        {
            throw new InvalidOperationException("GitUrl must be provided");
        }

        return new GitRepositoryConfig
        {
            GitUrl = _gitUrl,
            Name = _name,
            CloneTargetPath = _cloneTargetPath,
            ProjectPath = _projectPath,
            Branch = _branch,
            ProcessCommandsExecutor = _processCommandsExecutor ?? new ProcessCommandExecutor(),
            FileSystem = _fileSystem ?? new FileSystem(),
        };
    }

    public GitRepositoryConfigBuilder WithCloneTargetPath(string cloneTargetPath)
    {
        _cloneTargetPath = cloneTargetPath;
        return this;
    }

    public GitRepositoryConfigBuilder WithDefaultBranch(string branch)
    {
        _branch = branch;
        return this;
    }

    public GitRepositoryConfigBuilder WithFileSystem(IFileSystem? fileSystem)
    {
        _fileSystem = fileSystem;
        return this;
    }

    public GitRepositoryConfigBuilder WithGitUrl(string gitUrl)
    {
        _gitUrl = gitUrl;
        return this;
    }

    public GitRepositoryConfigBuilder WithName(string? name)
    {
        _name = name;
        return this;
    }

    public GitRepositoryConfigBuilder WithProcessCommandExecutor(IProcessCommandsExecutor? processCommandsExecutor)
    {
        _processCommandsExecutor = processCommandsExecutor;
        return this;
    }

    /// <summary>
    /// Adds path to the project
    /// </summary>
    /// <param name="projectPath">This path is relative to where the CloningPath provided.</param>
    /// <returns></returns>
    public GitRepositoryConfigBuilder WithProjectPath(string projectPath)
    {
        _projectPath = projectPath;
        return this;
    }
}