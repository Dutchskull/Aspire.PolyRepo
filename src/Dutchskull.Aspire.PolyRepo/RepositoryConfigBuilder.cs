using Dutchskull.Aspire.PolyRepo.Interfaces;

namespace Dutchskull.Aspire.PolyRepo;

public class RepositoryConfigBuilder
{
    private string? _branch;
    private IFileSystem? _fileSystem;
    private string _gitUrl = string.Empty;
    private bool _keepUpToDate;
    private IProcessCommandExecutor? _processCommandsExecutor;
    private string _targetPath = ".";
    private GitConfig? _gitConfig;

    public RepositoryConfig Build()
    {
        if (string.IsNullOrEmpty(_gitUrl))
        {
            throw new NoRepositoryUrlException();
        }

        string gitProjectName = GitUrlUtilities.GetProjectNameFromGitUrl(_gitUrl);
        string resolvedRepositoryPath = Path.Combine(Path.GetFullPath(_targetPath), gitProjectName);

        return new RepositoryConfig
        {
            GitConfig = _gitConfig ?? new GitConfigBuilder().SetUrl(_gitUrl).Build(),
            RepositoryPath = resolvedRepositoryPath,
            Branch = _branch,
            KeepUpToDate = _keepUpToDate,
            ProcessCommandsExecutor = _processCommandsExecutor ?? new ProcessCommandExecutor(),
            FileSystem = _fileSystem ?? new FileSystem()
        };
    }

    public RepositoryConfigBuilder WithTargetPath(string targetPath)
    {
        _targetPath = targetPath;

        return this;
    }

    public RepositoryConfigBuilder WithDefaultBranch(string branch)
    {
        _branch = branch;

        return this;
    }

    public RepositoryConfigBuilder KeepUpToDate()
    {
        _keepUpToDate = true;

        return this;
    }

    internal RepositoryConfigBuilder WithFileSystem(IFileSystem? fileSystem)
    {
        _fileSystem = fileSystem;

        return this;
    }

    internal RepositoryConfigBuilder WithGitConfig(GitConfig gitConfig) {
        _gitConfig = gitConfig;

        return this;
    }

    internal RepositoryConfigBuilder WithGitUrl(string gitUrl)
    {
        _gitUrl = gitUrl;

        return this;
    }

    internal RepositoryConfigBuilder WithProcessCommandExecutor(IProcessCommandExecutor? processCommandsExecutor)
    {
        _processCommandsExecutor = processCommandsExecutor;

        return this;
    }
}