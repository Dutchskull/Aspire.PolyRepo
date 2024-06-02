namespace Dutchskull.Aspire.Git;

public class FileSystem : IFileSystem
{
    public bool DirectoryExists(string path) => Directory.Exists(path);

    public bool FileExists(string path) => File.Exists(path);

    public bool FileOrDirectoryExists(string path) => Directory.Exists(path) || File.Exists(path);
}